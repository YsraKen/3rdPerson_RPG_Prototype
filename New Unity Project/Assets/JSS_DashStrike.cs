using UnityEngine;

public class JSS_DashStrike : StateMachineBehaviour
{
	public delegate void OnDashStrikeUpdateChanged();
	
	public static OnDashStrikeUpdateChanged
		onDashEnter, onDashUpdate, onDashExit;

	override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onDashEnter?.Invoke();
	}
	
	override public void OnStateUpdate(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onDashUpdate?.Invoke();
	}
	
	override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onDashExit?.Invoke();
	}
}