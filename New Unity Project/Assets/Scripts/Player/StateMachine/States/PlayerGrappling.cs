using UnityEngine;

public class PlayerGrappling : PlayerStates
{
	#region variables
	
	public bool enableGrapplingMode;
	public float knockBack = 10f;
	
	[Header("Raycast")]
	public LayerMask grappleablesMask;
	public Transform rayPoint, grapplePoint;
	
	[Header("SpringJoint")]
	public SpringJoint joint;
	public Rigidbody playerRb;
	
	public float
		maxDistance = 0.25f,
		minDistance = 0.1f;
		
	[Header("Rope")]
	public GameObject ropeObject;
	public Transform ropeA, ropeB;
	
	[Header("Ragdoll")]
	public bool useRagdoll;
	
	public GameObject staticModel, ragdollModel;
	public Transform staticHand, ragdollHand;
	
	Transform hand{
		get{
			return (useRagdoll)? ragdollHand: staticHand;
		}
	}
	
	[Header("Particles")]
	public ParticleSystem ropeParticle;
	public int particleAmount = 1;
	
	[Header("Animations")]
	public string throwClipName;
	public string exitClipName;
	
	[Header("Scripts")]
	public PlayerCombatStateMachine combatStateScript;
	public PlayerJumpController jumpController;
	
	Camera cam;
	
	Vector3 targetDiretion;
	Transform objToGrapple;
	Rigidbody objToGrappleRB;
	
	bool isCurrentlyGrappling;
	
	#region outher script references
	
		public int ParticleEmissionCount{ get; private set; }
		public Vector3 ParticlePosition{ get; private set; }
		public Vector3 ParticleDirection{ get; private set; }
		
		[HideInInspector] public bool bypassExitEffects;
		
		public Rigidbody ObjectToGrappleRB{ get{ return objToGrappleRB; }}
		
	#endregion
	#endregion
	
	#region PlayerStates
	
	protected override void Start(){
		base.Start();
		
		cam = Camera.main;
		joint.autoConfigureConnectedAnchor = false;
		
		WorldObjectsClicker.onLeftDownDelayed += InitializeRayDirection;
		// WorldObjectsClicker.onLeftClick += InitializeRayDirection;
		// JSS_OnGrapplingBehaviour.onAnticipationExit += OnGrappling; // to animation-event receiver
	}
	
	protected override void OnAnyStateListen(){
		base.OnAnyStateListen();
		
		if(inputMgr.EnableGrapplingMode()){
			enableGrapplingMode = !enableGrapplingMode;
			
			if(enableGrapplingMode){ combatStateScript.BypassListener(true); }
			else{ combatStateScript.BypassListener(false); }
		}
		
		if(isCurrentlyGrappling){
			RopePositioning();
		}
	}
	
	public override void OnStateEnter(){
		base.OnStateEnter();
		bypassExitEffects = false;
	}
	
	public override void OnStateUpdate(){
		base.OnStateUpdate();
		if(inputMgr.Fire2()){ OnStateExit(); }
	}
	
	public override void OnStateExit(){
		if(!isCurrentlyGrappling){ return; }
		base.OnStateExit();
		
		ResetSpringJoint();
		
		objToGrapple = null;
		// objToGrappleRB = null;
		grapplePoint.parent = null;
		
		isCurrentlyGrappling = false;
		enableGrapplingMode = false;
		
		ropeObject.SetActive(false);
		combatStateScript.BypassListener(false);
		
		if(useRagdoll){
			staticModel.SetActive(true);
			ragdollModel.SetActive(false);
		}
		
		playerController.velocityY = 0f;
		jumpController.RegainAerialJump();
		
		EnableStaticController(true);
		
		if(!bypassExitEffects){
			anim.Play(exitClipName);
			ParticleEffects(ropeB.position);
		}
	}
	
	#endregion
	
	#region Raycasts
	
