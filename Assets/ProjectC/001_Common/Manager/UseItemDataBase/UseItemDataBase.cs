using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UseItemDataBase",menuName = "ScriptableObjects/UseItem/作成 UseItemDataBase")]
public class UseItemDataBase : ScriptableObject
{
    // 制作者 田内
    // アイテム使用用途データベース


    [Header("データリスト")]
    [SerializeField]
    private List<UseItemData> m_useItemDataList = new();


    public List<UseItemData> UseItemDataList { get { return m_useItemDataList; } }
}
