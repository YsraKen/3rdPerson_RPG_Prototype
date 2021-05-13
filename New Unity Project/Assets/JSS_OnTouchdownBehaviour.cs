using UnityEngine;

public class JSS_OnTouchdownBehaviour : StateMachineBehaviour
{
	// public delegate void OnTouchDownEnter();
	// public static OnTouchDownEnter onTouchdownEnter;
	
    override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		// onTouchdownEnter?.Invoke();
	}
}