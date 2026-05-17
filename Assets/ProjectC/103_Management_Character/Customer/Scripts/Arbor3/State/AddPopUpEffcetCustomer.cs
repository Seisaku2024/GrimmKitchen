using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AddPopUpEffcetCustomer : BaseCustomerStateBehaviour
{

    [SerializeField] private string m_objectKey = null;
    // Use this for enter state
    public override void OnStateBegin()
    {
        var obj = GetCustomerGameObject();
        if (obj == null) return;


        var popup = obj.AddComponent<PopupEmotion>();
        if (popup == null) return;

        if (m_objectKey != null)
            popup.Popup(m_objectKey);

    }

}
