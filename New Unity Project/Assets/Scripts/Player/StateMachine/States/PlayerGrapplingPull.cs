using UnityEngine;
using System.Collections;

public class PlayerGrapplingPull : PlayerStates
{
	public float
		triggerHoldDuration = 0.55f,
		pullForce = 50f,
		impulseForce = 1.5f;
		
	public bool impulse;	
	
	[Header("Particle Effects")]
	public ParticleSystem particleEffects;
	
	public float
		particleLengthMultiplier = 2f, // because the mesh emitter has some scale-ratio imperfections, so use this to calibrate
		particleWidthRange = 3f;
	
	[Header("Animations")]
	public string pullClipName;
	
	public PlayerGrappling grapplingScript;
	
	IEnumerator currentTriggerTimer;

	protected override void Start(){
		base.Start();
		
		grapplingScript.onStateEnter += OnPullPotentialTrigger;
	}
	
	#region potential check
	
	void OnPullPotentialTrigger(){ // to activate this state at from input-hold
		if(inputMgr.Fire1Hold()){ PotentialTriggerTimer(); }
		else{ CancelPullPotential(); }
	}
	
	void PotentialTriggerTimer(){
		if(currentTriggerTimer != null){ StopCoroutine(currentTriggerTimer); }
		currentTriggerTimer = NewTriggerTimer();
		StartCoroutine(currentTriggerTimer);
	}
	
	IEnumerator NewTriggerTimer(){
		yield return new WaitForSeconds(triggerHoldDuration);
		OnStateEnter();
	}
	
	void CancelPullPotential(){
		if(currentTriggerTimer != null){ StopCoroutine(currentTriggerTimer); }
	}
	
	#endregion
	
	public override void OnStateEnter(){
		
		grapplingScript.bypassExitEffects = true;
		
		base.OnStateEnter();
		
		var particleT = particleEffects.transform; // cache
		var direction = grapplingScript.ParticleDirection;
		
		particleT.position = grapplingScript.ParticlePosition;
		particleT.forward = direction;
		
		var sh = particleEffects.shape;
		var distance = direction.magnitude;
		
		sh.scale = new Vector3(
			particleWidthRange,
			particleWidthRange,
			distance * particleLengthMultiplier
		);
		
		particleEffects.Emit(grapplingScript.ParticleEmissionCount);
		// Debug.Log("Entered pull mode");
		
		anim.Play(pullClipName);
		
		var rb = grapplingScript.ObjectToGrappleRB;
		
		if(impulse){
			rb?.AddForceAtPosition(
				-direction * impulseForce,
				grapplingScript.grapplePoint.position,
				ForceMode.Impulse
			);
		}else{
			rb?.AddForceAtPosition(
				-direction * pullForce,
				grapplingScript.grapplePoint.position
			);
		}
		
	}
}