using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleController : MonoBehaviour
{

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    //=====================================
    //           実行処理
    //=====================================

    public async UniTask OnUpdate()
    {


        await UniTask.CompletedTask;
    }


}
