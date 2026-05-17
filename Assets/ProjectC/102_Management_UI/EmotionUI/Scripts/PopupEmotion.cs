using System.Collections;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UniRx.Triggers;

public class PopupEmotion : MonoBehaviour
{
    [SerializeField] float m_destroyTime = 3;

    public string EmotionKey;
    private GameObject popupBalloon; // 既存のPopupBalloonを保持

    public async void Popup(string objectKey)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        EmotionKey = objectKey;

        try
        {
            // すでに Balloon が存在する場合は再利用
            if (popupBalloon == null)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>("Emotion_Balloon");
                await handle;
                cancelToken.ThrowIfCancellationRequested();

                popupBalloon = Instantiate(handle.Result, this.gameObject.transform);
                popupBalloon.AddComponent<EmotionBase>().Emotion = objectKey;

                var emotionBase = popupBalloon.GetComponent<EmotionBase>();
                emotionBase.Parent = popupBalloon;
                Destroy(popupBalloon, m_destroyTime);
                await emotionBase.UpdateEmotion(objectKey);


                popupBalloon.OnDestroyAsObservable().Subscribe(_ =>
                {
                    Addressables.Release(handle);
                    popupBalloon = null;
                });

            }
            else
            {
                // 既存の Balloon の Emotion を更新
                var emotionBase = popupBalloon.GetComponent<EmotionBase>();
                if (emotionBase != null)
                {
                    await emotionBase.UpdateEmotion(objectKey);
                }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }
}
