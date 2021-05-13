using UnityEngine;

public class JSS_TouchdownBehaviour : StateMachineBehaviour
{
	public delegate void OnLandUpdate();
	public static OnLandUpdate onLandEnter, onLandExit; // listening from playerController rootmotion toggling
	
	override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLandEnter?.Invoke();
    }
	
	override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onLandExit?.Invoke();
    }
}