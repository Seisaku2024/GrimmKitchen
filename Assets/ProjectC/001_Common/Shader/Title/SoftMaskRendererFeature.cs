using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class SoftMaskRendererFeature : ScriptableRendererFeature
{
    public class SoftMaskRendererPass : ScriptableRenderPass
    {
        private Material _material;
        private Material _maskMaterial;
        private Texture2D _mainTex;

        private RenderTexture _renderTexture;

        private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

        private class PassData
        {
            public Material material;
            public Material maskMaterial;
            public Texture2D mainTex;
            public RenderTexture renderTexture;
        }

        public SoftMaskRendererPass(Material material, Material maskMaterial, Texture2D mainTex)
        {
            SetParameters(material, maskMaterial, mainTex);
            CreateRenderTexture();
        }

        public void SetParameters(Material material, Material maskMaterial, Texture2D mainTex)
        {
            _material = material;
            _maskMaterial = maskMaterial;
            _mainTex = mainTex;
        }

        public void Setup(RenderPassEvent passEvent)
        {
            renderPassEvent = passEvent;
        }

        private void CreateRenderTexture()
        {
            if (_renderTexture != null)
            {
                return;
            }

            _renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32)
            {
                name = "_SoftMaskRT",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Repeat,
                useMipMap = false,
                autoGenerateMips = false
            };

            _renderTexture.Create();
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null || _maskMaterial == null || _mainTex == null)
            {
                return;
            }

            CreateRenderTexture();

            if (_renderTexture == null)
            {
                return;
            }

            using (var builder = renderGraph.AddUnsafePass<PassData>("SoftMask", out var passData))
            {
                passData.material = _material;
                passData.maskMaterial = _maskMaterial;
                passData.mainTex = _mainTex;
                passData.renderTexture = _renderTexture;

                builder.AllowPassCulling(false);
                builder.AllowGlobalStateModification(true);

                builder.SetRenderFunc((PassData data, UnsafeGraphContext context) =>
                {
                    if (data.material == null ||
                        data.maskMaterial == null ||
                        data.mainTex == null ||
                        data.renderTexture == null)
                    {
                        return;
                    }

                    var cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);

                    data.renderTexture.wrapMode = TextureWrapMode.Repeat;

                    cmd.Blit(
                        data.mainTex,
                        data.renderTexture,
                        data.material
                    );

                    data.maskMaterial.SetTexture(MainTexId, data.renderTexture);
                });
            }
        }

        public void Dispose()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                CoreUtils.Destroy(_renderTexture);
                _renderTexture = null;
            }
        }
    }

    [SerializeField] private Material _material1;
    [SerializeField] private Material _maskMaterial;
    [SerializeField] private Texture2D _mainTexture;

    [SerializeField] private RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    private SoftMaskRendererPass _pass;

    public override void Create()
    {
        _pass = new SoftMaskRendererPass(_material1, _maskMaterial, _mainTexture);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_pass == null)
        {
            return;
        }

        if (_material1 == null || _maskMaterial == null || _mainTexture == null)
        {
            return;
        }

        _pass.SetParameters(_material1, _maskMaterial, _mainTexture);
        _pass.Setup(_passEvent);

        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;


//[System.Serializable]
//public class SoftMaskRendererFeature : ScriptableRendererFeature
//{
//    public class SoftMaskRendererPass : ScriptableRenderPass
//    {
//        Material m_material;
//        Material m_maskMaterial;
//        Texture2D m_mainTex;

//        private RenderTexture m_renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);

//        public SoftMaskRendererPass(Material material, Material maskMaterial, Texture2D mainTex)
//        {
//            m_material = material;
//            m_maskMaterial = maskMaterial;
//            m_mainTex = mainTex;

//        }

//        public void Setup(RenderPassEvent _renderPass)
//        {
//            renderPassEvent = _renderPass;
//        }


//        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
//        {
//            if (m_material == null || m_maskMaterial == null)
//            {
//                return;
//            }

//            //コマンドリスト取得(コマンドリストをレンタルする)
//            var cmd = CommandBufferPool.Get("SoftMask");
//            //中身一応クリア
//            cmd.Clear();

//            //現在のRTの情報を取得
//            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
//            tempTargetDescriptor.depthBufferBits = 0;

//            //ソース画像
//            int tempRT = Shader.PropertyToID("_Temp");
//            cmd.GetTemporaryRT(tempRT, tempTargetDescriptor);


//            //ステンシル用画像
//            int tempMaskRTID = Shader.PropertyToID("_TempMaskRT");
//            cmd.GetTemporaryRT(tempMaskRTID, 280, 280, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

//            //var tempRT= RenderTexture.GetTemporary(tempTargetDescriptor);
//            //tempRT.wrapMode = TextureWrapMode.Repeat;


//            // RenderTextureを作成
//            //RenderTexture renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);

//            if (m_renderTexture == null)
//            {
//                return;
//            }

//            m_renderTexture.Release();
//            // WrapModeを設定
//            m_renderTexture.wrapMode = TextureWrapMode.Repeat;

//            ////RTにカメラ画像をコピー
//            cmd.Blit
//                (
//                m_mainTex,
//                 m_renderTexture,
//                 m_material
//                 );


//            //cmd.SetGlobalTexture("_BackTex", renderTexture);
//            m_maskMaterial.SetTexture("_MainTex", m_renderTexture);



//            //RenderTexture.DestroyImmediate(renderTexture);

//            //RT解放
//            cmd.ReleaseTemporaryRT(tempRT);
//            cmd.ReleaseTemporaryRT(tempMaskRTID);

//            //解放
//             m_renderTexture.Release();

//            //コマンドバッファをGPUに転送
//            context.ExecuteCommandBuffer(cmd);
//            //返却
//            CommandBufferPool.Release(cmd);

//        }


//    }

//    [SerializeField] Material _material1;
//    [SerializeField] Material _maskMaterial;
//    [SerializeField] Texture2D _mainTexture;

//    [SerializeField] RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

//    SoftMaskRendererPass _pass;

//    public override void Create()
//    {
//        //作成は必ずCreateに作る
//        _pass = new SoftMaskRendererPass(_material1, _maskMaterial, _mainTexture);
//    }

//    /// <summary>
//    /// パスを追加される必要がある時に実行される
//    /// </summary>
//    /// <param name="renderer"></param>
//    /// <param name="renderingData"></param>
//    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//    {
//        //描画として動かせるようになった
//        renderer.EnqueuePass(_pass);
//    }

//    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
//    {
//        _pass.Setup(_passEvent);
//    }


//}
