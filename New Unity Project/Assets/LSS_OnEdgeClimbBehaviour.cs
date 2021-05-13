using UnityEngine;

public class LSS_OnEdgeClimbBehaviour : StateMachineBehaviour
{
	public delegate void OnEdgeClimbUpdate();
	public static OnEdgeClimbUpdate onEdgeClimbExit;
	
    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onEdgeClimbExit?.Invoke();
	}
}