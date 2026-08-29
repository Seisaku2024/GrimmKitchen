//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class ListViewElement : ListView, ISerializationCallbackReceiver
	{
		public ListViewElement(IList itemsSource, int itemHeight, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
			: base(itemsSource, itemHeight, makeItem, bindItem)
		{
		}

		public event Action onAfterDeserialize;

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Because viewData restores the selection list
			// Reflects the node selection status of NodeGraphEditor
			onAfterDeserialize?.Invoke();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}