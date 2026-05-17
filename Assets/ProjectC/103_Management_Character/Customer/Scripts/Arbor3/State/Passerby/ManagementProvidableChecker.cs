using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementProvidableChecker : StateBehaviour
{

    [SerializeField]
    private StateLink FailLink;
    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        if (!ManagementGameDataManager.instance.IsProvideAll())
        {
            Transition(FailLink);
        }
    }
}
