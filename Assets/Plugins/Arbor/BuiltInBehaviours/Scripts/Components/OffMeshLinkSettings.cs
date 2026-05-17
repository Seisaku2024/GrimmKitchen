//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentControllerが<see cref="OffMeshLink"/>(または<a href="https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/NavMeshLink.html">NavMeshLink</a>)を横切る方法の設定を行う。
	/// </summary>
#else
	/// <summary>
	/// Set the method for the AgentController to traverse <see cref="OffMeshLink"/>(or <a href="https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/NavMeshLink.html">NavMeshLink</a>).
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/OffMeshLinkSettings")]
	[BuiltInComponent]
#if !ARBOR_OFFMESHLINKSETTINGS_DISABLE_REQUIRECOMPONENT
#if ARBOR_SUPPORT_AI_NAVIGATION && !ARBOR_OFFMESHLINKSETTINGS_USE_OFFMESHLINK
	[RequireComponent(typeof(Unity.AI.Navigation.NavMeshLink))]
#else
#pragma warning disable 0618
	[RequireComponent(typeof(OffMeshLink))]
#pragma warning restore
#endif
#endif
	[HelpURL(ArborReferenceUtility.docUrl + "manual/builtin/offmeshlinksettings.html")]
	[Internal.DocumentManual("/manual/builtin/offmeshlinksettings.md")]
	public class OffMeshLinkSettings : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OffMeshLink"/>を通過する方法の設定
		/// </summary>
#else
		/// <summary>
		/// Setting how to traverse <see cref="OffMeshLink"/>
		/// </summary>
#endif
		[SerializeField]
		private OffMeshLinkTraverseData _TraverseData = new OffMeshLinkTraverseData();

#if ARBOR_DOC_JA
		/// <summary>
		/// OffMeshLinkTraverseDataを取得する
		/// </summary>
#else
		/// <summary>
		/// Get OffMeshLinkTraverseData
		/// </summary>
#endif
		public OffMeshLinkTraverseData traverseData
		{
			get
			{
				return _TraverseData;
			}
		}
	}
}