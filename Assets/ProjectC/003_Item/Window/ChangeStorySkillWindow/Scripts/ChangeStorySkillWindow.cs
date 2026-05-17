using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StorySkillType;
using UniRx;

public class ChangeStorySkillWindow : BaseWindow
{
    // 童話スキルの交換を行うウィンドウ（山本）
    [Header("スロット作成")]
    [SerializeField]
    private CreateStorySkillSlotList m_createStorySkillSlotList = null;

    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("説明文")]
    [SerializeField]
    private StorySkillDescription m_storySkillDescription = null;

    [Header("どの装備した童話スキルを変更するか")]
    [SerializeField]
    private StorySkillNo m_storySkillNo = StorySkillNo.Skill1;

    public StorySkillNo SetStorySkillNo { get { return m_storySkillNo; } set { m_storySkillNo = value; } }

    private CharacterCore m_character;

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createStorySkillSlotList == null)
        {
            Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
            return;
        }

        if(m_storySkillDescription==null)
        {
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スロット作成
            await m_createStorySkillSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            if(StorySkillDataBaseManager.instance.GetStorySkillData(StorySkill_ID.Akazukin).MasterFlg)
           await m_storySkillDescription.UpdateStorySkillDescription(StorySkillDataBaseManager.instance.GetStorySkillData(StorySkill_ID.Akazukin));

            await UniTask.DelayFrame(1);
            cancelToken.ThrowIfCancellationRequested();

            // プレイヤーのコアセット
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.GroupNo == CharacterGroupNumber.player)
                {
                    m_character = chara;
                    break;
                }
            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        // Nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.Log("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }
        #endregion

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

                // スクロールの更新
                m_changeScrollViewPosition.OnUpdate();

                // 童話スキル変更
                await ChangeStorySkill();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    private async UniTask ChangeStorySkill()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if(m_selectUIController==null)
            {
                return;
            }

            // 選択しているUIがスロットであれば
            var slotData = m_selectUIController?.CurrentSelectUI?.GetComponent<StorySkillSlotData>();
            if (slotData == null) return;

            // イベント発行
            UpdateStorySkillDescription.PublishStorySkillEvent(slotData.StorySkillData);

            // 選択したか
            if (m_selectUIController.IsPress == false) return;

            // 選択しているUIがなければ
            if (m_selectUIController.CurrentSelectUI == null) return;

            if (m_character == null) return;

            if (m_storySkillNo == StorySkillNo.Skill1)
            {
                m_character.PlayerParameters.StorySkill1_ID.Value = slotData.StorySkillData.StorySkill_ID;
            }
            else
            {
                m_character.PlayerParameters.StorySkill2_ID.Value = slotData.StorySkillData.StorySkill_ID;
            }

            //　リストの中身を全てfalseにする
            foreach(var ui in m_selectUIController.UIList)
            {
               foreach( var obj in ui.List)
                {
                   var slotSkillData = obj.UI.GetComponent<StorySkillSlotData>();
                    if (slotSkillData == null) break;

                    slotSkillData.ChangeSetStorySkillImage(false);

                }
            }

            // 装備中アイコンをTrueに切替
            slotData.ChangeSetStorySkillImage(true);

            // 変更をセーブする（山本）
            m_character.PlayerParameters.SaveStorySkill();

        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }

        await UniTask.CompletedTask;
    }

}


public class UpdateStorySkillDescription
{
    public StorySkillData m_storySkillData = null;

    public static void PublishStorySkillEvent(StorySkillData _data)
    {
        // イベント送信
        UpdateStorySkillDescription eve = new();
        eve.m_storySkillData = _data;
        MessageBroker.Default.Publish<UpdateStorySkillDescription>(eve);
    }

}
