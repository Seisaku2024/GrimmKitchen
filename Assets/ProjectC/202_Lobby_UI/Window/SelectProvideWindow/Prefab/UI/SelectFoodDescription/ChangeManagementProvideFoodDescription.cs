using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeManagementProvideFoodDescription : ChangeItemDescription
{ 
    protected override void Start()
    {
        m_pocketType = ProvideFoodManager.instance.PocketType;
        base.Start();
    }
}
