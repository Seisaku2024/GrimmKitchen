using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
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
	public class AgentSetSamplePositionEnabled : AgentBase
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

		public override void OnStateBegin()
		{
			AgentController agentControlelr = cachedAgentController;
			if (agentControlelr != null)
			{
				agentControlelr.samplePositionEnabled = _Enabled.value;
			}
		}
	}
}