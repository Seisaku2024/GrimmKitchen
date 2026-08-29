//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal static class ArborScriptCreator
	{
		static readonly string _CalculatorCSharpTemplatePath = @"C# Script-NewCalculatorScript";

		[MenuItem("Assets/Create/Arbor/Calculator C# Script", false, 101)]
		public static void CreateCSharpScriptCalculator()
		{
			StartNameEditing("NewCalculatorScript.cs", _CalculatorCSharpTemplatePath);
		}

		internal static void StartNameEditing(string pathName, string resourceFile)
		{
#if UNITY_6000_4_OR_NEWER
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(EntityId.None, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), pathName, DefaultScriptIcon.CSharpIcon, resourceFile);
#else
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), pathName, DefaultScriptIcon.CSharpIcon, resourceFile);
#endif
		}
	}
}
