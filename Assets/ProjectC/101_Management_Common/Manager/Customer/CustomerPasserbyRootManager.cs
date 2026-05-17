/*
 * @file CustomerPasserbyRootManager.cs
 * @brief 客の出現/帰宅位置を管理するクラス
 * 現在の仕様では数ある中からランダムに選び出す
 * @author 上甲
 */


using ExternalPropertyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// @brief 客の出現/帰宅位置を管理するクラス
/// 現在の仕様では数ある中からランダムに選び出す
/// </summary>
public class CustomerPasserbyRootManager : BaseManager<CustomerPasserbyRootManager>
{

    [Header("==========出現位置==========")]
    [SerializeField] public List<Transform> m_rightSidePos = new();

    [SerializeField] public List<Transform> m_leftSidePos = new();


    [Header("==========帰宅位置==========")]
    [SerializeField] public List<Transform> m_endPos = new();


    public Vector3 RightSideAppearPos
    {
        get
        {
            if (m_rightSidePos.Count == 0)
            {
                return Vector3.zero;
            }

            return m_rightSidePos[Random.Range(0, m_rightSidePos.Count)].position;
        }
    }
    public Vector3 LeftSideAppearPos
    {
        get
        {
            if (m_leftSidePos.Count == 0)
            {
                return Vector3.zero;
            }
            return m_leftSidePos[Random.Range(0, m_leftSidePos.Count)].position;
        }
    }

    public Vector3 RandomAppearPos
    {
        get
        {
            if (Random.Range(0, 2) == 0)
            {
                return RightSideAppearPos;
            }
            else
            {
                return LeftSideAppearPos;
            }
        }
    }



    public Vector3 RandomEndPos
    {
        get 
        {
            return m_endPos[Random.Range(0, m_endPos.Count)].position;
        }
    }


    public Vector3 RightSideSelectPos(int i)
    {
        if (m_rightSidePos.Count == 0 && m_rightSidePos.Count > i - 1)
        {
            return Vector3.zero;
        }
        return m_rightSidePos[i].position;
    }
    public Vector3 LeftSideSelectPos(int i)
    {
        if (m_leftSidePos.Count == 0 && m_leftSidePos.Count > i - 1)
        {
            return Vector3.zero;
        }
        return m_leftSidePos[i].position;
    }
}