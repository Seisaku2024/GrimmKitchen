//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal static class ListViewExtensions
	{
		[System.Obsolete("use ListView.selectionChanged event")] // The minimum supported Unity version is now 6.0, so it is recommended to call ListView type members directly.
		public static void RegisterCallbackSelectionChange(this ListView listView, System.Action<IEnumerable<object>> onSelectionChange)
		{
			listView.selectionChanged += onSelectionChange;
		}

		[System.Obsolete("use ListView.selectionChanged event")] // The minimum supported Unity version is now 6.0, so it is recommended to call ListView type members directly.
		public static void UnregisterCallbackSelectionChange(this ListView listView, System.Action<IEnumerable<object>> onSelectionChange)
		{
			listView.selectionChanged -= onSelectionChange;
		}

		[System.Obsolete("use ListView.itemsChosen event")] // The minimum supported Unity version is now 6.0, so it is recommended to call ListView type members directly.
		public static void RegisterCallbackItemsChosen(this ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
		{
			listView.itemsChosen += onItemsChosen;
		}

		[System.Obsolete("use ListView.itemsChosen event")] // The minimum supported Unity version is now 6.0, so it is recommended to call ListView type members directly.
		public static void UnregisterCallbackItemsChosen(this ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
		{
			listView.itemsChosen -= onItemsChosen;
		}

		[System.Obsolete("use ListView.Rebuild")] // The minimum supported Unity version is now 6.0, so it is recommended to call ListView type members directly.
		public static void RebuildList(this ListView listView)
		{
			listView.Rebuild();
		}
	}
}