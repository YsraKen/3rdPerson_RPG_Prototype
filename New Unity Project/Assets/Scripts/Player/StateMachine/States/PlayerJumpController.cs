using UnityEngine;

// required JSS_JumpLaunchBehaviour on Launch animation
public class PlayerJumpController : PlayerStates
{
	#region variables
	
	[SerializeField] float
		jumpHeight = 2f,
		_groundJumpPotentialExpireDuration = 0.1f;
		
	[SerializeField] int _aerialJumpPotentialCount = 2;
	
	[Header("Wall Jumping")]
	[SerializeField] float wallJumpCheckRadius = 0.25f;
	[SerializeField] private Transform wallJumpCheck;
	[SerializeField] private LayerMask environmentsLayer;
	
	[Header("Free Falling")]
	[SerializeField] float fallSmooth = 0.15f;
	[SerializeField] private AnimationCurve fallSmoothSpeed;
	
	[Space(10)]
	[SerializeField] private PlayerEvade evadeReference;
	
	float groundJumpPotentialExpireDuration;
	int aerialJumpPotentialCount;
	
	bool groundJumpPotential, wallJumpPotential;
	bool aerialJumpPotential{ get{ return aerialJumpPotentialCount > 0; }}
	
	// dst from gnd
	float maxHeightReached;
	Collider dstMeasureRefObj;
	
	bool isGrounded{ get{ return playerController.isGrounded; }}
	float velocityY{ set{ playerController.velocityY = value; }}
	float gravity{ get{ return playerController.gravity; }}
	
	public string 
		groundJumpClipName,
		aerialJumpClipName;
		
	static readonly string
		// doJumpingParam = "doJumping",
		// doAerialJumpingParam = "doAerialJumping",
		dstFromGndParam = "dstFromGnd";
	
	#endregion
	
	protected override void Start(){
		base.Start();
		
		JSS_JumpLaunchBehaviour.onLaunchEnter = OnActualLaunching;
		JSS_JumpLaunchBehaviour.onLaunchExit += OnStateExit;
	}
	
	protected override void OnAnyStateListen(){
		base.OnAnyStateListen();
		
		JumpPotentialsCheck();
		
		if(inputMgr.Jump() && !CompareCurrentState(evadeReference)){
			if(groundJumpPotential){ GroundJumpAnticipate(); }
			else if(aerialJumpPotential){ AerialJumpAnticipate(); }
		}
		
		MeasureDistanceFromGround();
	}
	
	public void ResetJumping(){
		groundJumpPotential = true;
		groundJumpPotentialExpireDuration = _groundJumpPotentialExpireDuration;
		aerialJumpPotentialCount = _aerialJumpPotentialCount;
		
		maxHeightReached = 0f;
	}
	
	public void RegainAerialJump(){ // Evade Boost, Grappling Hook etc
		aerialJumpPotentialCount = _aerialJumpPotentialCount;
	}
	
	void GroundJumpAnticipate(){
		OnStateEnter();
		
		groundJumpPotential = false;
		// anim.SetTrigger(doJumpingParam);
		anim.Play(groundJumpClipName);
	}
	
	void AerialJumpAnticipate(){
		OnStateEnter();
		
		aerialJumpPotentialCount--;
		// anim.SetTrigger(doAerialJumpingParam);
		anim.Play(aerialJumpClipName);
	}
	
	void OnActualLaunching(){
		float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
		velocityY = jumpVelocity;
	}
	
	void JumpPotentialsCheck(){
		if(isGrounded){
			// groundJumpPotential = true
			if(!groundJumpPotential){ // <<< just a gate so it won't do this every frames
				ResetJumping();
			}
		}else{
			WallJumpCheck();
			
			if(!wallJumpPotential){
				// set a timer, then groundJumpPotential = false;
				if(groundJumpPotentialExpireDuration <= 0){ groundJumpPotential = false; }
				else{ groundJumpPotentialExpireDuration -= Time.deltaTime; }
			}
		}
	}
	
	void WallJumpCheck(){
		wallJumpPotential = Physics.CheckSphere(
			wallJumpCheck.position,
			wallJumpCheckRadius,
			environmentsLayer
		);
		
		if(wallJumpPotential){ ResetJumping(); }
	}
	
	void MeasureDistanceFromGround(){
		if(isGrounded){ return; }
		
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100, environmentsLayer)){
			var currentObjectReference = hit.collider;
			var currentHeight = hit.distance;
			
			if(currentObjectReference != dstMeasureRefObj){
				dstMeasureRefObj = currentObjectReference;
				maxHeightReached = currentHeight;
			}
			if(currentHeight > maxHeightReached){ maxHeightReached = currentHeight; }
			
			var lerp = currentHeight / maxHeightReached;
			var smoothedLerp = fallSmoothSpeed.Evaluate(lerp) * fallSmooth;
			
			anim.SetFloat(
				dstFromGndParam,
				lerp,
				smoothedLerp,
				Time.deltaTime
			);
		}
	}
	
	void OnDrawGizmosSelected(){
		if(wallJumpCheck == null){ wallJumpCheck = transform; }
		
		Gizmos.DrawWireSphere(
			wallJumpCheck.position,
			wallJumpCheckRadius
		);
	}
}