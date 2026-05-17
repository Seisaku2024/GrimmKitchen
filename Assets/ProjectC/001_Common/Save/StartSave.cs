using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class StartSave : MonoBehaviour
{
    // 制作者 田内
    // Start時にセーブを行う

    //==============================
    //          実行処理
    //==============================

    private void Start()
    {
        SaveManager.instance.AllSave().Forget();
    }

}
