using UnityEngine;

public class JSS_OnEvadeBoostBehaviour : StateMachineBehaviour
{
	public delegate void OnBoostUpdateChange();
	public static OnBoostUpdateChange onBoostExit;
	
    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onBoostExit?.Invoke();
	}
}