using UnityEngine;

public class JSS_OnGrapplingBehaviour : StateMachineBehaviour
{
	public delegate void OnGrapplingUpdateChanged();
	
	public static OnGrapplingUpdateChanged
		onAnticipationExit;
	
    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onAnticipationExit?.Invoke();
	}
}