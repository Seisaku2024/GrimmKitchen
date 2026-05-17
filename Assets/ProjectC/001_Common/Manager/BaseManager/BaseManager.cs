using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    [Header("シーン変更後も削除しない")]
    [SerializeField]
    protected bool m_dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (m_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            // 一度のみ
            StartInstance();
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeleteInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = null;
        }
    }

    /// <summary>
    /// インスタンス作成時に一度だけ通す
    /// </summary>
    virtual protected void StartInstance()
    {
        Load();

        MessageBroker.Default.Receive<SaveManager.GlobalDeleteSaveEvent>().Subscribe(_ =>
        {
            Load();
        });
    }

    virtual protected void Load()
    {
    }
}
