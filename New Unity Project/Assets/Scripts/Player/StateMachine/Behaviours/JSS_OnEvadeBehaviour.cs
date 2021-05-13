using UnityEngine;

public class JSS_OnEvadeBehaviour : StateMachineBehaviour
{
	[SerializeField] private bool useAsEnter, useAsExit;
	
	public delegate void OnEvadeUpdate();
	public static OnEvadeUpdate onEvadeEnter, onEvadeExit;
	
	override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		if(useAsEnter){ onEvadeEnter?.Invoke(); }
	}
	
	override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		if(useAsExit){ onEvadeExit?.Invoke(); }
	}
}