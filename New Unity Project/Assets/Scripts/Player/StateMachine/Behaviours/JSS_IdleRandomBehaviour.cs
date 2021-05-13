using UnityEngine;

public class JSS_IdleRandomBehaviour : StateMachineBehaviour
{
	public delegate void OnIdleRandomExit();
	public static OnIdleRandomExit onIdleRandomExit;
	
    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo
		stateInfo,
		int layerIndex
	){
		onIdleRandomExit?.Invoke();
	}
}