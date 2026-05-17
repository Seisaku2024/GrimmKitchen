using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;

[EditorToolbarElement(id, typeof(SceneView))]
class ScreenshotButton : EditorToolbarButton
{
    public const string id = "ProjectC/TakeScreenshot";

    public ScreenshotButton()
    {
        text = "Take Screenshot";

        clicked += () =>
        {
            TakeScreenshot();
        };
    }

    private void TakeScreenshot()
    {
        // スクリーンショットを保存するパスを選択
        string path = EditorUtility.SaveFilePanel("Save Screenshot", "", "screenshot.png", "png");
        if (!string.IsNullOrEmpty(path))
        {
            // カメラを使ってスクリーンショットを撮影
            Camera sceneCamera = SceneView.lastActiveSceneView.camera;
            if (sceneCamera != null)
            {
                // フルHD解像度
                int width = 1920;
                int height = 1080;

                RenderTexture tempRT = new RenderTexture(width, height, 24);
                sceneCamera.targetTexture = tempRT;
                sceneCamera.Render();

                RenderTexture.active = tempRT;
                Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                screenshot.Apply();

                byte[] bytes = screenshot.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
                Debug.Log($"Screenshot saved to {path}");

                RenderTexture.active = null;
                sceneCamera.targetTexture = null;
                Object.DestroyImmediate(tempRT);
                Object.DestroyImmediate(screenshot);
            }
        }
    }
}

[Overlay(typeof(SceneView), "ProjectC")]
public class EditorToolbarExample : ToolbarOverlay
{
    EditorToolbarExample() : base(
        ScreenshotButton.id  // Screenshotボタンをオーバーレイに追加
    )
    { }
}
