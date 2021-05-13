using UnityEngine;

public class AnimationSpeedCurve : StateMachineBehaviour
{
	static readonly string
		playSpeedParam = "PlaySpeed",
		setPlaySpeedParam = "SetPlaySpeed";
	
    override public void OnStateUpdate(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		float playSpeed = animator.GetFloat(playSpeedParam);
		animator.SetFloat(setPlaySpeedParam, playSpeed);
	}
}