using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameGuid : System.IEquatable<GameGuid>, ISerializationCallbackReceiver
{
    // 制作者 田内
    // Guid

    //================
    // Guid
    private System.Guid m_guid;

    //===========================
    // 保存読み込み時のみ使用
    [HideInInspector]
    [SerializeField]
    private byte[] m_serializedGuid;


    public GameGuid(string _guid)
    {
        m_guid = new System.Guid(_guid);
        m_serializedGuid = null;
    }

    public GameGuid(byte[] _guid)
    {
        m_guid = new System.Guid(_guid);
        m_serializedGuid = null;
    }

    public GameGuid(System.Guid _guid)
    {
        m_guid = _guid;
        m_serializedGuid = null;
    }

    /// <summary>
    /// 空のGUID（読み取り専用）
    /// </summary>
    public static readonly GameGuid Empty;

    //========================================
    //              実行処理
    //========================================

    /// <summary>
    /// 新規Guidを作成し変換
    /// </summary>
    /// <returns></returns>
    public static GameGuid NewGuid()
    {
        return new GameGuid(System.Guid.NewGuid());
    }

    /// <summary>
    /// ToString()で一致するか
    /// </summary>
    public static bool IsMatchString(GameGuid _id1, GameGuid _id2)
    {
        if (_id1.ToString() == _id2.ToString()) return true;
        return false;
    }


    /// <summary>
    /// 新規Guidを作成し更新
    /// </summary>
    public void Generate()
    {
        m_guid = System.Guid.NewGuid();
    }

    /// <summary>
    /// Guidをクリア
    /// </summary>
    public void Clear() => m_guid = System.Guid.Empty;


    /// <summary>
    /// Guidが空か確認
    /// </summary>
    public bool IsEmpty => m_guid == System.Guid.Empty;

    /// <summary>
    /// 文字列に変換
    /// </summary>
    public override string ToString() => m_guid.ToString();


    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (m_serializedGuid != null && m_serializedGuid.Length == 16)
        {
            //正常なら
            //読み込まれたとき
            m_guid = new System.Guid(m_serializedGuid);
        }
        else
        {
            //不正の値なら
            m_guid = System.Guid.Empty;
        }

    }


    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        //シリアライズ前に保存
        m_serializedGuid = m_guid.ToByteArray();
    }


    public bool Equals(GameGuid guid) => m_guid.Equals(guid);

    public override bool Equals(object obj)
    {
        return m_guid.Equals(obj);
    }

    //public override bool Equals(Object obj)=> m_guid.Equals(obj);
    public override int GetHashCode() => m_guid.GetHashCode();

    //比較
    public static bool operator ==(GameGuid a, GameGuid b)
    {
        return a.m_guid == b.m_guid;
    }

    public static bool operator !=(GameGuid a, GameGuid b)
    {
        return a.m_guid != b.m_guid;
    }

}
