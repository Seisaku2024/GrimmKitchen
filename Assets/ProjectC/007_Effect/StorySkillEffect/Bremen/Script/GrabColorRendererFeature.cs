using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Built-in Render Pipeline の GrabPass に近い RendererFeature。
/// 現在の CameraColor をコピーし、grabbedTextureName の名前で
/// グローバルテクスチャとしてシェーダーから参照できるようにする。
/// </summary>
public class GrabColorRendererFeature : ScriptableRendererFeature
{
    public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    public string grabbedTextureName = "g_GrabbedTexture";

    private GrabColorPass _pass;

    public override void Create()
    {
        _pass = new GrabColorPass(grabbedTextureName, renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_pass == null)
        {
            return;
        }

        _pass.Setup(grabbedTextureName, renderPassEvent);
        renderer.EnqueuePass(_pass);
    }

    private class GrabColorPass : ScriptableRenderPass
    {
        private int _texturePropertyId;

        private class PassData
        {
            public TextureHandle source;
            public TextureHandle destination;
        }

        public GrabColorPass(string textureName, RenderPassEvent passEvent)
        {
            Setup(textureName, passEvent);
        }

        public void Setup(string textureName, RenderPassEvent passEvent)
        {
            renderPassEvent = passEvent;
            _texturePropertyId = Shader.PropertyToID(textureName);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            if (resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            TextureHandle source = resourceData.activeColorTexture;

            var descriptor = cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            TextureHandle grabbedTexture = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph,
                descriptor,
                "_GrabbedColorTexture",
                false
            );

            using (var builder = renderGraph.AddRasterRenderPass<PassData>(
                       "Grab Color",
                       out var passData))
            {
                passData.source = source;
                passData.destination = grabbedTexture;

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(grabbedTexture, 0);

                builder.AllowPassCulling(false);
                builder.AllowGlobalStateModification(true);

                builder.SetGlobalTextureAfterPass(grabbedTexture, _texturePropertyId);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                {
                    Blitter.BlitTexture(
                        context.cmd,
                        data.source,
                        Vector4.one,
                        0,
                        false
                    );
                });
            }
        }
    }
}


//using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

///// <summary>
///// Built-in Render PipelineのGrabPassに近いRendererFeature
///// RenderObjectsRendererFeatureでGrab対象のオブジェクトを描画し、
///// そのあとにGrabColorRendererFeatureを設定することで任意のタイミングでキャプチャ、
///// 以降のシェーダーでgrabbedTextureNameで指定した名前でキャプチャしたテクスチャを参照できる
/////
///// 例：Opaqueの一部のオブジェクトをキャプチャする場合、UniversalRenderDataをInspector上で以下のように設定する
///// 　1. キャプチャする対象オブジェクトに専用のレイヤー（以下GrabTargetLayer）を設定
///// 　2. UniversalRendererDataのopaqueLayerマスクからGrabTargetLayerを除外
///// 　3. RenderObjectsRendererFeatureをRenderFeatureに追加
///// 　   - EventをBeforeRenderingOpaquesにセット
///// 　   - Filters>LayerMaskでGrabTargetLayerをセット
///// 　4. 3.の次になるようにGrabColorRenderFeatureを設定
/////      - RenderPassEventをBeforeRenderingOpaquesにセット
///// </summary>
//public class GrabColorRendererFeature : ScriptableRendererFeature
//{
//    public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
//    public string grabbedTextureName = "g_GrabbedTexture";

//    private GrabColorPass _pass;

//    public override void Create()
//    {
//        _pass = new(grabbedTextureName, renderPassEvent);
//    }

//    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//    {
//        renderer.EnqueuePass(_pass);
//    }


//    private class GrabColorPass : ScriptableRenderPass
//    {
//        private readonly RTHandle _textureHandle;
//        private readonly int _texturePropertyId;

//        public GrabColorPass(string textureName, RenderPassEvent renderPassEvent)
//        {
//            this.renderPassEvent = renderPassEvent;
//            _texturePropertyId = Shader.PropertyToID(textureName);
//            _textureHandle = RTHandles.Alloc(_texturePropertyId, textureName);
//        }

//        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
//        {
//            cmd.GetTemporaryRT(_texturePropertyId, cameraTextureDescriptor);
//        }

//        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
//        {
//            var cmd = CommandBufferPool.Get(nameof(GrabColorPass));

//#if UNITY_2022_2_OR_NEWER
//            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;
//            if (source.rt != null)
//            {
//                Blit(cmd, source, _textureHandle);
//            }
//#else
//            Blit(cmd, renderingData.cameraData.renderer.cameraColorTarget, _textureHandle);
//#endif
//            context.ExecuteCommandBuffer(cmd);

//            CommandBufferPool.Release(cmd);
//        }

//        public override void OnCameraCleanup(CommandBuffer cmd)
//        {
//            cmd.ReleaseTemporaryRT(_texturePropertyId);
//        }
//    }

//}