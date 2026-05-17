/* @file CleaningAssignEvent.cs
 * @brief 任意のTagのコライダーが接触した際に行われる汚れ自体の挙動を制御するスクリプト
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using ExternalPropertyAttributes;

/// <summary>
/// @brief 汚れ自体の接触時の挙動を制御するスクリプト
/// </summary>
public class CleaningAssignEvent : BaseAssignEventObject
{
    private DecalProjector m_projector = null;


    //[SerializeField,Label("成功時に上昇する評価値")] int m_satisfactionValue = 1;

    [HideInInspector]// イベント終了を設定するための参照
    public GenerateCleaningEvent m_generateCleaningEvent;


    [SerializeField] private Texture2D m_dirtTexture;

    [SerializeField] private Shader m_copyShader;

    private AsyncOperationHandle<GameObject> CleaningFoamHandle;
    private AsyncOperationHandle<GameObject> CleaningCompleteHandle;

    //! @brief 接触時のイベント 汚れを強調表示する
    protected override void OnCollisionTriggerEvent()
    {
        m_projector.material.SetFloat("_EmphasisValue", 1.0f);
    }

    //! @brief 接触終了時のイベント 汚れの強調表示を解除する
    protected override void OnCollisionTriggerExitEvent()
    {
        m_projector.material.SetFloat("_EmphasisValue", 0.0f);
    }

    //! @brief アクセスされたかどうかの定義 Gathering(Eキー)を押すとアクセスされる
    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        return input.Cleanning;
    }

    //! @brief アクセス時のイベント 汚れを消す
    public override async void OnCollisionAccessEvent()
    {
        // 所持状態なら早期リターン
        if (m_cCore?.PlayerParameters.m_isFoodHold == true) return;

        var CleaningFoamObj = Instantiate(CleaningFoamHandle.Result, transform);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        Destroy(CleaningFoamObj);

        //ManagementGameDataManager.instance.AddSatisfactionValue(m_satisfactionValue);

        if (this == null) return;
        var CleaningCompleteObj = Instantiate(CleaningCompleteHandle.Result, transform.position, Quaternion.identity);

        if (m_generateCleaningEvent != null)
        {
            m_generateCleaningEvent.SetEventEnd(ManagementGameInfo.EventSolutionType.Solution);
        }
        Destroy(gameObject);
    }



    private void Start()
    {
        base.Start();

        m_projector = GetComponent<DecalProjector>();

        if (m_projector != null)
        {
            // マテリアルをコピーしておかないと末尾の情報で固定されてしまうためコピー
            m_projector.material = new(m_copyShader);
            m_projector.material.SetTexture("_MainTex", m_dirtTexture);
        }


        CleaningFoamHandle = Addressables.LoadAssetAsync<GameObject>("CleaningFoam");
        CleaningCompleteHandle = Addressables.LoadAssetAsync<GameObject>("CleaningComplete");
    }

    private void OnDestroy()
    {
        // マテリアルを新しく作成したので明示的に削除しておく
        // https://www.create-forever.games/unity-material-memory-leak/
        Destroy(m_projector.material);
    }
}
