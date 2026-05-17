using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddStatusTypeDataBase",
    menuName = "ScriptableObjects/AddStatusType/作成 FoodAddStatusTypeDataBase")]
public class AddStatusTypeDataBase : ScriptableObject
{
    // 制作者 吉田
    // 追加ステータス種類データベース

    [Header("データリスト")]
    [SerializeField]
    private List<AddStatusTypeData> m_addStatusTypeDataList = new();

    public List<AddStatusTypeData> AddStatusTypeDataList
    {
        get { return m_addStatusTypeDataList; }
    }

}
