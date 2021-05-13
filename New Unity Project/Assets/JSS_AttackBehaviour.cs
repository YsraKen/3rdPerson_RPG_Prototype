using UnityEngine;

public class JSS_AttackBehaviour : StateMachineBehaviour
{
	public delegate void OnAttackUpdateChanged();
	public static OnAttackUpdateChanged onAttackExit;
	
    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onAttackExit?.Invoke();
	}
}