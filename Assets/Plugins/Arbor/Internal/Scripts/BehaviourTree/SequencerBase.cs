using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コンポジットノードの子ノードを一つずつ順番に実行するCompositeBehaviourの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class for CompositeBehaviour that executes the child nodes of a composite node one by one.
	/// </summary>
#endif
	public abstract class SequencerBase : CompositeBehaviour
	{
		private int _CurrentIndex = 0;

		private NodeStatus _ChildNodeStatus = NodeStatus.Running;

		internal sealed override bool IsExecutor()
		{
			return false;
		}

		internal override sealed int CalculateChildPropert(int order)
		{
			var childrenLink = compositeNode.GetChildrenLinkSlot();
			int currentLink = (compositeNode.isActive && childrenLink.branchIDs.Count > 0) ? childrenLink.branchIDs[_CurrentIndex] : 0;

			behaviourTree.SortNodeLinkSlot(childrenLink);

			for (int slotIndex = 0; slotIndex < childrenLink.branchIDs.Count; slotIndex++)
			{
				int branchID = childrenLink.branchIDs[slotIndex];
				NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(branchID);
				if (branch != null)
				{
					TreeNodeBase childNode = behaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;
					if (childNode != null)
					{
						order = childNode.CalculatePriority(order);
					}
				}
			}

			if (currentLink != 0)
			{
				_CurrentIndex = childrenLink.branchIDs.IndexOf(currentLink);
			}

			return order;
		}

		internal override sealed NodeStatus Execute()
		{
			var childrenLink = compositeNode.GetChildrenLinkSlot();
			if (0 <= _CurrentIndex && _CurrentIndex < childrenLink.branchIDs.Count && CanExecute(_ChildNodeStatus))
			{
				int branchID = childrenLink.branchIDs[_CurrentIndex];
				compositeNode.executor.Push(branchID);
				return NodeStatus.Running;
			}

			return _ChildNodeStatus;
		}

		internal void InitializeChildStatus(bool interrupt, bool isRevaluator)
		{
			if (!interrupt || isRevaluator)
			{
				_CurrentIndex = GetBeginIndex();
			}

			_ChildNodeStatus = NodeStatus.Running;
		}

		internal int Interrupt(TreeNodeBase node)
		{
			_CurrentIndex = GetInterruptIndex(node);

			var childrenLink = compositeNode.GetChildrenLinkSlot();
			if (0 <= _CurrentIndex && _CurrentIndex < childrenLink.branchIDs.Count)
			{
				return childrenLink.branchIDs[_CurrentIndex];
			}
			else
			{
				return 0;
			}
		}

		internal void ChildExecuted(NodeStatus childStatus)
		{
			_CurrentIndex = GetNextIndex(_CurrentIndex);

			_ChildNodeStatus = childStatus;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時に実行する子ノードのインデックスを取得する。
		/// </summary>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the child node index to be executed at the start.
		/// </summary>
		/// <returns>Index of child node</returns>
#endif
		protected virtual int GetBeginIndex()
		{
			return 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 次に実行する子ノードのインデックスを取得する。
		/// </summary>
		/// <param name="currentIndex">現在のインデックス</param>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the child node index to be executed.
		/// </summary>
		/// <param name="currentIndex">Current index</param>
		/// <returns>Index of child node</returns>
#endif
		protected virtual int GetNextIndex(int currentIndex)
		{
			return currentIndex + 1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込んだノードのインデックスを取得する。
		/// </summary>
		/// <param name="node">割り込んだノード</param>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the index of the interrupted node.
		/// </summary>
		/// <param name="node">Interrupted node</param>
		/// <returns>Index of child node</returns>
#endif
		protected virtual int GetInterruptIndex(TreeNodeBase node)
		{
			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			int childCount = childrenLinkSlot.branchIDs.Count;
			var nodeBrachies = compositeNode.behaviourTree.nodeBranchies;
			for (int childIndex = 0; childIndex < childCount; ++childIndex)
			{
				NodeBranch branch = nodeBrachies.GetFromID(childrenLinkSlot.branchIDs[childIndex]);
				if (branch.childNodeID == node.nodeID)
				{
					return childIndex;
				}
			}

			return -1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行できるか判定する。
		/// </summary>
		/// <param name="childStatus">子ノードの状態</param>
		/// <returns>実行できる場合はtrueを返す。</returns>
#else
		/// <summary>
		/// It is judged whether it can be executed.
		/// </summary>
		/// <param name="childStatus">State of child node</param>
		/// <returns>Returns true if it can be executed.</returns>
#endif
		protected virtual bool CanExecute(NodeStatus childStatus)
		{
			return true;
		}
	}
}