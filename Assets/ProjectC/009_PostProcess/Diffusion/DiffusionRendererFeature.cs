using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class DiffusionRendererFeature : ScriptableRendererFeature
{
    public class DiffusionRenderPass : ScriptableRenderPass
    {
        private Material _material;
        private Material _material2;

        private static readonly int BackTexId = Shader.PropertyToID("_BackTex");
        private static readonly int DispersionId = Shader.PropertyToID("_Dispersion");
        private static readonly int SamplingTexelAmountId = Shader.PropertyToID("_SmaplingTexelAmount");
        private static readonly int DirectionId = Shader.PropertyToID("_Direction");
        private static readonly int BlendId = Shader.PropertyToID("_Blend");

        public enum Passes
        {
            Blur,
            BlendScreen,
            ComparisonBright,
            SelfMultiply,
        }

        private class PassData
        {
            public Material material;
            public TextureHandle source;
            public TextureHandle backTex;
            public Vector4 direction;
            public float dispersion;
            public int samplingTexelAmount;
            public float blend;
            public int shaderPass;
        }

        public DiffusionRenderPass(Shader shader)
        {
            if (shader != null)
            {
                _material = CoreUtils.CreateEngineMaterial(shader);
                _material2 = CoreUtils.CreateEngineMaterial(shader);
            }
        }

        public void Setup(RenderPassEvent passEvent)
        {
            renderPassEvent = passEvent;
        }

        private class CopyPassData
        {
            public TextureHandle source;
        }

        private static void AddCopyPass(
            RenderGraph renderGraph,
            string passName,
            TextureHandle source,
            TextureHandle destination)
        {
            using (var builder = renderGraph.AddRasterRenderPass<CopyPassData>(
                       passName,
                       out var passData))
            {
                passData.source = source;

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0);

                builder.AllowPassCulling(false);

                builder.SetRenderFunc((CopyPassData data, RasterGraphContext context) =>
                {
                    Blitter.BlitTexture(
                        context.cmd,
                        data.source,
                        new Vector4(1, 1, 0, 0),
                        0.0f,
                        false
                    );
                });
            }
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null)
            {
                return;
            }

            var resourceData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            // BackBuffer を直接 source にすると RenderGraph で読めないためスキップ
            if (resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            var volumeStack = VolumeManager.instance.stack;
            var volume = volumeStack.GetComponent<DiffusionPostProcessVolume>();

            if (volume == null)
            {
                return;
            }

            var cameraDescriptor = cameraData.cameraTargetDescriptor;
            cameraDescriptor.depthBufferBits = 0;

            int fullW = cameraDescriptor.width;
            int fullH = cameraDescriptor.height;

            TextureHandle cameraColor = resourceData.activeColorTexture;

            TextureHandle tempMultiplyRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_TempMultiplyRT",
                false
            );

            TextureHandle tempComparisonRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_TempComparisonRT",
                false
            );

            var blurDescriptor = cameraDescriptor;
            blurDescriptor.width = Mathf.Max(1, fullW / 2);
            blurDescriptor.height = Mathf.Max(1, fullH / 2);
            blurDescriptor.depthBufferBits = 0;

            TextureHandle tempBlurXRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                blurDescriptor,
                "_TempBlurXRT",
                false
            );

            TextureHandle tempBlurYRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                blurDescriptor,
                "_TempBlurYRT",
                false
            );

            TextureHandle finalRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_DiffusionFinalRT",
                false
            );

            
            // ② 乗算画像作成
            AddBlitPass(
                renderGraph,
                "Diffusion Self Multiply",
                cameraColor,
                tempMultiplyRT,
                _material,
                (int)Passes.SelfMultiply
            );

            // ③ 縮小バッファへコピー
            AddCopyPass(
             renderGraph,
            "Diffusion Downsample",
            tempMultiplyRT,
            tempBlurYRT
            );

            // ④ Xブラー
            AddDiffusionPass(
                renderGraph,
                "Diffusion Blur X",
                tempBlurYRT,
                tempBlurXRT,
                TextureHandle.nullHandle,
                _material,
                new Vector4(1.0f / Mathf.Max(1, blurDescriptor.width), 0, 0, 0),
                volume.GaussDispersion.value,
                volume.GaussSmaplingTexelAmount.value,
                0.0f,
                (int)Passes.Blur
            );

            // ⑤ Yブラー
            AddDiffusionPass(
                renderGraph,
                "Diffusion Blur Y",
                tempBlurXRT,
                tempBlurYRT,
                TextureHandle.nullHandle,
                _material2,
                new Vector4(0, 1.0f / Mathf.Max(1, blurDescriptor.height), 0, 0),
                volume.GaussDispersion.value,
                volume.GaussSmaplingTexelAmount.value,
                0.0f,
                (int)Passes.Blur
            );

            // ⑥ 比較明
            AddDiffusionPass(
                renderGraph,
                "Diffusion Comparison Bright",
                cameraColor,
                tempComparisonRT,
                tempBlurYRT,
                _material,
                Vector4.zero,
                0.0f,
                0,
                0.0f,
                (int)Passes.ComparisonBright
            );

            // ⑦ スクリーン合成
            // 元コードは ComparisonBright を使っていましたが、
            // enum 名から見るとここは BlendScreen の可能性が高いです。
            AddDiffusionPass(
                renderGraph,
                "Diffusion Blend Screen",
                tempMultiplyRT,
                finalRT,
                tempComparisonRT,
                _material,
                Vector4.zero,
                0.0f,
                0,
                volume.ScreenBlend.value,
                (int)Passes.BlendScreen
            );

            // 最終結果をカメラカラーとして差し替える
            resourceData.cameraColor = finalRT;
        }

        private static void AddBlitPass(
            RenderGraph renderGraph,
            string passName,
            TextureHandle source,
            TextureHandle destination,
            Material material,
            int shaderPass)
        {
            if (material == null)
            {
                var blitParams = new RenderGraphUtils.BlitMaterialParameters(
                    source,
                    destination,
                    null,
                    0
                );

                renderGraph.AddBlitPass(blitParams, passName);
            }
            else
            {
                var blitParams = new RenderGraphUtils.BlitMaterialParameters(
                    source,
                    destination,
                    material,
                    shaderPass
                );

                renderGraph.AddBlitPass(blitParams, passName);
            }
        }

        private static void AddDiffusionPass(
            RenderGraph renderGraph,
            string passName,
            TextureHandle source,
            TextureHandle destination,
            TextureHandle backTex,
            Material material,
            Vector4 direction,
            float dispersion,
            int samplingTexelAmount,
            float blend,
            int shaderPass)
        {
            using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName, out var passData))
            {
                passData.material = material;
                passData.source = source;
                passData.backTex = backTex;
                passData.direction = direction;
                passData.dispersion = dispersion;
                passData.samplingTexelAmount = samplingTexelAmount;
                passData.blend = blend;
                passData.shaderPass = shaderPass;

                builder.UseTexture(source, AccessFlags.Read);

                if (backTex.IsValid())
                {
                    builder.UseTexture(backTex, AccessFlags.Read);
                }

                builder.SetRenderAttachment(destination, 0);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                {
                    if (data.material == null)
                    {
                        return;
                    }

                    data.material.SetFloat(DispersionId, data.dispersion);
                    data.material.SetInt(SamplingTexelAmountId, data.samplingTexelAmount);
                    data.material.SetVector(DirectionId, data.direction);
                    data.material.SetFloat(BlendId, data.blend);

                    if (data.backTex.IsValid())
                    {
                        context.cmd.SetGlobalTexture(BackTexId, data.backTex);
                    }

                    Blitter.BlitTexture(
                        context.cmd,
                        data.source,
                        Vector4.one,
                        data.material,
                        data.shaderPass
                    );
                });
            }
        }
    }

    [SerializeField] private Shader _shader;
    [SerializeField] private RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    private DiffusionRenderPass _pass;

    public override void Create()
    {
        _pass = new DiffusionRenderPass(_shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_shader == null)
        {
            return;
        }

        _pass.Setup(_passEvent);

        renderer.EnqueuePass(_pass);
    }

}