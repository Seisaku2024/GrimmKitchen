//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="AgentController.samplePositionEnabled"/>を設定する
	/// </summary>
#else
	/// <summary>
	/// Set <see cref="AgentController.samplePositionEnabled"/>
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentSetSamplePositionEnabled")]
	[BuiltInBehaviour]
	public sealed class AgentSetSamplePositionEnabled : AgentBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 有効フラグ
		/// </summary>
#else
		/// <summary>
		/// Enable flag
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _Enabled = new FlexibleBool(false);

		protected override void OnExecute()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				agentController.samplePositionEnabled = _Enabled.value;
			}
			FinishExecute(true);
		}
	}
}