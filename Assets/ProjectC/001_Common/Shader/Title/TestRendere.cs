using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DynamicStencilRenderer : MonoBehaviour
{
    public Material uvShiftMaterial;  // UV操作用マテリアル
    public Material stencilMaterial; // ステンシル処理用マテリアル
    private RenderTexture renderTexture;
    private CommandBuffer commandBuffer;

    [NonSerialized]
    private Graphic _graphic;
    public Graphic graphic => _graphic ? _graphic : _graphic = GetComponent<Graphic>();


    [Range(0, 1)]
    public float uvOffsetSpeed = 0.1f; // 毎フレームUVを変化させる速度
    private Vector2 uvOffset;

    private float m_degree;
    bool m_finish = false;

    public void Start()
    {

        //// RenderTextureを作成
        //renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
        //renderTexture.Create();

        //// シェーダーにRenderTextureを設定
        //uvShiftMaterial.SetTexture("_MainTex", renderTexture);
        //stencilMaterial.SetTexture("_MainTex", renderTexture);

        //// 初期化
        //commandBuffer = new CommandBuffer();
        //commandBuffer.name = "Dynamic UV and Stencil Rendering";
    }

    public void Update()
    {
        if(graphic)
        {
            graphic.SetMaterialDirty();
        }
        

        if (gameObject.TryGetComponent(out RectTransform rect))
        {
            Vector3 position = rect.position;

            if (m_degree >= 360.0f)
            {
                m_degree -= 360.0f;
            }

            m_degree++;

            position.x += 0.1f * Mathf.Cos(m_degree);

           // rect.position = position;

        }

        // UV Offsetを時間に応じて変化
        //uvOffset.x += Time.deltaTime * uvOffsetSpeed;
        //uvOffset.y += Time.deltaTime * uvOffsetSpeed;

        //// マテリアルに新しいUVオフセットを設定
        //uvShiftMaterial.SetVector("_UVOffset", uvOffset);

        ////// CommandBufferを再構築して毎フレーム更新
        //commandBuffer.Clear();

        ////// UV操作の結果をRenderTextureに描画
        //commandBuffer.SetRenderTarget(renderTexture);
        //commandBuffer.ClearRenderTarget(true, true, Color.clear);
        //////commandBuffer.DrawRenderer(GetComponent<Renderer>(), uvShiftMaterial);

        //// Graphics.Blitを使用してRenderTextureに加工結果を描画
        //commandBuffer.Blit(null, renderTexture, uvShiftMaterial);

        //// コマンドバッファを実行
        //Graphics.ExecuteCommandBuffer(commandBuffer);
    }

    public void OnDestroy()
    {
        //if (renderTexture != null)
        //    renderTexture.Release();

        //if (commandBuffer != null)
        //    commandBuffer.Release();
    }
}

