using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHanselGretelSkill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.enemy)
            {
                var vec = chara.transform.position - transform.position;
                var dist = vec.magnitude;

                if(dist<=10.0f)
                {
                    chara.m_animator.SetTrigger("Absorb");

                }

            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
