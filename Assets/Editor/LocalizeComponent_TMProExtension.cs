using System;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

namespace xrdnk.Editor
{
    /// <summary>
    /// TextMeshProを自動的にローカライズ設定するエディタ拡張
    /// </summary>
    static class LocalizeComponent_TMProExtension
    {
        [MenuItem("CONTEXT/TextMeshProUGUI/Localize Extension")]
        static void LocalizeTMProTextWithFontAssets(MenuCommand command)
        {
            var target = command.context as TextMeshProUGUI;
            SetupForLocalizeString(target);
            SetupForLocalizeTmpFont(target);
        }

        /// <summary>
        /// LocalizeStringEvent コンポーネントをアタッチすると同時に自動的に UpdateAsset イベントに text プロパティを変更する処理を追加する
        /// </summary>
        /// <param name="target">TextMeshProUGUI</param>
        static void SetupForLocalizeString(TextMeshProUGUI target)
        {
            var comp = Undo.AddComponent(target.gameObject, typeof(LocalizeStringEvent)) as LocalizeStringEvent;
            var setStringMethod = target.GetType().GetProperty("text").GetSetMethod();
            var methodDelegate =
                Delegate.CreateDelegate(typeof(UnityAction<string>), target, setStringMethod) as UnityAction<string>;
            UnityEventTools.AddPersistentListener(comp.OnUpdateString, methodDelegate);
            comp.OnUpdateString.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
        }

        /// <summary>
        /// LocalizeTmpFontEvent コンポーネントをアタッチすると同時に自動的に UpdateAsset イベントに font プロパティを変更する処理を追加する
        /// </summary>
        /// <param name="target">TextMeshProUGUI</param>
        static void SetupForLocalizeTmpFont(TextMeshProUGUI target)
        {
            var comp = Undo.AddComponent(target.gameObject, typeof(LocalizedTmpFontEvent)) as LocalizedTmpFontEvent;
            var setStringMethod = target.GetType().GetProperty("font").GetSetMethod();
            var methodDelegate =
                Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setStringMethod) as
                    UnityAction<TMP_FontAsset>;

            UnityEventTools.AddPersistentListener(comp.OnUpdateAsset, methodDelegate);
            comp.OnUpdateAsset.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
        }
    }
}