using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class  PlayerSkillsParameters  : MonoBehaviour
{
    // ヘンゼルとグレーテルのスキル用パラメータ
    [Header("釜のオブジェクト")]
    [SerializeField]
    private GameObject m_cauldronObj = null;
    public GameObject CauldronObj => m_cauldronObj;


}
