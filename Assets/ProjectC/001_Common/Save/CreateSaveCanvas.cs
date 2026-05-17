using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CreateSaveCanvas : MonoBehaviour
{

    [Header("セーブ表示")]
    [SerializeField]
    private Canvas m_saveCanvas = null;


    //=============================================
    //              実行処理
    //=============================================

    private void Awake()
    {
        MessageBroker.Default.Receive<SaveManager.GlobalSaveEvent>().Subscribe(_ =>
        {
            if (_.SaveType == SaveManager.GlobalSaveEvent.Type.Start)
            {
                Create();
            }
        }).AddTo(this);
    }

    private void Create()
    {
        Instantiate(m_saveCanvas);
    }

}
