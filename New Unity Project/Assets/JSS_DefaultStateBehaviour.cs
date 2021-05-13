using UnityEngine;

public class JSS_DefaultStateBehaviour : StateMachineBehaviour
{
	public delegate void OnDefaultStateEnter();
	public static OnDefaultStateEnter onDefaultStateEnter;
	
    override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onDefaultStateEnter?.Invoke();
	}
}