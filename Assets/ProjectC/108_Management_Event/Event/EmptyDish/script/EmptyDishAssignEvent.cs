/* @file CleaningAssignEvent.cs
 * @brief 任意のTagのコライダーが接触した際に行われる空皿自体の挙動を制御するスクリプト
 */
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



/// <summary>
/// @brief 汚れ自体の接触時の挙動を制御するスクリプト
/// </summary>
public class EmptyDishAssignEvent : BaseAssignEventObject
{
    //[SerializeField]int m_satisfactionValue = 1;

    private AsyncOperationHandle<GameObject> CleaningFoamHandle;
    private AsyncOperationHandle<GameObject> CleaningCompleteHandle;

    //! @brief 接触時のイベント 汚れを強調表示する
    protected override void OnCollisionTriggerEvent()
    {
    }

    //! @brief 接触終了時のイベント 汚れの強調表示を解除する
    protected override void OnCollisionTriggerExitEvent()
    {
    }

    //! @brief アクセスされたかどうかの定義 Gathering(Eキー)を押すとアクセスされる
    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        return input.Cleanning;
    }

    //! @brief アクセス時のイベント 汚れを消す
    public override async void OnCollisionAccessEvent()
    {
        // アイテム所持状態なら早期リターン
        if (m_cCore?.PlayerParameters.m_isFoodHold == true)
        {
            return;
        }

        if (CleaningFoamHandle.Result != null)
        {
            var CleaningFoamObj = Instantiate(CleaningFoamHandle.Result, transform);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            Destroy(CleaningFoamObj);
        }

        //ManagementGameDataManager.instance.AddSatisfactionValue(m_satisfactionValue);

        if (CleaningCompleteHandle.Result != null)
        {
            if (this == null) return;

            var CleaningCompleteObj = Instantiate(CleaningCompleteHandle.Result, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();

        CleaningFoamHandle = Addressables.LoadAssetAsync<GameObject>("CleaningFoam");
        CleaningCompleteHandle = Addressables.LoadAssetAsync<GameObject>("CleaningComplete");
    }

    //private void OnDestroy()
    //{
    //}
}
