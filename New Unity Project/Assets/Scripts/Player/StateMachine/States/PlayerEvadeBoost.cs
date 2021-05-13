using UnityEngine;
using System.Collections;

public class PlayerEvadeBoost : PlayerStates
{
	[SerializeField] float 
		boostSpeed = 50f,
		boostPotentialExpireDuration = 0.5f,
		aerialUpwardsVelocityBonus = 5f;

	[SerializeField] int _boostCount = 2;
	
	[Header("Particle Effects")]
	[SerializeField] int particleCounts = 50;
	[SerializeField] float positionOffset = 0.5f;
	
	[SerializeField] private ParticleSystem
		exitParticles,
		enterParticles;
	
	[Header("Other Script References")]
	[SerializeField] private PlayerEvade evadeScript;
	
	int boostCount;
	bool boostPotential;
	
	IEnumerator currentBoostPotentialTimer;
	
	bool isGrounded{ get{ return playerController.isGrounded; }}
	
	public delegate void OnBoostUpdateChanged();
	public OnBoostUpdateChanged onBoostEnded;
	
	[Header("Animations")]
	public string groundBoostClipName;
	public string aerialBoostClipName;
		
	// static readonly string
		// evadeBoostParam = "evadeBoost";
		
	protected override void Start(){
		base.Start();
		
		evadeScript.onStateExit += ResetBoost;
		evadeScript.onStateExit += BoostPotentialTimer;
		
		JSS_OnEvadeBoostBehaviour.onBoostExit = OnStateExit;
	}
	
	protected override void OnAnyStateListen(){
		base.OnAnyStateListen();
		
		if(
			boostPotential &&
			inputMgr.EvadeBoost()
		){
			OnStateEnter();
		}
	}
	
	void ResetBoost(){
		boostCount = _boostCount;
		boostPotential = true;
		
		evadeScript.BypassListener(true);
	}
	
	#region Potential Timer
	
	void BoostPotentialTimer(){
		StopCurrentBoostPotentialTimer();
		
		currentBoostPotentialTimer = BoostPotentialTimerLogic();
		StartCoroutine(currentBoostPotentialTimer);
	}
	
	void StopCurrentBoostPotentialTimer(){
		if(currentBoostPotentialTimer != null){
			StopCoroutine(currentBoostPotentialTimer);
		}
	}
	
	IEnumerator BoostPotentialTimerLogic(){
		yield return new WaitForSeconds(boostPotentialExpireDuration);
		OnStateExit(); // Exit state! wether or not we entered this state
	}
	
	#endregion

	public override void OnStateEnter(){
		base.OnStateEnter();
		
		Boost();
		boostCount --;
		
		if(boostCount <= 0){
			onBoostEnded?.Invoke(); // fast runner, aerial jump regain. 
			OnStateExit();
			
			return;
		}
		
		// if boostCount is not zero
		BoostPotentialTimer(); // let's time it
	}
	
	void Boost(){
		if(playerController.isMoving){
			playerTransform.forward = playerController.viewRelativeDirectionRaw;
		}
		
		if(isGrounded){ GroundBoost(); }
		else{ AerialBoost(); }
		
		EvadeParticleEffects();
		// anim.SetTrigger(evadeBoostParam);
	}
	
	void GroundBoost(){
		// playerController.BypassMovement(playerTransform.forward * boostSpeed + Vector3.down);
		anim.Play(groundBoostClipName);
	}
	
	void AerialBoost(){
		// playerController.BypassMovement(playerTransform.forward * boostSpeed);
		playerController.velocityY = aerialUpwardsVelocityBonus;
		
		anim.Play(aerialBoostClipName);
	}
	
	void EvadeParticleEffects(){
		var offset = Vector3.up * positionOffset;
		
		StartCoroutine(Logic());
		
		IEnumerator Logic(){
			exitParticles.transform.position = playerTransform.position + offset;
			exitParticles.Emit(particleCounts);
			
			yield return 0;
			
			enterParticles.transform.position = playerTransform.position + offset;
			enterParticles.Emit(particleCounts);
		}
	}
	
	public override void OnStateExit(){
		base.OnStateExit();
		
		boostPotential = false;
		evadeScript.BypassListener(false);
		
		// playerController.UnbypassAll();
	}
}