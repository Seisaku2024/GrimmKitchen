using UniRx;
using UnityEngine;

/// <summary>
/// HPBarのPresenter(吉田)
/// 体力とHPBarを紐づける
/// </summary>
[DefaultExecutionOrder(100)]    // CharacterCoreのStartを待つために実行タイミングを遅くする
public class HPBarPresenter : MonoBehaviour
{
    [SerializeField] private CharacterCore characterCore;
    [SerializeField] private HPBarController barController;

    public void Start()
    {
        if (barController == null)
        {
            barController = GetComponent<HPBarController>();
        }

        // HPの初期設定
        BarSetting();

        // 初期HPを記憶(山本)
        barController.TempHP = characterCore.Status.m_hp.Value;

        if (characterCore.Status.MaxHP == null) { return; }

        // 変わったら実行する処理を登録
        characterCore.Status.m_hp.Subscribe(x => BarUpdate());
        characterCore.Status.MaxHP.Subscribe(x => BarSetting());
    }

    private void BarUpdate()
    {
        barController.SetHealthValue(characterCore.Status.m_hp.Value);

        // デバッグ用
        //Debug.Log("HealthUpdate");
    }

    [ContextMenu("BarSetting")]
    private void BarSetting()
    {
        if (characterCore.Status.MaxHP == null) { return; }
        // HPの初期値を設定
        barController.SetHealth(
            characterCore.Status.m_hp.Value,
            characterCore.Status.MaxHP.Value);
    }

}
