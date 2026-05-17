using Arbor;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PassebryColliderAttach : MonoBehaviour
{

    [Tag][SerializeField] string tag = "Customer";

    [Header("来店確率(100で確定)")]
    [Range(0, 100)]
    [SerializeField] float m_probability = 30;

    [Header("接触時に判定を実行するか")]
    [SerializeField] private bool m_isTriggerThink = true;

    public float Probability
    {
        get { return m_probability; }
        set { m_probability = value; }
    }

    CustomerData customerData = null;

    private Collider m_collider = null;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
        if (m_collider == null)
        {
            Debug.LogError("Colliderがアタッチされていません");
            return;
        }

        this.OnTriggerEnterAsObservable()
    .Where(_ => enabled) // ゲームオブジェクトが有効な場合のみ
    .Where(_ => m_isTriggerThink) // 条件に合致した場合に処理を実行
    .Where(collider => collider.CompareTag(tag)) // コライダーが指定のタグを持つ場合のみ
    .Subscribe(collider =>
    {
        ThinkEntering(collider); // 条件に合致した場合に処理を実行
    })
    .AddTo(this);
    }

    private bool ProbabilityCheck(float probabirity = -1)
    {
        if (probabirity < 0)
        {
            probabirity = m_probability;
        }
        if (probabirity < 100)
        {
            int rand = Random.Range(0, 100);
            if (probabirity < rand) return false;
        }
        return true;
    }

    private void ThinkEntering(Collider collider, float probabirity = -1)
    {
        if (ManagementGameDataManager.instance.IsTimeOut())
        {
            return;
        }

        if (!ProbabilityCheck(probabirity))
        {
            return;
        }
        var obj = collider.transform.root.gameObject;

        customerData = obj.GetComponent<CustomerData>();

        if (customerData && customerData.CurrentCustomerState == CustomerStateInfo.CustomerState.Passerby)
        {
            customerData.CurrentCustomerState = CustomerStateInfo.CustomerState.Normal;

            var popup = obj.GetOrAddComponent<PopupEmotion>();
            if (popup == null) return;
            popup.Popup("Emotion_Happy");
        }

    }


    /// <summary>
    /// @brief コライダー内に存在するオブジェクトを取得し、抽選を行う
    /// </summary>
    public void InsideThinkEntering(float probabirity = -1, bool isFlyers = false)
    {
        if (m_collider == null)
        {
            Debug.LogError("Colliderがアタッチされていません");
            return;
        }

        // コライダー内に存在するオブジェクトを取得
        Collider[] colliders = Physics.OverlapSphere(m_collider.bounds.center, m_collider.bounds.extents.magnitude);
        foreach (var collider in colliders)
        {
            if(!collider.CompareTag(tag))
            {
                continue;
            }
            if (isFlyers)
            {
                var obj = collider.transform.root.gameObject;

                if (obj.TryGetComponent(out CustomerData customerData))
                {
                    if (IsThinkableState(customerData.CurrentCustomerState))
                    {
                        obj.GetOrAddComponent<PopupEmotion>().Popup("Emotion_Question");
                    }
                }
            }
            ThinkEntering(collider, probabirity);
        }
    }
    // 通行状態のみ
    bool IsThinkableState(CustomerStateInfo.CustomerState state)
    {
        switch (state)
        {
            case CustomerStateInfo.CustomerState.Passerby:
                return true;

            default:
                return false;
        }
    }

    bool IsPopUpAbleState(CustomerStateInfo.CustomerState state)
    {
        switch (state)
        {
            case CustomerStateInfo.CustomerState.Passerby:
            case CustomerStateInfo.CustomerState.Normal:
                return true;
            default:
                return false;
        }
    }

}

public static class ComponentExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
}