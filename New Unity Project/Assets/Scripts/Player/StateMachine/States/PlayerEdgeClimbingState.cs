using UnityEngine;

public class PlayerEdgeClimbingState : PlayerStates
{
	[SerializeField] float edgeClimbCheckRadius = 0.25f;

	[SerializeField] private Transform edgeClimbCheck;
	[SerializeField] private LayerMask edgeClimbCheckLayers;
	
	bool isGrounded{ get{ return playerController.isGrounded; }}
	
	static readonly string
		onEdgeClimbParam = "onEdgeClimb";
	
	protected override void Start(){
		base.Start();
		
		LSS_OnEdgeClimbBehaviour.onEdgeClimbExit = OnStateExit;
	}
	
	public override void AtAnyState(){
		base.AtAnyState();
		
		EdgeClimbCheck();
	}
	
	void EdgeClimbCheck(){
		if(isGrounded){ return; }
		
		bool detectObstacle = Physics.CheckSphere(
			edgeClimbCheck.position,
			edgeClimbCheckRadius,
			edgeClimbCheckLayers
		);
		
		anim.SetTrigger(onEdgeClimbParam);
		OnStateEnter();
	}
	
	void OnDrawGizmosSelected(){
		if(edgeClimbCheck == null){ edgeClimbCheck = transform; }

		Gizmos.DrawWireSphere(
			edgeClimbCheck.position,
			edgeClimbCheckRadius
		);
	}
}