//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System;

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	internal sealed class ArborUpdateCheck : Arbor.ScriptableSingleton<ArborUpdateCheck>, IUpdateCallback
	{
		private const string kUpdateSkipVersionKey = "ArborEditor.UpdateSkipVersionString";

		private static string s_SkipVersion = null;
		public static string skipVersion
		{
			get
			{
				if (s_SkipVersion == null && EditorPrefs.HasKey(kUpdateSkipVersionKey))
				{
					s_SkipVersion = EditorPrefs.GetString(kUpdateSkipVersionKey, "");
				}
				return s_SkipVersion;
			}
			set
			{
				if (value != null && s_SkipVersion != value)
				{
					s_SkipVersion = value;
					EditorPrefs.SetString(kUpdateSkipVersionKey, value);
				}
			}
		}

		private UnityWebRequest _Request;

		public event Action onDone;

		private bool _IsDone;
		private UpdateInfo _UpdateInfo;

		public bool isDone
		{
			get
			{
				return _IsDone;
			}
		}

		public UpdateInfo updateInfo
		{
			get
			{
				return _UpdateInfo;
			}
		}

		public string latestVersion
		{
			get
			{
				string currentVersion = ArborVersion.version;

				if (!isDone || _UpdateInfo == null)
				{
					return currentVersion;
				}

				switch (ArborVersion.buildType)
				{
					case VersionInfo.BuildType.Release:
						if (_UpdateInfo.Release.Version != currentVersion)
						{
							return _UpdateInfo.Release.Version;
						}
						else if (_UpdateInfo.Patch.BaseVersion == currentVersion)
						{
							return _UpdateInfo.Patch.Version;
						}
						break;
					case VersionInfo.BuildType.Patch:
						if (_UpdateInfo.Release.Version != ArborVersion.baseVersion)
						{
							return _UpdateInfo.Release.Version;
						}
						if (_UpdateInfo.Patch.Version != currentVersion)
						{
							return _UpdateInfo.Patch.Version;
						}
						break;
				}

				return currentVersion;
			}
		}

		public bool isUpdated
		{
			get
			{
				if (!isDone || _UpdateInfo == null)
				{
					return false;
				}

				string currentVersion = ArborVersion.version;

				string latestVersion = this.latestVersion;

				return (currentVersion != latestVersion && skipVersion != latestVersion);
			}
		}

		public bool isRelease
		{
			get
			{
				if (!isDone || _UpdateInfo == null)
				{
					return false;
				}

				return (ArborVersion.version != _UpdateInfo.Release.Version || _UpdateInfo.Release.Version != _UpdateInfo.Patch.BaseVersion) && skipVersion != _UpdateInfo.Release.Version;
			}
		}

		public bool isUpgrade
		{
			get
			{
				if (!isDone || _UpdateInfo == null || !_UpdateInfo.Upgrade.IsValid())
				{
					return false;
				}

				return _UpdateInfo.Upgrade.IsValid();
			}
		}

		public void CheckStart(bool force = false)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable || _Request != null || (_IsDone && !force))
			{
				return;
			}

			_Request = UnityWebRequest.Get(ArborVersion.updateCheckURL);
			_Request.SendWebRequest();
			_IsDone = false;
			_UpdateInfo = null;

			EditorCallbackUtility.RegisterUpdateCallback(this);
		}

		void IUpdateCallback.OnUpdate()
		{
			if (!_Request.isDone)
			{
				return;
			}

			_IsDone = true;

			bool isError = false;
			UnityWebRequest.Result result = _Request.result;
			isError = result != UnityWebRequest.Result.Success;

			if (!isError)
			{
				string json = _Request.downloadHandler.text;
				try
				{
					_UpdateInfo = JsonUtility.FromJson<UpdateInfo>(json);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			else
			{
				_UpdateInfo = null;
			}
			_Request.Dispose();
			_Request = null;

			EditorCallbackUtility.UnregisterUpdateCallback(this);

			onDone?.Invoke();
			onDone = null;
		}
	}
}