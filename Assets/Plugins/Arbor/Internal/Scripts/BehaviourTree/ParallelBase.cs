using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コンポジットノードの子ノードをすべて並列に実行するCompositeBehaviourの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class for CompositeBehaviour that executes all child nodes of a composite node in parallel.
	/// </summary>
#endif
	public abstract class ParallelBase : CompositeBehaviour
	{
		private NodeStatus _ChildNodeStatus = NodeStatus.Running;

		List<BehaviourTreeExecutor> _Executors = new List<BehaviourTreeExecutor>();
		int _RunningCount;

		internal sealed override bool IsExecutor()
		{
			return true;
		}

		internal override int CalculateChildPropert(int order)
		{
			var childrenLink = compositeNode.GetChildrenLinkSlot();

			for (int slotIndex = 0; slotIndex < childrenLink.branchIDs.Count; slotIndex++)
			{
				int branchID = childrenLink.branchIDs[slotIndex];
				NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(branchID);
				if (branch != null)
				{
					TreeNodeBase childNode = behaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;
					if (childNode != null)
					{
						childNode.CalculatePriority(0);
					}
				}
			}

			return order;
		}

		void ReleaseExecutors()
		{
			for (int i = 0; i < _Executors.Count; i++)
			{
				var executor = _Executors[i];
				executor.Stop();

				if (executor.rootNode is IParentLinkSlotHolder parentLinkSlotHolder)
				{
					var branch = parentLinkSlotHolder.GetParentBranch();
					branch.isActive = false;
				}

				Pool.GenericPool<BehaviourTreeExecutor>.Release(executor);
			}

			_Executors.Clear();
			_RunningCount = 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブになったときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node becomes active.
		/// </summary>
#endif
		protected override void OnStart()
		{
			base.OnStart();

			ReleaseExecutors();

			var childrenLink = compositeNode.GetChildrenLinkSlot();

			for (int slotIndex = 0; slotIndex < childrenLink.branchIDs.Count; slotIndex++)
			{
				int branchID = childrenLink.branchIDs[slotIndex];
				NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(branchID);
				if (branch != null)
				{
					branch.isActive = true;

					TreeNodeBase childNode = behaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;

					BehaviourTreeExecutor executor = Pool.GenericPool<BehaviourTreeExecutor>.Get();
					executor.Play(behaviourTree, childNode, OnFinish);

					_Executors.Add(executor);
				}
			}

			_RunningCount = _Executors.Count;

			_ChildNodeStatus = NodeStatus.Running;
		}

		void OnFinish(BehaviourTreeExecutor executor, NodeStatus nodeStatus)
		{
			if (executor.rootNode is IParentLinkSlotHolder parentLinkSlotHolder)
			{
				var branch = parentLinkSlotHolder.GetParentBranch();
				branch.isActive = false;
			}

			executor.Stop();
			_RunningCount--;

			if (!CanExecute(nodeStatus) 
				|| _RunningCount == 0)
			{
				_ChildNodeStatus = nodeStatus;
			}
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

		internal override NodeStatus Execute()
		{
			for (int i = 0; i < _Executors.Count; i++)
			{
				var executor = _Executors[i];
				executor.Update();

				if (_ChildNodeStatus != NodeStatus.Running)
				{
					break;
				}
			}

			return _ChildNodeStatus;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードが終了したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node ends.
		/// </summary>
#endif
		protected override void OnEnd()
		{
			base.OnEnd();

			ReleaseExecutors();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブの間、FixedUpdateで呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called in FixedUpdate while the local node is active.
		/// </summary>
#endif
		protected override void OnFixedUpdate()
		{
			for (int i = 0; i < _Executors.Count; i++)
			{
				var executor = _Executors[i];
				executor.FixedUpdate();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブの間、LateUpdateで呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called in LateUpdate while the local node is active.
		/// </summary>
#endif
		protected override void OnLateUpdate()
		{
			for (int i = 0; i < _Executors.Count; i++)
			{
				var executor = _Executors[i];
				executor.LateUpdate();
			}
		}
	}
}