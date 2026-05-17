using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class MapRendererFeature : ScriptableRendererFeature
{
    public class MapRenderPass : ScriptableRenderPass
    {
        public enum Passes
        {
            Contrast,
            HSV,
            GrayToon
        }

        private Material _material;

        private static readonly int ContrastPowerId = Shader.PropertyToID("_ContrastPower");
        private static readonly int HueId = Shader.PropertyToID("_Hue");
        private static readonly int SaturationId = Shader.PropertyToID("_Saturation");
        private static readonly int ValueId = Shader.PropertyToID("_Value");
        private static readonly int WhiteColorId = Shader.PropertyToID("_WhiteColor");
        private static readonly int GrayColorId = Shader.PropertyToID("_GrayColor");
        private static readonly int DarkColorId = Shader.PropertyToID("_DarkColor");

        public MapRenderPass(Shader shader)
        {
            if (shader != null)
            {
                _material = CoreUtils.CreateEngineMaterial(shader);
            }
        }

        public void Setup(RenderPassEvent passEvent)
        {
            renderPassEvent = passEvent;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null)
            {
                return;
            }

            var resourceData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            // BackBuffer は RenderGraph の Blit 元にできないためスキップ
            if (resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            var volumeStack = VolumeManager.instance.stack;
            var volume = volumeStack.GetComponent<MapPostProcessVolume>();

            if (volume == null)
            {
                return;
            }

            var cameraDescriptor = cameraData.cameraTargetDescriptor;
            cameraDescriptor.depthBufferBits = 0;

            TextureHandle cameraColor = resourceData.activeColorTexture;

            TextureHandle tempRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_MapTempRT",
                false
            );

            TextureHandle contrastRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_MapContrastRT",
                false
            );

            TextureHandle toonRT = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                cameraDescriptor,
                "_MapToonRT",
                false
            );


            // ② コントラスト
            _material.SetFloat(ContrastPowerId, volume.ContrastPower.value);

            AddBlitPass(
                renderGraph,
                "Map Contrast",
                cameraColor,
                contrastRT,
                _material,
                (int)Passes.Contrast
             );

            // ③ HSV
            _material.SetFloat(HueId, volume.HueShift.value);
            _material.SetFloat(SaturationId, volume.Saturation.value);
            _material.SetFloat(ValueId, volume.Value.value);

            AddBlitPass(
                renderGraph,
                "Map HSV",
                contrastRT,
                tempRT,
                _material,
                (int)Passes.HSV
            );

            TextureHandle finalTexture = tempRT;

            // ④ Toon
            if (volume.ToonAble.value)
            {
                _material.SetColor(WhiteColorId, Color.white);
                _material.SetColor(GrayColorId, Color.red);
                _material.SetColor(DarkColorId, Color.black);

                AddBlitPass(
                    renderGraph,
                    "Map Gray Toon",
                    tempRT,
                    toonRT,
                    _material,
                    (int)Passes.GrayToon
                );

                finalTexture = toonRT;
            }

            // 最終結果をカメラカラーとして差し替え
            resourceData.cameraColor = finalTexture;
        }

        private static void AddBlitPass(
            RenderGraph renderGraph,
            string passName,
            TextureHandle source,
            TextureHandle destination,
            Material material,
            int shaderPass)
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

    [SerializeField] private Shader _shader;
    [SerializeField] private RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    private MapRenderPass _pass;

    public override void Create()
    {
        _pass = new MapRenderPass(_shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_shader == null || _pass == null)
        {
            return;
        }

        _pass.Setup(_passEvent);
        renderer.EnqueuePass(_pass);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;
//using static UnityEngine.Rendering.PostProcessing.PostProcessResources;

//public class MapRendererFeature : ScriptableRendererFeature
//{
//    public class MapRenderePass : ScriptableRenderPass
//    {
//        public enum Passes
//        {
//            Contrast,
//            HSV,
//            GrayToon
//        }

//        Material m_material;
//        public MapRenderePass(Shader shader)
//        {
//            //シェーダーからマテリアルの作成
//            m_material = CoreUtils.CreateEngineMaterial(shader);
//        }

//        /// <summary>
//        /// レンダラーパス順のセット
//        /// </summary>
//        /// <param name="_renderPass"></param>
//        public void Setup(RenderPassEvent _renderPass)
//        {
//            renderPassEvent = _renderPass;
//        }

//        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
//        {
//            if (m_material == null) return;

//            //Volumeを取得
//            var volumeStack = VolumeManager.instance.stack;
//            var volum = volumeStack.GetComponent<MapPostProcessVolume>();

//            //コマンドリスト取得(コマンドリストをレンタルする)
//            var cmd = CommandBufferPool.Get("Map Post Prosess");
//            //中身一応クリア
//            cmd.Clear();

//            //現在のRTの情報を取得
//            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
//            tempTargetDescriptor.depthBufferBits = 0;

//            //ソース画像
//            int tempRTID = Shader.PropertyToID("_TempRT");
//            cmd.GetTemporaryRT(tempRTID, tempTargetDescriptor);

//            int contrastRTID = Shader.PropertyToID("_ContrastRT");
//            cmd.GetTemporaryRT(contrastRTID, tempTargetDescriptor);

//            int toonRTID = Shader.PropertyToID("_ToonRT");
//            cmd.GetTemporaryRT(toonRTID, tempTargetDescriptor);



//            //============================================================================
//            //①現在の画面をRTへコピー
//            //===========================================================================

//            //RTにカメラ画像をコピー
//            cmd.Blit
//                (
//                 renderingData.cameraData.renderer.cameraColorTargetHandle,
//                 tempRTID);

//            //======================================================================
//            //②加工画像作成
//            //======================================================================  

//            m_material.SetFloat("_ContrastPower", volum.ContrastPower.value);
//            //描画(コントラスト)
//            cmd.Blit
//                (
//                tempRTID,//描画元画像
//                contrastRTID,//描画先画像
//                m_material, ((int)Passes.Contrast)
//                );

//            // 描画（HSV）
//            m_material.SetFloat("_Hue", volum.HueShift.value);
//            m_material.SetFloat("_Saturation", volum.Saturation.value);
//            m_material.SetFloat("_Value", volum.Value.value);
//            cmd.Blit
//                (
//                contrastRTID,//描画元画像
//                tempRTID,//描画先画像
//                m_material,((int)Passes.HSV)
//                );

//            // トゥーンやるかどうか
//            if(volum.ToonAble.value)
//            {
//                //描画（GrayToon） 画面へ
//                m_material.SetColor("_WhiteColor", Color.white);
//                m_material.SetColor("_GrayColor", Color.red);
//                m_material.SetColor("_DarkColor", Color.black);
//                cmd.Blit
//                    (
//                    tempRTID,//描画元画像
//                    toonRTID,//描画先画像
//                    m_material, ((int)Passes.GrayToon)
//                    );
//               cmd.Blit
//                    (
//                    toonRTID,//描画元画像
//                    tempRTID//描画先画像
//                    );
//            }

//            //　画面
//            cmd.Blit
//                (
//                tempRTID,//描画元画像
//                renderingData.cameraData.renderer.cameraColorTargetHandle//描画先画像
//                );


//            //======================================================================
//            //後片付け
//            //======================================================================

//            //RT解放
//            cmd.ReleaseTemporaryRT(tempRTID);
//            cmd.ReleaseTemporaryRT(contrastRTID);

//            //コマンドバッファをGPUに転送
//            context.ExecuteCommandBuffer(cmd);
//            //返却
//            CommandBufferPool.Release(cmd);

//        }
//    }



//    [SerializeField] Shader _shader;
//    [SerializeField] RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;
//    MapRenderePass _pass;

//    public override void Create()
//    {
//        //作成は必ずCreateに作る
//        _pass = new MapRenderePass(_shader);
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
