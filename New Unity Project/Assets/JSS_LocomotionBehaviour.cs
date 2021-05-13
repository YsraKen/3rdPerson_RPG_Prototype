using UnityEngine;

public class JSS_LocomotionBehaviour : StateMachineBehaviour
{
	public delegate void OnLocomotionUpdate();
	public static OnLocomotionUpdate onLocomotionEnter, onLocomotionExit; // listening from playerController rootmotion toggling
	
	override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLocomotionEnter?.Invoke();
    }
	
	override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLocomotionExit?.Invoke();
    }
}