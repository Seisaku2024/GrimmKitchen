//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 現在のアクティブシーンをリスタートする。
	/// </summary>
#else
	/// <summary>
	/// Restart the current active scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Scene/RestartScene")]
	[BuiltInBehaviour]
	public sealed class RestartScene : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// シーンの読み込みが完了したときに遷移する
		/// </summary>
#else
		/// <summary>
		/// Transition when the scene loading is completed
		/// </summary>
#endif
		[SerializeField]
		private StateLink _Done = new StateLink();

		// Use this for enter state
		public override void OnStateBegin()
		{
			StartCoroutine(WaitLoad());
		}

		IEnumerator WaitLoad()
		{
			var scene = SceneManager.GetActiveScene();

#if UNITY_EDITOR
			if (scene.IsValid()
				&& scene.buildIndex == -1)
			{
				// Even if a scene is not registered in the Build Profile, it will be loaded using the editor function if it is valid.
				yield return UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(scene.path, 
					new LoadSceneParameters(LoadSceneMode.Single));
			}
			else
			{
				yield return SceneManager.LoadSceneAsync(scene.name);
			}
#else
			yield return SceneManager.LoadSceneAsync(scene.name);
#endif

			Transition(_Done);
		}
	}
}
