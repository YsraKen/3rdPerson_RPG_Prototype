using UnityEngine;

public class JSS_AerialJumpBehaviour : StateMachineBehaviour
{
	public delegate void OnAerialJump();
	public static OnAerialJump onAerialJump;
	
    override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		onAerialJump?.Invoke();
	}
}