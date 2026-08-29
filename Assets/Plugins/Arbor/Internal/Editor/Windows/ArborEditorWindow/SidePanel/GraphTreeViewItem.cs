//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.IMGUI.Controls;

	internal class GraphTreeViewItem : TreeViewItem
	{
		public ObjectId objectId
		{
			get;
			private set;
		}

		public NodeGraph nodeGraph
		{
			get;
			private set;
		}

		public virtual bool isExternal
		{
			get
			{
				return false;
			}
		}

		public override bool renamable
		{
			get
			{
				return (nodeGraph.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable && !isExternal;
			}
		}

		public GraphTreeViewItem(int id, ObjectId objectId, NodeGraph nodeGraph) : base(id, nodeGraph.graphName, null)
		{
			this.objectId = objectId;
			this.nodeGraph = nodeGraph;
			nodeGraph.onChangedGraphName += OnChangedGraphName;
		}

		public override void Dispose()
		{
			base.Dispose();

			if (nodeGraph is object)
			{
				nodeGraph.onChangedGraphName -= OnChangedGraphName;
			}
		}

		void OnChangedGraphName()
		{
			displayName = nodeGraph.graphName;
		}
	}
}