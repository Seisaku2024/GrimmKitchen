using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class HanselGretelAppearPlayableAsset : PlayableAsset
{
    
    [SerializeField] ExposedReference<CharacterCore> m_hanselCore;
    [SerializeField] ExposedReference<CharacterCore> m_gretelCore;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        HanselGretelAppearBehaviour hanselGretel = new HanselGretelAppearBehaviour();
        hanselGretel.m_hansel = m_hanselCore.Resolve(graph.GetResolver());
        hanselGretel.m_gretel = m_gretelCore.Resolve(graph.GetResolver());

        return ScriptPlayable<HanselGretelAppearBehaviour>.Create(graph, hanselGretel);

    }
}
