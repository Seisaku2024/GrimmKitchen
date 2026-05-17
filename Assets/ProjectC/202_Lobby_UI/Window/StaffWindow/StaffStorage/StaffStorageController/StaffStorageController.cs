using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class StaffStorageController : MonoBehaviour
{
    // 制作者 田内
    // スタッフストレージでスタッフを操作するコントローラー

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    //=======================================
    //              実行処理
    //=======================================


    public async UniTask OnUpdate()
    {

    }





    private void Dismissal()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }

    }


}
