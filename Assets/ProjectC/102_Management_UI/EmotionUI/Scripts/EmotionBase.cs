using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UniRx.Triggers;

public class EmotionBase : MonoBehaviour
{
    public string Emotion { get; set; }
    public GameObject Parent { get; set; }
    private GameObject emotionObject; // 現在のEmotionオブジェクトを保持

    public async UniTask UpdateEmotion(string newEmotion)
    {
        if (!Parent) return;

        // 既存のエモーションオブジェクトを削除
        if (emotionObject != null)
        {
            Destroy(emotionObject);
        }

        // 新しいエモーションを読み込み
        var handle = Addressables.LoadAssetAsync<GameObject>(newEmotion);
        await handle;

        if (!Parent) return;

        // 新しいエモーションをセット
        emotionObject = Instantiate(handle.Result, Parent.transform);
        Emotion = newEmotion;

        emotionObject.OnDestroyAsObservable().Subscribe(_ =>
        {
            Addressables.Release(handle);
        });
    }
}
