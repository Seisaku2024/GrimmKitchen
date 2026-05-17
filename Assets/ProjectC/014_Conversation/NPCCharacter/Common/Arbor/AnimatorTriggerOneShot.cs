using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AnimatorTriggerOneShot : StateBehaviour {
	[SerializeField]
	private FlexibleField<Animator> m_animator;
	[SerializeField]
	private string m_triggerName = "";
	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
		base.OnStateBegin();
		if(m_animator.value)
		{
			m_animator.value.SetTriggerOneShot(m_triggerName);
		}
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
