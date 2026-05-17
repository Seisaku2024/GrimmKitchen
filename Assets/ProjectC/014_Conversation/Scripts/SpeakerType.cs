using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 会話用キャラクターのタイプ追加（山本）

namespace Speaker
{
    public enum SpeakerType
    {
        None,
        Hansel,
        Gretel,
        Akazukin,
        Hunter,
        OldWoman,
        Bremen,

        GretelStory = 20,
        LobbyStoryConversation = 30,
        AkazukinStory = 40,
        HunterStory,
        WolfStory,
        OldWomanStory,
        AkazukinStory02,

        Player = 100,
    }

    public enum TalkingFase
    {
        None,
        Fase1,
        Fase2,
        Fase3,
        Fase4,
        Fase5,
        Fase6,
        Fase7,
        Fase8,
        Fase9,
        Fase10
    }

    public enum StoryType
    {
        None,
        LobbyStory01,
        HanselAndGretel,
        Akazukin,
    }


}
