using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
[VolumeComponentMenu("Post-processing/Custom/Diffusion")]
public class DiffusionPostProcessVolume : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter ScreenBlend =
        new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

    public ClampedFloatParameter GaussDispersion =
        new ClampedFloatParameter(2.0f, 0.0f, 10.0f);

    public ClampedIntParameter GaussSmaplingTexelAmount =
        new ClampedIntParameter(8, 1, 32);

    public bool IsActive()
    {
        return active && ScreenBlend.value > 0.0f;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;

//[System.Serializable]
//[VolumeComponentMenu("Diffusion")]
//public class DiffusionPostProcessVolume : VolumeComponent
//{



//    ////Defalt値はその処理が働かない値にした方がよい（チェック外れたときにDefalt値を返すから）
//    //public ClampedFloatParameter GaussDispersion = new ClampedFloatParameter(3, 0, 10);
//    //public ClampedIntParameter GaussSmaplingTexelAmount = new ClampedIntParameter(9, 0, 32);
//    //public ClampedFloatParameter ScreenBlend = new ClampedFloatParameter(1, 0, 3);


//    //public bool IsActive()
//    //{
//    //    return active && ScreenBlend.value > 0.0f;
//    //}

//    //public bool IsTileCompatible()
//    //{
//    //    return false;
//    //}


//}
