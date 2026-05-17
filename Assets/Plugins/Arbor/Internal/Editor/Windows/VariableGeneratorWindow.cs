//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ArborEditor
{
	internal sealed class VariableGeneratorWindow : EditorWindow
	{
		private static VariableGeneratorWindow _Instance = null;

		public static VariableGeneratorWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					VariableGeneratorWindow[] objects = Resources.FindObjectsOfTypeAll<VariableGeneratorWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = CreateInstance<VariableGeneratorWindow>();
				}

				return _Instance;
			}
		}

		[MenuItem("Assets/Create/Arbor/Variable C# Script", false, 102)]
		static void OnMenu()
		{
			instance.Open(UnityEditorBridge.ProjectWindowUtilBridge.GetActiveFolderPath());
		}

		private string _Path;
		private string _VariableName;
		private bool _OpenEditor = true;
		private string _ErrorMessage = string.Empty;

		void Open(string path)
		{
			_Path = path;

			_VariableName = string.Empty;
			_ErrorMessage = GetErrorMessage(_Path, _VariableName);

			instance.ShowAuxWindow();
		}

		static string GetVariableClassName(string variableName)
		{
			return variableName + "Variable";
		}

		static string GetVariableListClassName(string variableName)
		{
			return variableName + "ListVariable";
		}

		static string GetFlexibleClassName(string variableName)
		{
			return "Flexible" + variableName;
		}

		static string GetFlexibleListClassName(string variableName)
		{
			return "FlexibleList" + variableName;
		}

		static string GetInputSlotClassName(string variableName)
		{
			return "InputSlot" + variableName;
		}

		static string GetInputSlotListClassName(string variableName)
		{
			return "InputSlotList" + variableName;
		}

		static string GetOutputSlotClassName(string variableName)
		{
			return "OutputSlot" + variableName;
		}

		static string GetOutputSlotListClassName(string variableName)
		{
			return "OutputSlotList" + variableName;
		}

		static string GetVariableFileName(string variableName)
		{
			return GetVariableClassName(variableName) + ".cs";
		}

		static string GetVariableListFileName(string variableName)
		{
			return GetVariableListClassName(variableName) + ".cs";
		}

		static Object CreateScript(string pathName, string variableName, string templateName)
		{
			string template = string.Empty;
			TextAsset templateAsset = EditorResources.Load<TextAsset>(PathUtility.Combine("ScriptTemplates", templateName), ".txt");
			if (templateAsset != null)
			{
				template = templateAsset.text;
			}

			string content = template.Replace("#VARIABLENAME#", variableName);
			return DoCreateScriptAsset.CreateScriptAssetFromTemplate(pathName, content);
		}

		static Object CreateVariable(string path, string variableName)
		{
			string fileName = GetVariableFileName(variableName);
			string pathName = PathUtility.Combine(path, fileName);

			return CreateScript(pathName, variableName, "C# Script-NewVariableScript");
		}

		static Object CreateVariableList(string path, string variableName)
		{
			string fileName = GetVariableListFileName(variableName);
			string pathName = PathUtility.Combine(path, fileName);

			return CreateScript(pathName, variableName, "C# Script-NewVariableListScript");
		}

		static string GetErrorMessage(string path, string variableName)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				return "Please enter VariableName.";
			}
			
			StringBuilder sb = new StringBuilder();

			char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
			string variableFileName = GetVariableFileName(variableName);
			if (variableFileName.IndexOfAny(invalidChars) >= 0)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(variableFileName + " is invalid file name.");
			}

			string variableFilePath = PathUtility.Combine(path, variableFileName);
			if (File.Exists(variableFilePath))
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(variableFilePath + " file is already exists.");
			}

			string variableListFileName = GetVariableListFileName(variableName);
			if (variableListFileName.IndexOfAny(invalidChars) >= 0)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(variableListFileName + " is invalid file name.");
			}

			string variableListFilePath = PathUtility.Combine(path, variableListFileName);
			if (File.Exists(variableListFilePath))
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(variableListFilePath + " file is already exists.");
			}

			if (!System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(variableName))
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(variableName + " class is invalid name.");
			}
			
			ValidTypeName(variableName, sb);
			ValidTypeName(GetVariableClassName(variableName), sb);
			ValidTypeName(GetFlexibleClassName(variableName), sb);
			ValidTypeName(GetInputSlotClassName(variableName), sb);
			ValidTypeName(GetOutputSlotClassName(variableName), sb);

			ValidTypeName(GetVariableListClassName(variableName), sb);
			ValidTypeName(GetFlexibleListClassName(variableName), sb);
			ValidTypeName(GetInputSlotListClassName(variableName), sb);
			ValidTypeName(GetOutputSlotListClassName(variableName), sb);

			return sb.ToString();
		}

		static void ValidTypeName(string typeName, StringBuilder sb)
		{
			System.Type type = AssemblyHelper.GetTypeByName(typeName);
			if (type != null)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(typeName + " class is already exists");
			}
		}

		private void OnEnable()
		{
			titleContent = GUIContentCaches.Get("Variable Generator");
		}

		private void OnGUI()
		{
			EditorGUI.BeginChangeCheck();
			_VariableName = EditorGUILayout.TextField(GUIContentCaches.Get("Variable Name"), _VariableName);
			if (EditorGUI.EndChangeCheck())
			{
				_ErrorMessage = GetErrorMessage(_Path, _VariableName);
			}

			if (!string.IsNullOrEmpty(_ErrorMessage))
			{
				EditorGUILayout.HelpBox(_ErrorMessage, MessageType.Error);
			}

			GUILayout.FlexibleSpace();

			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				_OpenEditor = EditorGUILayout.Toggle("OpenEditor", _OpenEditor, GUILayout.ExpandWidth(false));
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				using (new EditorGUI.DisabledScope(!string.IsNullOrEmpty(_ErrorMessage)))
				{
					if (GUILayout.Button("Create"))
					{
						Object script = CreateVariable(_Path, _VariableName);
						CreateVariableList(_Path, _VariableName);
						if (_OpenEditor)
						{
							AssetDatabase.OpenAsset(script);
						}
						Selection.activeObject = script;
					}
				}
			}

			if (Event.current.type == EventType.MouseDown)
			{
				GUIUtility.hotControl = GUIUtility.keyboardControl = 0;
				Event.current.Use();
			}
		}
	}
}