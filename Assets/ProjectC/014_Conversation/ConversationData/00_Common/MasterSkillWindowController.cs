using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillWindowController : WindowController
{
    public async UniTask<WindowType> CreateMasterSkillWindow<WindowType>(StorySkill_ID _skill_ID = StorySkill_ID.None, Transform _lookAtTransform = null, bool _bSelef = false, System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : BaseWindow
    {
        if (m_createWindowObject != null)
        {
            // 作成済みであればここ
            return null;
        }

        if (m_window == null)
        {
            Debug.LogError("作成したいウィンドウが登録されていません");
            return null;
        }

        try
        {
            // ウィンドウを作成
            m_createWindowObject = Instantiate(m_window, transform);

            // 子オブジェクトからコンポーネントを取得
            var window = m_createWindowObject.GetComponentInChildren<MasterStorySkillWindow>();
            if (window == null)
            {
                Debug.LogError("MasterStorySkillWindowコンポーネントがアタッチされていません");
                DestroyWindow();
                return null;
            }

            // 童話スキルのIDを渡す
            window.StorySkill_ID = _skill_ID;   
            

            var cancelToken = window.GetCancellationTokenOnDestroy();

            // 非表示
            m_createWindowObject.SetActive(false);

            // 外部処理の実行
            if (onBeforeInitialize != null)
            {
                if (window is WindowType == false)
                {
                    Debug.LogError("キャストが行えません");
                }
                else
                {
                    await onBeforeInitialize(window as WindowType);
                    cancelToken.ThrowIfCancellationRequested();
                }
            }

            // 初期化処理
            await window.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();


            // 表示
            m_createWindowObject.SetActive(true);

         
            // 表示処理
            await window.OnShow();
            cancelToken.ThrowIfCancellationRequested();


            // 自分で操作する場合は通さない
            if (_bSelef == false)
            {
                // 本処理 
                await window.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 非表示処理
                await window.OnClose();
                cancelToken.ThrowIfCancellationRequested();


                // 終了処理
                await window.OnDestroy();
                cancelToken.ThrowIfCancellationRequested();

            }

            // ウィンドウを返す
            return window as WindowType;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);

            // エラーが出た場合ウィンドウを削除する
            DestroyWindow();
        }

        return null;
    }
}

