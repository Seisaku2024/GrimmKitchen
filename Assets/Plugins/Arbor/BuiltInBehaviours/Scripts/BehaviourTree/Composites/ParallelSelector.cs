using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// すべての子ノードを並列に実行し、いずれかの子ノードが成功した場合に親ノードに成功を返して実行を終了する。<br/>
	/// 子ノードがすべて失敗した場合は親ノードに失敗を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute all child nodes in parallel, and if any child node succeeds, return success to the parent node and finish execution.<br/>
	/// If all child nodes fail, return failure to the parent node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("ParallelSelector")]
	[BuiltInBehaviour]
	public sealed class ParallelSelector : ParallelBase
	{
		protected override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Success;
		}
	}
}