	void InitializeRayDirection(Vector2 clickPos){
		if(!enableGrapplingMode){ return; }
		
		OnStateExit();
		
		Ray ray = cam.ScreenPointToRay(clickPos);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100, grappleablesMask)){
			var hitPoint = hit.point;
			
			RaycastFromCharacterPosition(hitPoint);
			FaceTowardsDirection(hitPoint);
			ParticleEffects(hitPoint);
		}
		anim.Play(throwClipName);
	}
	
	void RaycastFromCharacterPosition(Vector3 hitPoint){
		targetDiretion = hitPoint - rayPoint.position;
		
		Ray ray = new Ray(rayPoint.position, targetDiretion);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100, grappleablesMask)){
			objToGrapple = hit.transform;
			objToGrappleRB = hit.rigidbody;
			
			var localOffset = hit.point - objToGrapple.position;
			grapplePoint.position = objToGrapple.position + localOffset;
			grapplePoint.parent = objToGrapple;
			
			if(objToGrappleRB != null){ KnockBack(targetDiretion); }
			
			isCurrentlyGrappling = true;
			enableGrapplingMode = false;
			
			RopePositioning();
			ropeObject.SetActive(true);
			
			OnStateEnter(); // Success!
		}
	}
	
	#endregion
	
	#region Visuals
	
	void FaceTowardsDirection(Vector3 hitPoint){
		var playerPos = playerTransform.position;
		var relativeDir = new Vector3(hitPoint.x, playerPos.y, hitPoint.z) - playerPos;
		
		playerTransform.forward = relativeDir.normalized;
	}
	
	void ParticleEffects(Vector3 targetPoint){
		// cacheing
		var handPosition = hand.position;
		var particleTransform = ropeParticle.transform;
		
		ParticlePosition = Vector3.Lerp(targetPoint, handPosition, 0.5f);
		ParticleDirection = targetPoint - handPosition;
		
		particleTransform.position = ParticlePosition;
		particleTransform.forward = ParticleDirection;
		
		var sh = ropeParticle.shape;
		var distance = ParticleDirection.magnitude;
		
		sh.radius = distance;
		
		ParticleEmissionCount = Mathf.RoundToInt(distance) * particleAmount;
		ropeParticle.Emit(ParticleEmissionCount);
	}
	
	void KnockBack(Vector3 direction){
		objToGrappleRB.AddForceAtPosition(
			direction.normalized * knockBack,
			grapplePoint.position,
			ForceMode.Impulse
		);
	}
	
	void RopePositioning(){
		if(objToGrapple == null){ return; }
		
		var grapplePos = grapplePoint.position;
		
		ropeA.position = hand.position;
		ropeB.position = grapplePos;
		
		if(objToGrappleRB == null){
			joint.connectedAnchor = grapplePos;
		}
	}
	
	#endregion
	
	#region SpringJoint
	
	void SetupSpringJoint(){
		if(objToGrappleRB != null){
			joint.connectedBody = objToGrappleRB;
			joint.connectedAnchor = objToGrapple.InverseTransformPoint(grapplePoint.position);
		}
		
		float distance = targetDiretion.magnitude;
		
		joint.maxDistance = distance * maxDistance;
		joint.minDistance = distance * minDistance;
	}
	
	void ResetSpringJoint(){
		joint.connectedBody = null;
		joint.connectedAnchor = Vector3.zero;
	}
	
	#endregion
	
	#region PlayerController
	
	public void OnGrappling(){
		// if(hasCalledAlreadyFromAnimationClip){ return; } // to avoid double callback, because i put it in both Animation event and Animation Exit state.
		if(!isCurrentlyGrappling){ return; }
		
		SetupSpringJoint();
		
		if(targetDiretion.y <= playerTransform.position.y){
			// freeze rotation
		}
		
		if(useRagdoll){
			staticModel.SetActive(false);
			ragdollModel.SetActive(true);
		}
		
		EnableStaticController(false);
	}
	
	void EnableStaticController(bool enable){
		playerController.enabled = enable;
		playerController.controller.enabled = enable;
		
		playerRb.isKinematic = enable;
	}
	
	#endregion
}