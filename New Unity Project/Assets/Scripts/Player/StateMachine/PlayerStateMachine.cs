using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
	#region variables
	
	[SerializeField] private Animator animatorReference;
	[SerializeField] private PlayerStates[] states;
	
	public PlayerStates currentState;
	
	#region playerReferences

		public Animator AnimRef{ get{ return animatorReference; }}
		public PlayerController playerController{ get{ return GetComponentInParent<PlayerController>(); }}
		
	#endregion
	#endregion
	
	public bool EnterNewState(PlayerStates newState){
		if(currentState == newState){ return false; }
		else{
			currentState?.OnStateExit();
			currentState = newState;
			
			return true;
		}
	}
	
	public bool ExitCurrentState(PlayerStates state){
		if(currentState == state){
			currentState = null;
			return true;
		}
		return false;
	}
	
	void Update(){
		if(currentState != null){
			currentState.OnStateUpdate();
		}
		
		foreach(var s in states){ s.AtAnyState(); }
	}
}