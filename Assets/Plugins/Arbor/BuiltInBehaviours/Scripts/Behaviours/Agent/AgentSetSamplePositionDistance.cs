using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
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
	public class AgentSetSamplePositionDistance : AgentBase
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

		public override void OnStateBegin()
		{
			AgentController agentControlelr = cachedAgentController;
			if (agentControlelr != null)
			{
				agentControlelr.samplePositionDistance = _Distance.value;
			}
		}
	}
}