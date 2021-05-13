using UnityEngine;

public class JSS_JumpLaunchBehaviour : StateMachineBehaviour
{
	public delegate void OnLaunchUpdate();
	public static OnLaunchUpdate onLaunchEnter, onLaunchExit; // listen to PlayerJumpController;
	
	static readonly string isJumpedParam = "isJumped";
	
	override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLaunchEnter?.Invoke();
		animator.SetBool(isJumpedParam, true);
    }
	
	override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLaunchExit?.Invoke();
        animator.SetBool(isJumpedParam, false);
    }
}