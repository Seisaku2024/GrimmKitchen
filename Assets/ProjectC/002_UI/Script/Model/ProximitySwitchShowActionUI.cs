using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;


[RequireComponent(typeof(BoxCollider))]
public class ProximitySwitchShowActionUI : MonoBehaviour
{
    // 元：ProximityCreateWindow.cs
    // 近くにタグがある場合ActionUIを表示
    // BoxColliderが必要です
    // 制作者　吉田


    [Header("どこの表示非表示を切り替えるか")]
    [SerializeField]
    private ActionUIController.ActionUIState m_showActionUI = ActionUIController.ActionUIState.AbleToCooking;

    [Header("タグ")]
    [SerializeField]
    private string m_tag = "Player";

    private CharacterCore m_characterCore = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            // キャラクターコアからActionUIを表示する
            if (m_characterCore == null)
            {
                if (other.TryGetComponent<CharacterCore>(out CharacterCore characterCore))
                {
                    m_characterCore = characterCore;
                }
                else
                {
                    Debug.LogError("CharacterCoreが見つかりませんでした");
                    return;
                }
            }

            Add(other.transform.position);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            // キャラクターコアからActionUIを表示する
            if (m_characterCore == null)
            {
                if (other.TryGetComponent<CharacterCore>(out CharacterCore characterCore))
                {
                    m_characterCore = characterCore;
                }
                else
                {
                    Debug.LogError("CharacterCoreが見つかりませんでした");
                    return;
                }
            }

            Remove();
        }
    }

    private void OnDisable()
    {
        Remove();
    }


    private void Add(Vector3 _otherposition)
    {
        if (m_characterCore == null) return;
        if (m_characterCore.PlayerParameters == null) return;

        float distance = Vector3.Distance(_otherposition, transform.position);
        m_characterCore.PlayerParameters.AddAnyActionUIState(m_showActionUI, distance);
        m_characterCore.PlayerParameters.AddActionUIState(m_showActionUI);
    }

    private void Remove()
    {
        if (m_characterCore == null) return;
        if (m_characterCore.PlayerParameters == null) return;

        m_characterCore.PlayerParameters.RemoveAnyActionUIState(m_showActionUI);
        m_characterCore.PlayerParameters.RemoveActionUIState(m_showActionUI);
    }

}
