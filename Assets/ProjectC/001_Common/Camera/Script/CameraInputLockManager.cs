using UnityEngine;

public class CameraInputLockManager : MonoBehaviour
{
    public static CameraInputLockManager Instance { get; private set; }

    [Header("Cinemachine Input Provider / Input Axis Controller を入れる")]
    [SerializeField]
    private Behaviour m_cameraInputComponent;

    private int m_lockCount;
    private bool m_prevEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Lock()
    {
        m_lockCount++;

        if (m_lockCount != 1)
        {
            return;
        }

        if (m_cameraInputComponent == null)
        {
            Debug.LogWarning("CameraInputLockManager: Camera Input Component が設定されていません。");
            return;
        }

        m_prevEnabled = m_cameraInputComponent.enabled;
        m_cameraInputComponent.enabled = false;
    }

    public void Unlock()
    {
        if (m_lockCount <= 0)
        {
            m_lockCount = 0;
            return;
        }

        m_lockCount--;

        if (m_lockCount != 0)
        {
            return;
        }

        if (m_cameraInputComponent == null)
        {
            return;
        }

        m_cameraInputComponent.enabled = m_prevEnabled;
    }
}
