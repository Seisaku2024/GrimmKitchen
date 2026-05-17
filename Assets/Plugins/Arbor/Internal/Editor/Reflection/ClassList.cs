//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.Build;
using Assembly = System.Reflection.Assembly;
using CompilationAssembly = UnityEditor.Compilation.Assembly;

namespace ArborEditor
{
	using Arbor;

	public static class ClassList
	{
		private const bool kUseThread = true;

		public static ReadOnlyCollection<Assembly> runtimeAssemblies
		{
			get;
			private set;
		}

		static HashSet<Assembly> _Assemblies = new HashSet<Assembly>();
		static Dictionary<string, NamespaceItem> _NamespaceDic = new Dictionary<string, NamespaceItem>();
		static Dictionary<AssemblyName, Assembly> _AssemblieDic = new Dictionary<AssemblyName, Assembly>();

		static List<NamespaceItem> _Namespaces = new List<NamespaceItem>();
		static List<TypeItem> _TypeItems = new List<TypeItem>();

		static Thread _CreateMethodThread = null;

		enum BuildStatus
		{
			None,
			DelayBuild,
			Building,
			BuildingForce,
			Ready,
			Canceling,
		}

		private static volatile BuildStatus _BuildStatus = BuildStatus.None;

		public static bool isReady
		{
			get
			{
				if (_BuildStatus == BuildStatus.DelayBuild)
				{
					_BuildStatus = BuildStatus.BuildingForce;
					Build();
				}
				return _BuildStatus == BuildStatus.Ready;
			}
		}

		public static int namespaceCount
		{
			get
			{
				return _Namespaces.Count;
			}
		}

		static ClassList()
		{
			_BuildStatus = BuildStatus.DelayBuild;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			switch (_BuildStatus)
			{
				case BuildStatus.DelayBuild:
					if (!EditorApplication.isPlayingOrWillChangePlaymode)
					{
						_BuildStatus = BuildStatus.Building;
						Build();
					}
					break;
				case BuildStatus.Building:
					if (EditorApplication.isPlayingOrWillChangePlaymode)
					{
						_BuildStatus = BuildStatus.Canceling;
					}
					break;
				case BuildStatus.Ready:
					{
						EditorApplication.update -= OnUpdate;
					}
					break;
			}
		}

		#region Build

		internal static ReadOnlyCollection<Assembly> GetAssemblies()
		{
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ReflectionOnlyAssemblyResolve;
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ReflectionOnlyAssemblyResolve;

			var defines = EditorUserBuildSettings.activeScriptCompilationDefines;
			var buildTarget = EditorUserBuildSettings.activeBuildTarget;

			var assemblyPathList = new List<string>();

			foreach (var a in CompilationPipeline.GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies))
			{
				assemblyPathList.Add(a.outputPath);
			}

			foreach (var pluginImporter in PluginImporter.GetImporters(buildTarget))
			{
				if (pluginImporter.isNativePlugin)
					continue;

				if (!pluginImporter.ShouldIncludeInBuild())
					continue;

				var cpu = pluginImporter.GetPlatformData(buildTarget, "CPU");
				if (string.Equals(cpu, "None", StringComparison.OrdinalIgnoreCase))
					continue;

				if (!CompilationPipeline.IsDefineConstraintsCompatible(defines, pluginImporter.DefineConstraints))
					continue;

				assemblyPathList.Add(pluginImporter.assetPath);
			}

			var assemblyPaths = assemblyPathList.ToArray();

			foreach (var t in TypeCache.GetTypesDerivedFrom<IFilterBuildAssemblies>())
			{
				if (t.IsAbstract || t.IsInterface)
					continue;

				var filter = Activator.CreateInstance(t) as IFilterBuildAssemblies;
				assemblyPaths = filter.OnFilterAssemblies(BuildOptions.None, assemblyPaths);
			}

			HashSet<Assembly> loadAssemblies = new HashSet<Assembly>();

			foreach (var path in assemblyPaths)
			{
				var fullPath = System.IO.Path.GetFullPath(path);
				if (!TryGetAssembly(fullPath, out var a))
				{
					a = ReflectionOnlyLoadFrom(fullPath);
				}

				if (a != null)
					loadAssemblies.Add(a);
			}

			HashSet<string> unityEditorAssemblyPaths = new HashSet<string>(CompilationPipeline.GetPrecompiledAssemblyPaths(CompilationPipeline.PrecompiledAssemblySources.UnityEditor));
			foreach (var path in CompilationPipeline.GetPrecompiledAssemblyPaths(CompilationPipeline.PrecompiledAssemblySources.UnityEngine))
			{
				if (unityEditorAssemblyPaths.Contains(path))
					continue;

				var systemPath = PathUtility.GetSystemPath(path);
				if (!TryGetAssembly(systemPath, out var a))
				{
					a = ReflectionOnlyLoadFrom(systemPath);
				}

				if (a != null)
					loadAssemblies.Add(a);
			}

