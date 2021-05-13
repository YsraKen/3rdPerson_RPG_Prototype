using UnityEngine;
using System.Collections;

// Fast runner can be upgradable as long as there's atleast one level of evade boost upgraded
public class PlayerFastRunner : PlayerStates
{
	#region variables
	
	[SerializeField] float
		turboSpeed = 10f,
		_turboDuration = 3f,
		fastRunPotentialExpireDuration = 0.25f;
		
	float turboDuration;
	
	bool
		fastRunPotential,
		isCurrentlyRunning;
	
	IEnumerator
		currentFastRunPotentialTimer,
		currentDurationCountdown;
	
	[SerializeField] private PlayerEvadeBoost evadeBoostScript;
	
	bool isMoving{ get{ return playerController.isMoving; }}
	bool isGrounded{ get{ return playerController.isGrounded; }}
	
	static readonly string
		fastRunningParam = "fastRunning";
		
	#endregion
	
	protected override void Start(){
		base.Start();
		
		evadeBoostScript.onBoostEnded += FastRunPotentialTimer;
	}
	
	#region Potential Timer
	
	void FastRunPotentialTimer(){
		StopCurrentFastRunPotentialTimer();
		
		currentFastRunPotentialTimer = FastRunPotentialTimerLogic();
		StartCoroutine(currentFastRunPotentialTimer);
	}
	
	void StopCurrentFastRunPotentialTimer(){
		if(currentFastRunPotentialTimer != null){
			StopCoroutine(currentFastRunPotentialTimer);
		}
	}
	
	IEnumerator FastRunPotentialTimerLogic(){
		// reset first
		fastRunPotential = true;
		turboDuration = _turboDuration;
		
		yield return new WaitForSeconds(fastRunPotentialExpireDuration);
		OnStateExit(); // Exit state! wether or not we entered this state
	}
	
	#endregion

	#region PlayerStates
	
	protected override void OnAnyStateListen(){
		base.OnAnyStateListen();
		
		if(
			fastRunPotential &&
			isMoving &&
			isGrounded &&
			!isCurrentlyRunning
		){
			OnStateEnter();
			isCurrentlyRunning = true;
		}
	}
	
	public override void OnStateEnter(){
		base.OnStateEnter();
		
		// playerController.BypassRotation();
		anim.SetBool(fastRunningParam, true);
	}
	
	public override void OnStateUpdate(){
		base.OnStateUpdate();
		
		if(!isMoving || !isGrounded){ OnStateExit(); }
		
		turboDuration -= Time.deltaTime;
		if(turboDuration <= 0){ OnStateExit(); }
		
		playerTransform.forward = playerController.viewRelativeDirection;
		// playerController.BypassMovement(playerTransform.forward * turboSpeed);
	}
	
	public override void OnStateExit(){
		base.OnStateExit();
		
		fastRunPotential = false;
		isCurrentlyRunning = false;
		
		// playerController.UnbypassAll();
		anim.SetBool(fastRunningParam, false);
	}
	
	#endregion
}