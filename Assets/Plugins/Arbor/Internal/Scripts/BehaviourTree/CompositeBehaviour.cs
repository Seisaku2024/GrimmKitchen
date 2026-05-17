//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コンポジットの挙動を定義するクラス。継承して利用する。
	/// </summary>
#else
	/// <summary>
	/// Class that defines the behavior of the composite. Inherited and to use.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public abstract class CompositeBehaviour : TreeNodeBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeNodeを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the CompositeNode.
		/// </summary>
#endif
		public CompositeNode compositeNode
		{
			get
			{
				return node as CompositeNode;
			}
		}

		internal abstract bool IsExecutor();
		internal abstract int CalculateChildPropert(int order);
		internal abstract NodeStatus Execute();

		internal static CompositeBehaviour Create(Node node, System.Type type)
		{
			System.Type classType = typeof(CompositeBehaviour);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `CompositeBehaviour' in order to use it as parameter `type'", "type");
			}

			return CreateNodeBehaviour(node, type) as CompositeBehaviour;
		}
	}
}