			foreach (var path in CompilationPipeline.GetPrecompiledAssemblyPaths(CompilationPipeline.PrecompiledAssemblySources.SystemAssembly))
			{
				if (path.Contains("unityscript"))
					continue;

				var systemPath = PathUtility.GetSystemPath(path);
				if (!TryGetAssembly(systemPath, out var a))
				{
					a = ReflectionOnlyLoadFrom(systemPath);
				}

				if (a != null)
					loadAssemblies.Add(a);
			}

			List<Assembly> result = new List<Assembly>();
			foreach (var a in loadAssemblies)
			{
				result.Add(a);
			}

			return result.AsReadOnly();
		}

		static Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		{
			return Assembly.ReflectionOnlyLoad(args.Name);
		}

		static void Build()
		{
			runtimeAssemblies = GetAssemblies();
			
			BeginCreateList();
		}

		static void BeginCreateList()
		{
#pragma warning disable 0162
			if (kUseThread)
			{
				_CreateMethodThread = new Thread(new ThreadStart(CreateMethodList));
				_CreateMethodThread.Start();
			}
			else
			{
				CreateMethodList();
			}
#pragma warning restore 0162
		}

		static bool TryGetAssembly(string path, out Assembly assembly)
		{
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					if (a.Location == path)
					{
						assembly = a;
						return true;
					}
				}
				catch { }
			}

			assembly = null;
			return false;
		}

		static Assembly LoadFrom(string path)
		{
			try
			{
				return Assembly.LoadFrom(path);
			}
			catch
			{
				return null;
			}
		}

		static Assembly ReflectionOnlyLoadFrom(string path)
		{
			try
			{
				return Assembly.ReflectionOnlyLoadFrom(path);
			}
			catch 
			{
				return null;
			}
		}

		static Dictionary<Type, bool> s_HideTypes = new Dictionary<Type, bool>();

		static bool IsHideType(Type type)
		{
			bool isHideType = false;
			if (s_HideTypes.TryGetValue(type, out isHideType))
			{
				return isHideType;
			}

			for (Type current = type; current != null; current = current.BaseType)
			{
				bool foundHideType = false;
				bool forChildren = true;

				if (current.Assembly.ReflectionOnly)
				{
					try
					{
						foreach (var cad in CustomAttributeData.GetCustomAttributes(current))
						{
							if (cad.AttributeType.FullName == typeof(HideTypeAttribute).FullName)
							{
								foundHideType = true;
								try
								{
									if (cad.ConstructorArguments.Count == 1)
									{
										forChildren = (bool)cad.ConstructorArguments[0].Value;
									}
								}
								catch { }
								break;
							}
						}
					}
					catch { }
				}
				else
				{
					HideTypeAttribute hideType = AttributeHelper.GetAttribute<HideTypeAttribute>(current);
					if (hideType != null)
					{
						foundHideType = true;
						forChildren = hideType.forChildren;
					}
				}
				if (foundHideType)
				{
					if (type == current || forChildren)
					{
						isHideType = true;
						break;
					}
				}
			}

			s_HideTypes.Add(type, isHideType);

			return isHideType;
		}

		static bool ListUpTypes(Assembly assembly)
		{
			if (assembly == null)
			{
				return false;
			}

			if (!_Assemblies.Contains(assembly))
			{
				_Assemblies.Add(assembly);

				var types = TypeUtility.GetTypesFromAssembly(assembly);
				for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
				{
					Type type = types[typeIndex];
					if (type.IsVisible && !type.IsSubclassOf(typeof(Attribute)) && !type.IsGenericType && !type.IsNested && !IsHideType(type))
					{
						string Namespace = type.Namespace;
						if (string.IsNullOrEmpty(Namespace))
						{
							Namespace = "<unnamed>";
						}

						NamespaceItem namespaceItem = null;
						if (!_NamespaceDic.TryGetValue(Namespace, out namespaceItem))
						{
							namespaceItem = new NamespaceItem();
							namespaceItem.name = Namespace;

							_NamespaceDic.Add(Namespace, namespaceItem);
							_Namespaces.Add(namespaceItem);
						}

						namespaceItem.typeIndices.Add(AddType(type));
					}

					if (_BuildStatus == BuildStatus.Canceling)
					{
						break;
					}
				}

				return true;
			}

			return false;
		}

		static void ClearAssemblyHashes()
		{
			_Assemblies.Clear();
			_NamespaceDic.Clear();
			_AssemblieDic.Clear();
		}

		static int SortTypeIndices(int lhs, int rhs)
		{
			TypeItem lhsType = _TypeItems[lhs];
			TypeItem rhsType = _TypeItems[rhs];
			return lhsType.CompareTo(rhsType);
		}

		static void CreateMethodList()
		{
			try
			{
				_Assemblies.Clear();
				_Namespaces.Clear();
				_TypeItems.Clear();

				for (int assemblyIndex = 0; assemblyIndex < runtimeAssemblies.Count; assemblyIndex++)
				{
					Assembly assembly = runtimeAssemblies[assemblyIndex];
					ListUpTypes(assembly);

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_Namespaces.Sort();

				for (int namespaceIndex = 0; namespaceIndex < _Namespaces.Count; namespaceIndex++)
				{
					NamespaceItem namespaceItem = _Namespaces[namespaceIndex];
					namespaceItem.typeIndices.Sort(SortTypeIndices);

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_BuildStatus = BuildStatus.Ready;
			}
			finally
			{
				ClearAssemblyHashes();

				GC.Collect();

				if (_BuildStatus == BuildStatus.Canceling)
				{
					_BuildStatus = BuildStatus.DelayBuild;
				}
			}
		}

		static int AddType(Type type)
		{
			_TypeItems.Add(new TypeItem(type));
			return _TypeItems.Count - 1;
		}

		public static TypeItem GetType(int index)
		{
			return _TypeItems[index];
		}

		#endregion

		public static NamespaceItem GetNamespaceItem(int index)
		{
			return _Namespaces[index];
		}

		public static TypeItem GetTypeItem(Type type)
		{
			if (type == null)
			{
				return null;
			}

			int namespaceCount = _Namespaces.Count;
			for (int namespaceIndex = 0; namespaceIndex < namespaceCount; namespaceIndex++)
			{
				NamespaceItem namespaceItem = _Namespaces[namespaceIndex];
				int typeCount = namespaceItem.typeIndices.Count;
				for (int typeIndex = 0; typeIndex < typeCount; typeIndex++)
				{
					TypeItem typeItem = GetType(namespaceItem.typeIndices[typeIndex]);
					TypeItem findTypeItem = typeItem.GetTypeItem(type);
					if (findTypeItem != null)
					{
						return findTypeItem;
					}
				}
			}

			return null;
		}

		#region Item classes

		public class Item : System.IComparable
		{
			public string name;

			public int CompareTo(object obj)
			{
				return name.CompareTo((obj as Item).name);
			}
		}

		public sealed class TypeItem : Item
		{
			private RuntimeTypeHandle _TypeHandle;
			public Type type
			{
				get
				{
					return Type.GetTypeFromHandle(_TypeHandle);
				}
			}

			private List<int> _NestedTypeIndices = new List<int>();
			public List<int> nestedTypeIndices
			{
				get
				{
					return _NestedTypeIndices;
				}
			}

			public TypeItem(Type type)
			{
				this.name = Arbor.TypeUtility.GetTypeName(type);
				_TypeHandle = type.TypeHandle;

				CreateNestedTypes(type);
			}

			void CreateNestedTypes(Type type)
			{
				_NestedTypeIndices.Clear();

				Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public);
				int nestedTypeCount = nestedTypes.Length;
				for (int nestedTypeIndex = 0; nestedTypeIndex < nestedTypeCount; nestedTypeIndex++)
				{
					Type nestedType = nestedTypes[nestedTypeIndex];

					if (!nestedType.IsSubclassOf(typeof(Attribute)) && !nestedType.IsGenericType && !IsHideType(nestedType))
					{
						_NestedTypeIndices.Add(AddType(nestedType));
					}

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_NestedTypeIndices.Sort(SortTypeIndices);
			}

			public TypeItem GetTypeItem(Type type)
			{
				if (type == null)
				{
					return null;
				}

				if (type == this.type)
				{
					return this;
				}

				int nestedTypeCount = _NestedTypeIndices.Count;
				for (int nestedTypeIndex = 0; nestedTypeIndex < nestedTypeCount; nestedTypeIndex++)
				{
					TypeItem nestedType = ClassList.GetType((_NestedTypeIndices[nestedTypeIndex]));
					TypeItem findTypeItem = nestedType.GetTypeItem(type);
					if (findTypeItem != null)
					{
						return findTypeItem;
					}
				}

				return null;
			}
		}

		public sealed class NamespaceItem : Item
		{
			public List<int> typeIndices = new List<int>();
		}

		#endregion
	}
}
