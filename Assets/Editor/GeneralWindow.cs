/*!
 * @file GeneralWindow.cs
 * @brief ProjectC用一般設定ウィンドウ
 * @author 上甲
 * @data v1
 *      音量設定を追加 実行中のみ
 */

using UnityEditor;

/// <summary>
/// @brief 一般設定ウィンドウ
/// </summary>
public class GeneralWindow : EditorWindow
{
    [MenuItem("PROJECT_C/General")]
    public static void ShowMyEditor()
    {
        EditorWindow.GetWindow(typeof(GeneralWindow));
    }
    //====================================================================
    // Sound
    private bool m_soundOpen = false;
    VolumeSaveLoader.VolumeSaveLoad m_volumeSaveLoad = new VolumeSaveLoader.VolumeSaveLoad();
    private void OnSoundEnable()
    {
        m_volumeSaveLoad.LoadVolume();
    }
    private void SoundOnGUI()
    {
        m_soundOpen = EditorGUILayout.BeginFoldoutHeaderGroup(m_soundOpen, "Sound");
        if (m_soundOpen)
        {
            // 音量取得
            m_volumeSaveLoad.LoadVolumeCriCategory();

            EditorGUI.BeginChangeCheck();

            m_volumeSaveLoad.MasterVolume = EditorGUILayout.Slider("Master", m_volumeSaveLoad.MasterVolume, 0, 1);

            m_volumeSaveLoad.MusicVolume = EditorGUILayout.Slider("Music", m_volumeSaveLoad.MusicVolume, 0, 1);

            m_volumeSaveLoad.SFXVolume = EditorGUILayout.Slider("SFX", m_volumeSaveLoad.SFXVolume, 0, 1);

            m_volumeSaveLoad.UIVolume = EditorGUILayout.Slider("UI", m_volumeSaveLoad.UIVolume, 0, 1);


            if (EditorGUI.EndChangeCheck())
            {
                m_volumeSaveLoad.SetCriCategoryVolume();
                m_volumeSaveLoad.SaveVolumeCriCategory();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

    }

    private void OnEnable()
    {
        OnSoundEnable();
    }
    public void OnGUI()
    {
        SoundOnGUI();
    }
}