using ButtonInfo;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameterWindow : BaseWindow
{
    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スキル装備用ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_setStorySkillWindowController = null;

    public override async UniTask OnInitialize()
    {
        #region nullチェック

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            //await base.OnInitialize();
            //cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }



    public override async UniTask OnUpdate()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スキル変更画面表示
                await ChangeSkill();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                return;
                //// 閉じる
                //if (IsClose()) return;

                //await UniTask.DelayFrame(1);
                //cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    private async UniTask ChangeSkill()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_setStorySkillWindowController == null)
        {
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択したボタン
            var id = m_selectUIController.IsPressButton();

            switch (id)
            {
                // 経営開始ウィンドウを作成
                case ButtonID.ChangeStorySkill1:
                    {
                        var currentUI = m_selectUIController.CurrentSelectUI;
                        if (currentUI == null) return;

                        var controller = Instantiate(m_setStorySkillWindowController);
                        await controller.CreateWindow<ChangeStorySkillWindow>(false, async _ =>
                        {
                            _.SetStorySkillNo=StorySkillType.StorySkillNo.Skill1;
                            await UniTask.CompletedTask;
                        });
                        if (controller != null) Destroy(controller.gameObject);

                        break;
                    }
                case ButtonID.ChangeStorySkill2:
                    {
                        var currentUI = m_selectUIController.CurrentSelectUI;
                        if (currentUI == null) return;

                        var controller = Instantiate(m_setStorySkillWindowController);
                        await controller.CreateWindow<ChangeStorySkillWindow>(false, async _ =>
                        {
                            _.SetStorySkillNo = StorySkillType.StorySkillNo.Skill2;
                            await UniTask.CompletedTask;
                        });
                        if (controller != null) Destroy(controller.gameObject);

                        break;
                    }

                default:
                    break;

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}
