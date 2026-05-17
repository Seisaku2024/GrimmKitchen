using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMoveSpeedCore : UseCharacterCoreBase
{
    private enum CalcType
    { Add, Multiply, Set }

    [SerializeField] CalcType calcType = CalcType.Set;

    [SerializeField]FlexibleFloat flexibleSpeed = new FlexibleFloat(2f);



    private void SetSpeed()
    {
        switch(calcType)
        {
            case CalcType.Add:
                base.CharacterCore.Status.WalkSpeed += flexibleSpeed.value;
                base.CharacterCore.Status.DushSpeed += flexibleSpeed.value;
                break;
            case CalcType.Multiply:
                base.CharacterCore.Status.WalkSpeed *= flexibleSpeed.value;
                base.CharacterCore.Status.DushSpeed *= flexibleSpeed.value;
                break;
            case CalcType.Set:
                base.CharacterCore.Status.WalkSpeed = flexibleSpeed.value;
                base.CharacterCore.Status.DushSpeed = flexibleSpeed.value;
                break;
        }
    }

    public override void OnStateBegin()
    {
        SetSpeed();
    }

}
