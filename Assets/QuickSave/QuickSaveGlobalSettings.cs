////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using CI.QuickSave.Core.Serialisers;
using Newtonsoft.Json;
using UnityEngine;

namespace CI.QuickSave
{
    public static class QuickSaveGlobalSettings
    {
        /// <summary>
        /// The path to save data to - defaults to Application.persistentDataPath
        /// </summary>
        /// 

        //! @todo リリース時ifdef削除
        public static string StorageLocation { get; set; } =

#if UNITY_EDITOR
            "Assets/ProjectC/998_DebugSaveData";

#else
            Application.persistentDataPath;
#endif




        /// <summary>
        /// Register a new json converter
        /// </summary>
        /// <param name="converter">The json converter to register</param>
        public static void RegisterConverter(JsonConverter converter) => JsonSerialiser.RegisterConverter(converter);
    }
}