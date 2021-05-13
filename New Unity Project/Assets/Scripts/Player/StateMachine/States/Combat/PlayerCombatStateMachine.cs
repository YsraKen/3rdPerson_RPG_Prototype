using UnityEngine;

public class PlayerCombatStateMachine : PlayerStates
{
	#region variables
	
	[SerializeField] private LayerMask targetsCheckLayer; // environment, entities, IDamageables
	
	public PlayerCombat currentCombat;
	
	public Animator GetAnimator(){ return anim; }
	public PlayerController GetController(){ return playerController; }
	
	public LayerMask GetTargetsCheckLayer(){ return targetsCheckLayer; }
	
	#endregion
	
	protected override void Start(){
		base.Start();
		WorldObjectsClicker.onDownDelayed += OnClickListener;
		// WorldObjectsClicker.onClick += OnClickListener;
	}
	
	void OnClickListener(Vector2 clickPos){
		if(isListenerBypassed){ return; }
		
		currentCombat.Attack(clickPos);
		currentCombat.onCombatStateExit = OnStateExit;
		
		OnStateEnter();
	}
	
	public override void OnStateExit(){
		base.OnStateExit();
		
		currentCombat.onCombatStateExit -= OnStateExit;
	}
}