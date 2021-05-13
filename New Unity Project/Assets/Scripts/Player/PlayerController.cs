using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{	
	public float
		moveSpeedDebug = 5f,
		turnSmoothTime = 1f,
		gravityScale = -10f,
		jumpHeight = 5f;
	
	[Range(0,1)] public float airControlPercent = 0.5f;
	
	public bool lockRotation;
	
	public bool rootMotion;
	public Vector3 unrootedMotion;
	
	Vector2 inputDir;
	float turnSmoothVelocity;
	
	PlayerInputManager inputMgr;
	Animator animator;
	PlayerLocomotion locomotion;
	Transform camT;
	
	const float normalized = 1f;
	
	static readonly string
		isGroundedParam = "isGrounded";
		
	#region Other Scripts References
	
		public Vector3 viewRelativeDirectionRaw{ get; private set; }
		public Vector3 viewRelativeDirection{ get; private set; }
		
		public Vector3 inputDirectionRelativeToViewRaw{ get; private set; }
		public Vector3 inputDirectionRelativeToView{ get; private set; }
		
		public bool isMoving{ get; private set; }
		public bool isGrounded{ get; private set; }
		
		public float gravity{ get{ return gravityScale; }}
		[HideInInspector] public float velocityY;
	
		public CharacterController controller{ get; private set; }
		
	#endregion
	
	void Start(){
		inputMgr = PlayerInputManager.instance;
		controller = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
		locomotion = GetComponentInChildren<PlayerLocomotion>();
		camT = Camera.main.transform;
		
		ApplyRootMotion();
		JSS_LocomotionBehaviour.onLocomotionEnter = ApplyRootMotion;
	}
	
	void Update(){
		var input = inputMgr.Move();
		inputDir = Vector2.ClampMagnitude(input, normalized);
		
		Movement();
		RotateTowards();
		
		GeneralParameters(); // gravity, ground check, airbourne
		Animations();
	}
	
	void Movement(){
		locomotion.SetDirection(inputDir.magnitude);
		
		var deltaTime = Time.deltaTime;
		
		velocityY += gravity * deltaTime;
		var gravityVelocity = Vector3.up * velocityY;
		
		var moveVelocity = (rootMotion)?
			locomotion.rootMotion: // delta already!
			unrootedMotion; // not delta!
		
		var deltaVelocity = (rootMotion)?
			moveVelocity + (gravityVelocity * deltaTime):
			(moveVelocity + gravityVelocity) * deltaTime;
		
		controller.Move(deltaVelocity);
	}
	
	void RotateTowards(){
		if(lockRotation) return;
		if(!isMoving) return;
		
		float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camT.eulerAngles.y;
		
		transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
			transform.eulerAngles.y,
			targetRotation,
			ref turnSmoothVelocity,
			GetModifiedSmoothTime(turnSmoothTime)
		);
	}
	
	float GetModifiedSmoothTime(float smoothTime) {
		if(isGrounded){ return smoothTime; }
		if(airControlPercent == 0){ return float.MaxValue; }
		return smoothTime / airControlPercent;
	}
	
	#region GeneralParameters
	
	void GeneralParameters(){
		isMoving = inputDir != Vector2.zero;
		isGrounded = controller.isGrounded;
		
		if(isGrounded){ velocityY = 0f; }
		else{ rootMotion = false; } // to allow the airbourne-controlling
		
		viewRelativeDirection = ViewRelativeDirection();
		inputDirectionRelativeToView = InputDirectionRelativeToView();
		
		if(Input.GetButtonDown("Jump")){
			float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}
	
	Vector3 ViewRelativeDirection(){
		viewRelativeDirectionRaw = transform.TransformDirection(camT.forward);
		
		var relativeDir = viewRelativeDirectionRaw;
			relativeDir.y = 0f;
			relativeDir.Normalize();
		
		return relativeDir;
	}
	
	Vector3 InputDirectionRelativeToView(){
		if(!isMoving){ return Vector3.zero; }
		
		/* SCENARIO
			// the Player is Facing Vector3.forward, and also the camera
				// therefore the viewRelativeDirection (0, n, 1) aka transform.forward
				
				// if input direction == back
					// then the return is (0, n, -1) aka transform.back [or -transform.forward]
				// if input direction = left
					// then the return is (-1, n, 0) aka transform.left [or -transform.right]
					
			// the player is facing Vector3.right, the camera is facing Vector3.forward
				// therefore the viewRelativeDirection = (-1, n, 0) aka transform.left [or -transform.right]
				
				// if input direction == back
					// then the return is (1, n, 0) aka transform.right
				// if input direction == left
					// then the return is (0, n, -1) aka transform.back [or -transform.forward]
		*/
		
		var inputDir_3d = new Vector3(inputDir.x, 0, inputDir.y);
		var relativeToCameraSpace = camT.TransformDirection(inputDir_3d); // localize the input in camera space
		var relativeToPlayerSpace = transform.InverseTransformDirection(relativeToCameraSpace); // localize the direction to player space
		
		inputDirectionRelativeToViewRaw = relativeToPlayerSpace; 
		
		var inputRelDir = inputDirectionRelativeToViewRaw;
			inputRelDir.y = 0f;
			inputRelDir.Normalize();
		
		return inputRelDir;
	}
	
	#endregion
	
	void Animations(){
		animator.SetBool(isGroundedParam, isGrounded);
	}
	
	#region Other Scripts Callbacks
	
	public void ApplyRootMotion(){ rootMotion = true; }
	
	// <summary> non-delta value only </summary>
	public void BypassMovement(Vector3 newMovement){
		rootMotion = false;
		unrootedMotion = newMovement;
	}
	
	public void BypassMovement(bool bypass){
		rootMotion = !bypass;
		unrootedMotion = Vector3.zero; // idk abt this, it shouldnt be here
	}
	
	#endregion
}