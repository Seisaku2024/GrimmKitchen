//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor.Compilation;

namespace ArborEditor
{
	public sealed class DoCreateScriptAsset : EndNameEditAction
	{
		internal static string RemoveOrInsertNamespace(string content, string rootNamespace)
		{
			var rootNamespaceBeginTag = "#ROOTNAMESPACEBEGIN#";
			var rootNamespaceEndTag = "#ROOTNAMESPACEEND#";

			if (!content.Contains(rootNamespaceBeginTag) || !content.Contains(rootNamespaceEndTag))
				return content;

			if (string.IsNullOrEmpty(rootNamespace))
			{
				content = Regex.Replace(content, $"((\\r\\n)|\\n)?[ \\t]*{rootNamespaceBeginTag}[ \\t]*", string.Empty);
				content = Regex.Replace(content, $"((\\r\\n)|\\n)?[ \\t]*{rootNamespaceEndTag}[ \\t]*", string.Empty);

				return content;
			}

			// Use first found newline character as newline for entire file after replace.
			var newline = content.Contains("\r\n") ? "\r\n" : "\n";
			var contentLines = new List<string>(content.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None));

			int i = 0;

			for (; i < contentLines.Count; ++i)
			{
				if (contentLines[i].Contains(rootNamespaceBeginTag))
					break;
			}

			var beginTagLine = contentLines[i];

			// Use the whitespace between beginning of line and #ROOTNAMESPACEBEGIN# as identation.
			var indentationString = beginTagLine.Substring(0, beginTagLine.IndexOf("#"));

			contentLines[i] = $"namespace {rootNamespace}";
			contentLines.Insert(i + 1, "{");

			i += 2;

			for (; i < contentLines.Count; ++i)
			{
				var line = contentLines[i];

				if (string.IsNullOrEmpty(line) || line.Trim().Length == 0)
					continue;

				if (line.Contains(rootNamespaceEndTag))
				{
					contentLines[i] = "}";
					break;
				}

				contentLines[i] = $"{indentationString}{line}";
			}

			return string.Join(newline, contentLines.ToArray());
		}

#if ARBOR_DLL
		static System.Func<string, string> s_GetAssemblyRootNamespaceFromScriptPath;
#endif

		static DoCreateScriptAsset()
		{
#if ARBOR_DLL
			var getAssemblyRootNamespaceFromScriptPathMethod = typeof(CompilationPipeline).GetMethod("GetAssemblyRootNamespaceFromScriptPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			if (getAssemblyRootNamespaceFromScriptPathMethod != null)
			{
				s_GetAssemblyRootNamespaceFromScriptPath = (System.Func<string, string>)System.Delegate.CreateDelegate(typeof(System.Func<string, string>), getAssemblyRootNamespaceFromScriptPathMethod);
			}
#endif
		}

		internal static string PreprocessScriptAssetTemplate(string pathName, string resourceContent)
		{
			string rootNamespace = null;

			if (Path.GetExtension(pathName) == ".cs")
			{
#if ARBOR_DLL
				if (s_GetAssemblyRootNamespaceFromScriptPath != null)
				{
					rootNamespace = s_GetAssemblyRootNamespaceFromScriptPath(pathName);
				}
#elif UNITY_2020_2_OR_NEWER
				rootNamespace = CompilationPipeline.GetAssemblyRootNamespaceFromScriptPath(pathName);
#endif
			}

			string content = resourceContent;

			// #NOTRIM# is a special marker that is used to mark the end of a line where we want to leave whitespace. prevent editors auto-stripping it by accident.
			content = content.Replace("#NOTRIM#", "");

			// macro replacement
			string baseFile = Path.GetFileNameWithoutExtension(pathName);

			content = content.Replace("#NAME#", baseFile);
			string baseFileNoSpaces = baseFile.Replace(" ", "");
			content = content.Replace("#SCRIPTNAME#", baseFileNoSpaces);

			content = RemoveOrInsertNamespace(content, rootNamespace);

			// if the script name begins with an uppercase character we support a lowercase substitution variant
			if (char.IsUpper(baseFileNoSpaces, 0))
			{
				baseFileNoSpaces = char.ToLower(baseFileNoSpaces[0]) + baseFileNoSpaces.Substring(1);
				content = content.Replace("#SCRIPTNAME_LOWER#", baseFileNoSpaces);
			}
			else
			{
				// still allow the variant, but change the first character to upper and prefix with "my"
				baseFileNoSpaces = "my" + char.ToUpper(baseFileNoSpaces[0]) + baseFileNoSpaces.Substring(1);
				content = content.Replace("#SCRIPTNAME_LOWER#", baseFileNoSpaces);
			}

			return content;
		}

		internal static string SetLineEndings(string content, LineEndingsMode lineEndingsMode)
		{
			const string windowsLineEndings = "\r\n";
			const string unixLineEndings = "\n";

			string preferredLineEndings;

			switch (lineEndingsMode)
			{
				case LineEndingsMode.OSNative:
					if (Application.platform == RuntimePlatform.WindowsEditor)
						preferredLineEndings = windowsLineEndings;
					else
						preferredLineEndings = unixLineEndings;
					break;
				case LineEndingsMode.Unix:
					preferredLineEndings = unixLineEndings;
					break;
				case LineEndingsMode.Windows:
					preferredLineEndings = windowsLineEndings;
					break;
				default:
					preferredLineEndings = unixLineEndings;
					break;
			}

			content = Regex.Replace(content, @"\r\n?|\n", preferredLineEndings);

			return content;
		}

		internal static Object CreateScriptAssetFromTemplate(string pathName, string template)
		{
			string content = PreprocessScriptAssetTemplate(pathName, template);
			content = SetLineEndings(content, EditorSettings.lineEndingsForNewScripts);

			string fullPath = Path.GetFullPath(pathName);

			// In a Japanese environment, Visual Studio's character code defaults to SJIS.
			// Output as UTF-8 with BOM to avoid garbled characters on Unity.
			var encoding = new UTF8Encoding(true);
			File.WriteAllText(fullPath, content, encoding);

			AssetDatabase.ImportAsset(pathName);
			return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
		}

		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			string template = "";

			TextAsset templateAsset = EditorResources.Load<TextAsset>(PathUtility.Combine("ScriptTemplates", resourceFile), ".txt");
			if (templateAsset != null)
			{
				template = templateAsset.text;
			}

			ProjectWindowUtil.ShowCreatedAsset(CreateScriptAssetFromTemplate(pathName, template));
		}
	}
}
