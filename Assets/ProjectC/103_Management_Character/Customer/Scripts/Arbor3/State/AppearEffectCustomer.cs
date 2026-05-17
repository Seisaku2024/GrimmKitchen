/*!
 * @file AppearEffecctCustomer.cs
 * @brief 指定したVisualEffectを生成/再再生を行う
 * @author 田内
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.VFX;

[AddComponentMenu("")]
[AddBehaviourMenu("Customer/AppearEffect")]
public class AppearEffectCustomer : BaseCustomerStateBehaviour
{
    [Header("出現するエフェクト")]
    [SerializeField] private AssetReferenceGameObject m_appearEffectAssetRef = null;

    private GameObject m_appearEffect = null;

    private void OnLoadVisualEffect(AsyncOperationHandle<GameObject> _handle)
    {
        if (gameObject == null)
        {
            return;
        }

        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            var obj = GetCustomerGameObject();
            if (obj == null) return;


            //取得できたエフェクトの作成
            m_appearEffect = Instantiate(_handle.Result, obj.transform);

            //エフェクト監視コンポーネントををルートオブジェクトに追加
            var observ = obj.AddComponent<EffectObservation>();
            if (observ)
            {
                observ.SetObservationEffect(m_appearEffect);
            }

        }
        else
        {
            //Addressableでの取得に失敗
            Debug.LogError("エフェクトの取得失敗。");
        }
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        if (m_appearEffect)
        {
            var ef = m_appearEffect.GetComponent<VisualEffect>();
            if (ef)
            {
                ef.Stop();
                ef.Reinit();
                ef.Play();
            }
        }
        else
        {
            //シリアライズのアセットがセットされていないなら処理はいらないようにする
            if (m_appearEffectAssetRef == null)
            {
                return;
            }

            if (gameObject)
            {
                Addressables.LoadAssetAsync<GameObject>(m_appearEffectAssetRef)
                    .Completed += OnLoadVisualEffect;
            }

        }
    }
}
