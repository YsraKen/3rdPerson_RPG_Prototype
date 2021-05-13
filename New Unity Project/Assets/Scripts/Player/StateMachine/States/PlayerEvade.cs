using UnityEngine;
using System.Collections;

public class PlayerEvade : PlayerStates
{
	[SerializeField] float
		groundEvadeSpeed = 10f,
		groundEvadeDuration = 0.5f;
		
	[SerializeField] private AnimationCurve groundEvadeSpeedOverTime;
	float currentEvadeTime;
	
	bool isGrounded{ get{ return playerController.isGrounded; }}
	bool isEvading;
	
	public string
		groundEvadeClipName,
		aerialEvadeClipName;
	
	static readonly string
		// groundEvadeParam = "groundEvade",
		// aerialEvadeParam = "aerialEvade",
		isEvadingParam = "isEvading";
	
	protected override void Start(){
		base.Start();
		
		JSS_OnEvadeBehaviour.onEvadeExit = OnStateExit;
	}
	
	protected override void OnAnyStateListen(){
		base.OnAnyStateListen();
		
		if(isEvading){ return; }
		if(inputMgr.Evade()){ OnStateEnter(); }
	}
	
	public override void OnStateUpdate(){
		base.OnStateUpdate();
		
		// if(isGrounded){ GroundEvade(); }
		// else{ AerialEvade(); }
	}
	
	public override void OnStateEnter(){
		base.OnStateEnter();
		
		if(isGrounded){ anim.Play(groundEvadeClipName); }
		else{ anim.Play(aerialEvadeClipName); }
		
		isEvading = true;
		currentEvadeTime = 0f;
		
		if(playerController.isMoving){
			playerTransform.forward = playerController.viewRelativeDirection;
		}
		
		anim.SetBool(isEvadingParam, true);
	}
	
	void GroundEvade(){
		// boost controller
		currentEvadeTime += Time.deltaTime;
		
		var currentTimeInPercent = Mathf.InverseLerp(0, groundEvadeDuration, currentEvadeTime);
		var currentSpeed = groundEvadeSpeedOverTime.Evaluate(currentTimeInPercent);
		var evadeVelocity = playerTransform.forward * currentSpeed * groundEvadeSpeed;
	
		// playerController.BypassMovement(evadeVelocity);
	}
	
	void AerialEvade(){
		// freeze controller
		// playerController.BypassVelocity(Vector3.zero);
	}
	
	public override void OnStateExit(){
		base.OnStateExit();
		
		isEvading = false;
		
		// reset controller bypass
		// playerController.UnbypassAll();
		anim.SetBool(isEvadingParam, false);
	}
}