using ConditionInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterCore : MonoBehaviour, IDamageable
{



    [System.Serializable]
    [AddTypeMenu("Player/LittleMarmaid/Appear")]
    public class ActionState_SkillMarmaid_Appear : ActionState_Base
    {
        private GameObject m_effct = null;
        private CharacterCore m_playerCore = null;

        public override void OnEnter()
        {
            base.OnEnter();

            if (Core.PlayerSkillsParameters && Core.PlayerSkillsParameters.WaterSurfaceEffect)
                m_effct = Instantiate(Core.PlayerSkillsParameters.WaterSurfaceEffect, Core.transform.position, Quaternion.identity);


            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_playerCore = core;
                }
            }

            if (m_playerCore == null) return;

            var dir = Vector3.forward;
            var v = Vector3.RotateTowards(Core.transform.forward, m_playerCore.transform.forward, 10000.0f, 0);
            Core.SetRotateToTarget(v, false);

        }


        public override void OnExit()
        {
            base.OnExit();

            if (m_effct)
            {
                Destroy(m_effct);
            }


        }

    }

    [System.Serializable]
    [AddTypeMenu("Player/LittleMarmaid/Regene")]
    public class ActionState_SkillMarmaid_Regene : ActionState_Base
    {
        private CharacterCore m_playerCore = null;

        public override void OnEnter()
        {
            base.OnEnter();

            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_playerCore = core;
                }
            }

            if (Core.m_animator)
            {
                Core.m_animator.ResetTrigger("IsCast");
            }

            if (m_playerCore == null) return;

            var dir = m_playerCore.transform.position - Core.transform.position;
            dir.Normalize();
            Core.SetRotateToTarget(dir, false);

        }

        public override void OnExit()
        {
            base.OnExit();

            if (m_playerCore == null) return;

            var conditionData = ConditionDataBaseManager.instance.GetConditionData(ConditionID.Regenerarte);
            if (conditionData == null || conditionData.ConditionPrefab == null) return;

            ConditionManager conditionmanager = m_playerCore.GetComponentInChildren<ConditionManager>();
            if (conditionmanager == null) return;

            conditionmanager.AddCondition(conditionData.ConditionPrefab, false);

        }

    }


    [System.Serializable]
    [AddTypeMenu("Player/LittleMarmaid/Disappear")]
    public class ActionState_SkillMarmaid_Disappear : ActionState_Base
    {
        private GameObject m_effct = null;

        public override void OnEnter()
        {
            base.OnEnter();

            if (Core.PlayerSkillsParameters && Core.PlayerSkillsParameters.WaterSurfaceEffect)
            {
                Vector3 effectPosition = Core.transform.position + Core.transform.forward * 1.0f;

                m_effct = Instantiate(Core.PlayerSkillsParameters.WaterSurfaceEffect,
                    effectPosition, Quaternion.identity);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (m_effct)
            {
                Destroy(m_effct);
            }

        }

    }


    [System.Serializable]
    [AddTypeMenu("Player/LittleMarmaid/Vanish")]
    public class ActionState_SkillMarmaid_Vanish : ActionState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Destroy(Core.transform.gameObject);


        }

    }
}

