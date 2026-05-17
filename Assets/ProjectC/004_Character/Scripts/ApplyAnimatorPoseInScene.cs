using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
struct PoseAnimatorSetting
{
    public string LayerName;
    public string StateName;
}

public class ApplyAnimatorPoseInScene : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    List<PoseAnimatorSetting> PoseAnimatorSettingList;

    [SerializeField]
    public float poseTimeInClip;

    private void OnValidate()
    {
        var animator = GetComponent<Animator>();
        foreach (var p in PoseAnimatorSettingList)
        {
            animator.PlayInFixedTime(p.StateName, animator.GetLayerIndex(p.LayerName), poseTimeInClip);
        }
        animator.Update(poseTimeInClip);
    }

#endif
}