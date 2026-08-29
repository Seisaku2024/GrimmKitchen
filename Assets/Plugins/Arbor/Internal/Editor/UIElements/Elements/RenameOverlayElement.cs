//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class RenameOverlayElement : VisualElement
	{
		private TextField _TextField;

		private int _UserData;
		private bool _IsWaitingForDelay;
		private bool _IsRenaming;
		private string _OriginalValue;

		private System.Action<string, int> _OnRenameEnded = null;

		public int renameUserData
		{
			get
			{
				return _UserData;
			}
		}

		public bool isWaitingForDelay
		{
			get
			{
				return _IsWaitingForDelay;
			}
		}

		private VisualElement _AttachTarget;
		public VisualElement attachTarget
		{
			get
			{
				return _AttachTarget;
			}
			set
			{
				if (_AttachTarget != value)
				{
					if (_AttachTarget != null)
					{
						UnregisterCallbackFromAttachElement();
					}

					_AttachTarget = value;

					if (_AttachTarget != null)
					{
						RegisterCallbackOnAttachElement();

						AlignOnTarget();
					}
				}
			}
		}

		private List<VisualElement> _WatchedElements = null;

		void RegisterCallbackOnAttachElement()
		{
			var commonAncestor = _AttachTarget.FindCommonAncestor(this);

			if (_WatchedElements == null)
			{
				_WatchedElements = new List<VisualElement>();
			}

			VisualElement v = _AttachTarget;

			while (v != commonAncestor)
			{
				_WatchedElements.Add(v);
				v.RegisterCallback<GeometryChangedEvent>(OnTargetLayout);
				v = v.hierarchy.parent;
			}

			v = hierarchy.parent;

			while (v != commonAncestor)
			{
				_WatchedElements.Add(v);
				v.RegisterCallback<GeometryChangedEvent>(OnTargetLayout);
				v = v.hierarchy.parent;
			}
		}

		void UnregisterCallbackFromAttachElement()
		{
			_AttachTarget.visible = true;

			if (_WatchedElements == null || _WatchedElements.Count == 0)
				return;

			foreach (VisualElement v in _WatchedElements)
			{
				v.UnregisterCallback<GeometryChangedEvent>(OnTargetLayout);
			}

			_WatchedElements.Clear();
		}

		private void OnTargetLayout(GeometryChangedEvent evt)
		{
			AlignOnTarget();
		}

		internal void AlignOnTarget()
		{
			if (_AttachTarget == null || hierarchy.parent == null)
			{
				return;
			}

			Rect targetRect = _AttachTarget.layout;
			targetRect.position = Vector2.zero;

			this.SetLayout(_AttachTarget.ChangeCoordinatesTo(hierarchy.parent, targetRect));
		}

		public RenameOverlayElement(System.Action<string, int> onRenameEnded)
		{
			_OnRenameEnded = onRenameEnded;

			style.position = Position.Absolute;

			_TextField = new TextField()
			{
				isDelayed = true,
				style =
				{
					marginBottom = 0f,
					marginLeft = -5f,
					marginTop = 0f,
					marginRight = 0f,
					flexGrow = 1f,
					flexShrink = 0f,
					display = DisplayStyle.None,
				},
			};
			_TextField.RegisterCallback<BlurEvent>(OnBlur);
			Add(_TextField);

			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			if (_AttachTarget != null)
			{
				UnregisterCallbackFromAttachElement();

				_AttachTarget = null;
			}
		}

		private void OnBlur(BlurEvent evt)
		{
			EndRename(true);
		}

		public void BeginRename(string name, int userData, float delay)
		{
			_IsRenaming = true;
			_OriginalValue = name;
			_UserData = userData;

			_TextField.SetValueWithoutNotify(_OriginalValue);

			// Depending on the timing of the call, it may not be possible to focus immediately, so a one-frame delay is always performed.
			_IsWaitingForDelay = true;
			schedule.Execute(BeginRenaming).ExecuteLater(Mathf.FloorToInt(delay * 1000));
		}

		void BeginRenaming()
		{
			_IsWaitingForDelay = false;

			_TextField.SelectAll();
			_TextField.style.display = DisplayStyle.Flex;
			_TextField.Focus();

			if (_AttachTarget != null)
			{
				_AttachTarget.visible = false;
			}
		}

		public void EndRename(bool acceptChanges)
		{
			if (!_IsRenaming)
			{
				return;
			}
			
			RenameEnded(acceptChanges);
		}

		public bool IsRenaming()
		{
			return _IsRenaming;
		}

		private void RenameEnded(bool userAcceptedRename)
		{
			if (userAcceptedRename)
			{
				if (_OnRenameEnded != null)
				{
					var value = _TextField.value;
					string name = !string.IsNullOrEmpty(value) ? value : _OriginalValue;
					_OnRenameEnded(name, _UserData);
				}
			}

			_IsRenaming = false;
			_TextField.style.display = DisplayStyle.None;

			RemoveFromHierarchy();

			if (focusAfterComfirm != null)
			{
				focusAfterComfirm.Focus();
			}
		}

		public VisualElement focusAfterComfirm;
	}
}