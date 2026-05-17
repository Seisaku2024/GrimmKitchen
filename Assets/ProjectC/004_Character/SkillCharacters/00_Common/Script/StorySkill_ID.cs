using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public enum StorySkill_ID
{
    None = 0,   //何もなし
    Akazukin = 1,   //赤ずきん
    Bremen = 2,   //ブレーメンの音楽隊
    HanselGretel = 3,//ヘンゼルとグレーテル
    LittleMermaid,

    StorySkillTypeNum,

}

[System.Serializable]
public class SkillIDReactiveProperty : ReactiveProperty<StorySkill_ID>
{
    public SkillIDReactiveProperty() { }
    public SkillIDReactiveProperty(StorySkill_ID initialValue) : base(initialValue) { }

}



