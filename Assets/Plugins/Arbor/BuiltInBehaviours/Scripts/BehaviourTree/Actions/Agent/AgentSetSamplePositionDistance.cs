//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="AgentController.samplePositionDistance"/>を設定する
	/// </summary>
#else
	/// <summary>
	/// Set <see cref="AgentController.samplePositionDistance"/>
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentSetSamplePositionDistance")]
	[BuiltInBehaviour]
	public sealed class AgentSetSamplePositionDistance : AgentBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 距離
		/// </summary>
#else
		/// <summary>
		/// Distance
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat(0.1f);

		protected override void OnExecute()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				agentController.samplePositionDistance = _Distance.value;
			}
			FinishExecute(true);
		}
	}
}