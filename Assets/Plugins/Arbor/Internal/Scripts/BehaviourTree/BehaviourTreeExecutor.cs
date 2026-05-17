using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ビヘイビアツリーを実行するクラス。
	/// </summary>
#else
	/// <summary>
	/// A class that executes a behaviour tree.
	/// </summary>
#endif
	public sealed class BehaviourTreeExecutor
	{
		private List<TreeNodeBase> _ActiveNodes = new List<TreeNodeBase>();
		private TreeNodeBase _CurrentNode;

		private List<TreeNodeBase> _Revaluators = new List<TreeNodeBase>();

		private int _InterruptCount = 0;
		private bool _IsBreakPoint;

		private BehaviourTreeInternal _BehaviourTree;
		private System.Action<BehaviourTreeExecutor, NodeStatus> _OnFinish;

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートのノード
		/// </summary>
#else
		/// <summary>
		/// root node
		/// </summary>
#endif
		public TreeNodeBase rootNode
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生状態
		/// </summary>
#else
		/// <summary>
		/// Play state
		/// </summary>
#endif
		public PlayState playState
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在のアクティブノード
		/// </summary>
#else
		/// <summary>
		/// Current active node
		/// </summary>
#endif
		public TreeNodeBase currentNode
		{
			get
			{
				return _CurrentNode;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生開始。
		/// </summary>
		/// <param name="behaviourTree">再生するビヘイビアツリー</param>
		/// <param name="node">最初に実行するルートとなるノード</param>
		/// <param name="onFinish">ルートノードの実行が終了した際のコールバック</param>
#else
		/// <summary>
		/// Start playing.
		/// </summary>
		/// <param name="behaviourTree">Behaviour tree to play</param>
		/// <param name="node">First root node to execute</param>
		/// <param name="onFinish">Callback when root node execution finishes</param>
#endif
		public void Play(BehaviourTreeInternal behaviourTree, TreeNodeBase node, System.Action<BehaviourTreeExecutor, NodeStatus> onFinish)
		{
			if (playState != PlayState.Stopping)
			{
				return;
			}

			_BehaviourTree = behaviourTree;
			_OnFinish = onFinish;
			rootNode = node;

			playState = PlayState.Playing;
			Push(node);
		}

		internal TreeNodeBase Push(int branchID)
		{
			NodeBranch branch = _BehaviourTree.nodeBranchies.GetFromID(branchID);
			if (branch == null)
			{
				return null;
			}

			TreeNodeBase node = _BehaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;
			if (node == null)
			{
				return null;
			}

			branch.isActive = true;

			Push(node);
			return node;
		}

		void Push(TreeNodeBase node)
		{
			if (_CurrentNode == node)
			{
				return;
			}

			_CurrentNode = node;
			if (_CurrentNode != null)
			{
				_CurrentNode.executor = this;
			}

			_BehaviourTree?.StateChanged();

			if (_CurrentNode == null)
			{
				return;
			}

			_ActiveNodes.Add(_CurrentNode);
		}

		void Pop()
		{
			if (_CurrentNode == null)
			{
				return;
			}

			_ActiveNodes.Remove(_CurrentNode);
			_CurrentNode.Activate(false, false, false);

			if (_CurrentNode == rootNode)
			{
				_CurrentNode = null;
			}
			else
			{
				var parentLinkHolder = _CurrentNode as IParentLinkSlotHolder;
				if (parentLinkHolder != null)
				{
					TreeNodeBase parentNode = null;

					NodeBranch branch = parentLinkHolder.GetParentBranch();
					if (branch != null)
					{
						branch.isActive = false;
						parentNode = _BehaviourTree.GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
					}
					_CurrentNode = parentNode;
				}
				else
				{
					_CurrentNode = null;
				}
			}

			if (_CurrentNode != null)
			{
				_CurrentNode.executor = this;
			}

			_BehaviourTree.StateChanged();
		}

		void Pop(NodeStatus childStatus)
		{
			Pop();

			if (_CurrentNode != null)
			{
				_CurrentNode.OnChildExecuted(childStatus);
			}
		}

		internal void Restart()
		{
			foreach (var revaluator in _Revaluators)
			{
				TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.RevaluationExit();
				}
			}
			_Revaluators.Clear();
			Push(rootNode);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新
		/// </summary>
#else
		/// <summary>
		/// Update
		/// </summary>
#endif
		public void Update()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnUpdate();
				}

				int actionExecutionCount = 0;
				_InterruptCount = 0;
				while (true)
				{
					if (_CurrentNode != null && _CurrentNode.IsExecutor() && _CurrentNode.isActive && _CurrentNode.status != NodeStatus.Running)
					{
						Pop(_CurrentNode.status);
					}

					if (_CurrentNode == null)
					{
						break;
					}

					_IsBreakPoint = false;

					Revaluation();

					bool execute = true;
					if (!_CurrentNode.isActive)
					{
						execute = _CurrentNode.Activate(true, false, false);

						TreeBehaviourNode treeBehaviourNode = _CurrentNode as TreeBehaviourNode;
						if (treeBehaviourNode != null && treeBehaviourNode.breakPoint)
						{
							_IsBreakPoint = true;
						}
					}

					if (execute && _IsBreakPoint)
					{
						_BehaviourTree.BreakNode(_CurrentNode);
						break;
					}

					NodeStatus status = NodeStatus.Running;

					if (execute)
					{
						TreeNodeBase currentNode = _CurrentNode;

						status = currentNode.Execute();

						if (currentNode != null && currentNode.IsExecutor())
						{
							actionExecutionCount++;

							bool isBreak = false;

							var executionSettings = _BehaviourTree.executionSettings;
							switch (executionSettings.type)
							{
								case ExecutionType.UntilRunning:
									isBreak = status == NodeStatus.Running;
									break;
								case ExecutionType.Count:
									isBreak = actionExecutionCount >= executionSettings.maxCount;
									break;
							}

							if (isBreak)
							{
								break;
							}
						}
					}
					else
					{
						_CurrentNode.Abort();
						status = _CurrentNode.status;
					}

					if (status != NodeStatus.Running)
					{
						Pop(status);

						if (_CurrentNode == null)
						{
							if (_OnFinish != null)
							{
								_OnFinish.Invoke(this, status);
							}
							else
							{
								Stop();
							}
							break;
						}
					}

					var debugLoopSettings = _BehaviourTree.currentDebugInfiniteLoopSettings;
					if (_InterruptCount >= debugLoopSettings.maxLoopCount)
					{
						if (debugLoopSettings.enableLogging)
						{
							Debug.LogWarning("Over " + debugLoopSettings.maxLoopCount + " interrupts per frame. Please check the infinite loop of " + ToString(), _BehaviourTree);
						}

						if (debugLoopSettings.enableBreak)
						{
							Debug.Break();
						}
						break;
					}

					if (!_BehaviourTree.isActiveAndEnabled)
					{
						break;
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateを実行する。
		/// </summary>
#else
		/// <summary>
		/// Perform an LateUpdate.
		/// </summary>
#endif
		public void LateUpdate()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnLateUpdate();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateを実行する。
		/// </summary>
#else
		/// <summary>
		/// Perform an FixedUpdate.
		/// </summary>
#endif
		public void FixedUpdate()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnFixedUpdate();
				}
			}
		}

		private TreeNodeBase CommonAncestorNode(TreeNodeBase node1, TreeNodeBase node2)
		{
			if (node1 == node2.parentNode)
			{
				return node1;
			}
			else if (node2 == node1.parentNode)
			{
				return node2;
			}

			HashSet<TreeNodeBase> parentNodes = new HashSet<TreeNodeBase>();

			TreeNodeBase parent1 = node1.parentNode;
			while (parent1 != null)
			{
				parentNodes.Add(parent1);
				parent1 = parent1.parentNode;
			}

			TreeNodeBase parent2 = node2.parentNode;
			TreeNodeBase current = parent2;
			while (current != null && parent2 != null && !parentNodes.Contains(current))
			{
				parent2 = parent2.parentNode;
				current = parent2;
			}

			return current;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再評価ノードかを返す。
		/// </summary>
		/// <param name="node">ノード</param>
		/// <returns>再評価ノードであればtrueを返す。</returns>
#else
		/// <summary>
		/// It returns the reevaluation node.
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>Returns true if it is a reevaluation node.</returns>
#endif
		public bool IsRevaluation(TreeNodeBase node)
		{
			return _Revaluators.Contains(node);
		}

		internal bool RegisterRevaluation(TreeNodeBase node)
		{
			if (!_Revaluators.Contains(node))
			{
				_Revaluators.Add(node);

				TreeBehaviourNode treeBehaviourNode = node as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.RevaluationEnter();
				}
				return true;
			}
			return false;
		}

		internal void RemoveRevaluator(TreeNodeBase node)
		{
			_Revaluators.Remove(node);
		}

		void AbortPop(TreeNodeBase targetNode)
		{
			while (_CurrentNode != null && _CurrentNode != targetNode)
			{
				_CurrentNode.Abort();
				Pop();
			}
		}

		bool IsRevaluationLowPriority(TreeNodeBase revaluationNode)
		{
			return !revaluationNode.isActive && revaluationNode.HasAbortFlags(AbortFlags.LowerPriority) && revaluationNode.priority < _CurrentNode.priority;
		}

		bool Revaluation(TreeNodeBase revaluationNode)
		{
			if (revaluationNode.isActive)
			{
				bool condition = revaluationNode.ConditionCheck(AbortFlags.Self);
				if (!condition)
				{
					if (_CurrentNode != revaluationNode)
					{
						AbortPop(revaluationNode);
					}

					revaluationNode.Abort();
					Pop(revaluationNode.status);

					return true;
				}
			}
			else if (IsRevaluationLowPriority(revaluationNode))
			{
				bool condition = revaluationNode.ConditionCheck(AbortFlags.LowerPriority);
				if (condition)
				{
					if (!revaluationNode.ConditionCheck(0))
					{
						return false;
					}

					TreeNodeBase commonAncestorNode = CommonAncestorNode(_CurrentNode, revaluationNode);
					if (commonAncestorNode == null)
					{
						return false;
					}

					List<TreeNodeBase> treeNodes = new List<TreeNodeBase>();
					treeNodes.Insert(0, revaluationNode);

					TreeNodeBase parentNode = revaluationNode.parentNode;
					while (parentNode != null && parentNode != commonAncestorNode)
					{
						if (parentNode.HasConditionCheck())
						{
							if (!parentNode.ConditionCheck(0))
							{
								return false;
							}
						}

						treeNodes.Insert(0, parentNode);
						parentNode = parentNode.parentNode;
					}

					treeNodes.Insert(0, commonAncestorNode);

					int revaluatorCount = _Revaluators.Count;
					for (int revaluatorIndex = revaluatorCount - 1; revaluatorIndex >= 0; --revaluatorIndex)
					{
						TreeNodeBase revaluator = _Revaluators[revaluatorIndex];
						if (revaluationNode.priority < revaluator.priority)
						{
							_Revaluators.RemoveAt(revaluatorIndex);

							TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
							if (treeBehaviourNode != null)
							{
								treeBehaviourNode.RevaluationExit();
							}
						}
					}

					AbortPop(commonAncestorNode);

					int parentCount = treeNodes.Count;
					for (int parentIndex = 0; parentIndex < parentCount - 1; parentIndex++)
					{
						TreeNodeBase node = treeNodes[parentIndex];
						TreeNodeBase childNode = treeNodes[parentIndex + 1];

						CompositeNode compositeNode = node as CompositeNode;
						int interruptSlot = (compositeNode != null) ? compositeNode.OnInterrupt(childNode) : 0;
						if (interruptSlot != 0)
						{
							Push(interruptSlot);
						}
						else
						{
							Push(childNode);
						}

						childNode.Activate(true, true, childNode == revaluationNode);

						TreeBehaviourNode treeBehaviourNode = childNode as TreeBehaviourNode;
						if (treeBehaviourNode != null && treeBehaviourNode.breakPoint)
						{
							_IsBreakPoint = true;
						}
					}

					_InterruptCount++;

					return true;
				}
			}

			return false;
		}

		bool Revaluation()
		{
			int count = _Revaluators.Count;
			for (int i = 0; i < count; ++i)
			{
				TreeNodeBase revaluator = _Revaluators[i];
				if (Revaluation(revaluator))
				{
					return true;
				}
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生を一時停止。
		/// </summary>
#else
		/// <summary>
		/// Pause playback.
		/// </summary>
#endif
		public void Pause()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			playState = PlayState.Pausing;

			TreeNodeBase current = _CurrentNode;
			while (current != null)
			{
				current.Pause();
				current = current.parentNode;
			}

			int count = _Revaluators.Count;
			for (int i = 0; i < count; i++)
			{
				TreeBehaviourNode revaluator = _Revaluators[i] as TreeBehaviourNode;
				if (revaluator != null && IsRevaluationLowPriority(revaluator))
				{
					revaluator.PauseDecorators();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生を再開。
		/// </summary>
#else
		/// <summary>
		/// Resume playing.
		/// </summary>
#endif
		public void Resume()
		{
			if (playState != PlayState.Pausing)
			{
				return;
			}

			playState = PlayState.Playing;

			TreeNodeBase current = _CurrentNode;
			Stack<TreeNodeBase> stack = new Stack<TreeNodeBase>();
			while (current != null)
			{
				stack.Push(current);
				current = current.parentNode;
			}

			while (stack.Count > 0)
			{
				TreeNodeBase node = stack.Pop();
				node.Resume();
			}

			int count = _Revaluators.Count;
			for (int i = 0; i < count; i++)
			{
				TreeBehaviourNode revaluator = _Revaluators[i] as TreeBehaviourNode;
				if (revaluator != null && IsRevaluationLowPriority(revaluator))
				{
					revaluator.ResumeDecorators();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生停止。
		/// </summary>
#else
		/// <summary>
		/// Stopping playback.
		/// </summary>
#endif
		public void Stop()
		{
			if (playState == PlayState.Stopping)
			{
				return;
			}

			playState = PlayState.Stopping;

			while (_CurrentNode != null)
			{
				_CurrentNode.Stop();
				_ActiveNodes.Remove(_CurrentNode);

				if (_CurrentNode == rootNode)
				{
					_CurrentNode = null;
				}
				else
				{
					var parentLinkSlotHolder = _CurrentNode as IParentLinkSlotHolder;

					if (parentLinkSlotHolder != null)
					{
						TreeNodeBase parentNode = null;

						NodeBranch branch = parentLinkSlotHolder.GetParentBranch();
						if (branch != null)
						{
							branch.isActive = false;
							parentNode = _BehaviourTree.GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
						}
						_CurrentNode = parentNode;
					}
					else
					{
						_CurrentNode = null;
					}
				}

				if (_CurrentNode != null)
				{
					_CurrentNode.executor = this;
				}
			}

			foreach (var revaluator in _Revaluators)
			{
				TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.RevaluationExit();
				}
			}
			_Revaluators.Clear();
		}
	}
}