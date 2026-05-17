using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// すべての子ノードを並列に実行し、いずれかの子ノードが失敗した場合に親ノードに失敗を返して実行を終了する。<br/>
	/// 子ノードがすべて成功した場合は親ノードに成功を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute all child nodes in parallel, and if any child node fails, return failure to the parent node and terminate execution.<br/>
	/// Return success to the parent node if all child nodes are successful.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("ParallelSequencer")]
	[BuiltInBehaviour]
	public sealed class ParallelSequencer : ParallelBase
	{
		protected override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Failure;
		}
	}
}