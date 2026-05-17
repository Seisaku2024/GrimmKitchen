using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UnityEngine.InputSystem;


[RequireComponent(typeof(CanvasGroup))]
public class BaseWindowUI : MonoBehaviour
{
    // 制作者 田内
    // 同ウィンドウで操作を分けたいUIの基底クラス

    [Header("非選択中エフェクト")]
    [SerializeField]
    private List<BaseWindowUIEffect> m_windowUIEffectList = new();

    //====================================================================
    // 選択されていなくても実行される処理
    //====================================================================

    /// <summary>
    /// 最初に一度初期化
    /// </summary>
    virtual public async UniTask OnInitialize()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 常に動作
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 常に動作、OnUpdateの後処理
    /// </summary>
    virtual public async UniTask OnLateUpdate()
    {
        await UniTask.CompletedTask;
    }


    //====================================================================
    // 選択されている場合ににのみ実行される処理
    //====================================================================


    /// <summary>
    /// 選択されたときに一度初期化
    /// </summary>
    virtual public async UniTask OnSelectInitialize()
    {
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 選択中の場合に動作(OnUpdateより先に行われる)
    /// </summary>
    virtual public async UniTask OnSelectUpdate()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 選択終了時に終了処理
    /// </summary>
    virtual public async UniTask OnSelectExit()
    {
        await UniTask.CompletedTask;
    }


    //=================================================
    // エフェクト再生処理
    //=================================================


    /// <summary>
    /// エフェクト再生
    /// true - 再生 ; false - 終了
    /// </summary>
    virtual public async UniTask PlayEffect(bool _flg)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            foreach (var effect in m_windowUIEffectList)
            {
                if (effect == null) continue;

                if (_flg)
                {
                    await effect.PlayEffect();
                    cancelToken.ThrowIfCancellationRequested();
                }
                else
                {
                    await effect.UnPlayEffect();
                    cancelToken.ThrowIfCancellationRequested();
                }

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



}
