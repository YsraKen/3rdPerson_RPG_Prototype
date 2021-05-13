using UnityEngine;

public class PlayerStates : MonoBehaviour
{
	#region initialization
	#region variables
	
	PlayerStateMachine stateMachine;
	
	protected static Animator anim{ get; private set; }
	protected static PlayerInputManager inputMgr{ get; private set; }
	protected static PlayerController playerController{ get; private set; }
	protected static Transform playerTransform{ get; private set; }
	
	public delegate void OnStateUpdateChange();
	public OnStateUpdateChange onStateEnter, onStateUpdate, onStateExit;
	
	protected bool
		isListenerBypassed,
		isStateSuccessfullyEntered,
		isStateSuccessfullyExited;
	
	#endregion
	
	protected virtual void Start(){
		stateMachine = GetComponentInParent<PlayerStateMachine>();
		anim = stateMachine.AnimRef;
		
		inputMgr = PlayerInputManager.instance;
		playerController = stateMachine.playerController;
		playerTransform = playerController.transform;
	}
	
	#endregion
	
	public virtual void OnStateUpdate(){ onStateUpdate?.Invoke(); }
	
	public virtual void AtAnyState(){ if(!isListenerBypassed){ OnAnyStateListen(); }}
	public void BypassListener(bool b){ isListenerBypassed = b; }
	
	protected virtual void OnAnyStateListen(){}
	
	public virtual void OnStateEnter(){
		onStateEnter?.Invoke();
		
		// reset
			isStateSuccessfullyEntered = false;
			isStateSuccessfullyExited = false;
		
		isStateSuccessfullyEntered = stateMachine.EnterNewState(this);
	}
	
	public virtual void OnStateExit(){
		onStateExit?.Invoke();
		
		isStateSuccessfullyExited = stateMachine.ExitCurrentState(this);
	}
	
	protected bool CompareCurrentState(PlayerStates state){
		if(stateMachine.currentState == null){ return false; }
		return stateMachine.currentState == state;
	}
}