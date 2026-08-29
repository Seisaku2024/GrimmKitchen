//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.IO;
using UnityEngine;
using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ArborEditor
{
    public sealed class EditorResources : Arbor.ScriptableSingleton<EditorResources>
    {
        const string k_DirectoryName = "EditorResources";

        private IAssetLoader _AssetLoader;
        private IAssetLoader loader
        {
            get
            {
                if (_AssetLoader == null)
                {
                    var packageInfo = PackageInfo.FindForAssembly(typeof(EditorResources).Assembly);

                    if (packageInfo != null)
                    {
                        // Package Manager 経由の場合
                        _ArborRootDirectory = packageInfo.assetPath;
                    }
                    else
                    {
                        // Assets/Plugins/Arbor に直接配置されている場合
                        _ArborRootDirectory = FindArborRootDirectory();
                    }

                    if (string.IsNullOrEmpty(_ArborRootDirectory))
                    {
                        Debug.LogError("Arbor root directory could not be found.");
                        return null;
                    }

                    _Directory = PathUtility.Combine(
                        _ArborRootDirectory,
                        k_DirectoryName);

                    _AssetLoader = new AssetDatabaseLoader(_Directory);
                }

                return _AssetLoader;
            }
        }

        private string _ArborRootDirectory;

        public static string arborRootDirectory
        {
            get
            {
                return instance._ArborRootDirectory;
            }
        }

        private string _Directory;

        public static string directory
        {
            get
            {
                return instance._Directory;
            }
        }

        private static string FindArborRootDirectory()
        {
            // まず現在の配置を直接確認
            const string defaultPath = "Assets/Plugins/Arbor";

            if (AssetDatabase.IsValidFolder(defaultPath))
            {
                return defaultPath;
            }

            // Arbor フォルダを検索
            string[] guids = AssetDatabase.FindAssets("EditorResources t:MonoScript");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!path.EndsWith("/EditorResources.cs"))
                {
                    continue;
                }

                // 例:
                // Assets/Plugins/Arbor/Internal/Editor/EditorResources.cs
                string normalizedPath = path.Replace("\\", "/");

                const string marker = "/Internal/Editor/EditorResources.cs";
                int index = normalizedPath.LastIndexOf(marker);

                if (index >= 0)
                {
                    return normalizedPath.Substring(0, index);
                }
            }

            return null;
        }

        public static Object Load(string name, System.Type type)
        {
            IAssetLoader loader = instance.loader;
            if (loader == null)
            {
                return null;
            }

            return loader.Load(name, type);
        }

        public static T Load<T>(string name) where T : Object
        {
            IAssetLoader loader = instance.loader;
            if (loader == null)
            {
                return null;
            }

            return loader.Load<T>(name);
        }

        public static Object Load(string name, string ext, System.Type type)
        {
            IAssetLoader loader = instance.loader;
            if (loader == null)
            {
                return null;
            }

            Object obj = loader.Load(name, type);
            if (obj != null)
            {
                return obj;
            }

            return loader.Load(Path.ChangeExtension(name, ext), type);
        }

        public static T Load<T>(string name, string ext) where T : Object
        {
            IAssetLoader loader = instance.loader;
            if (loader == null)
            {
                return null;
            }

            T obj = loader.Load<T>(name);
            if (obj != null)
            {
                return obj;
            }

            return loader.Load<T>(Path.ChangeExtension(name, ext));
        }

        public static Texture2D LoadTexture(string name)
        {
            Texture2D tex = Load<Texture2D>(name);
            if (tex != null)
            {
                return tex;
            }

            tex = Load<Texture2D>(name + ".png");
            return tex;
        }
    }
}