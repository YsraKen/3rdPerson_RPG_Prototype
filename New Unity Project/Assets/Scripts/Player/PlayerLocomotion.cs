using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
	[SerializeField] private AnimationCurve transitionCurve;
	[SerializeField] float smoothTime = 0.15f;
	
	PlayerController playerController;
	Transform playerTransform;
	Animator anim;
	
	public Vector3 rootMotion{ get; private set; }	
	
	static readonly string
		directionXParam = "directionX",
		directionYParam = "directionY";
		
	void Start(){
		playerController = GetComponentInParent<PlayerController>();
		playerTransform = playerController.transform;
		
		anim = GetComponent<Animator>();
		anim.applyRootMotion = true;
	}
	
	// called from Update() via PlayerController, that's why i put it above here
	public void SetDirection(float magnitude){
		// calculate the total direction (relative and input for magnitude lerpings in animations)
		
		// var relDir = playerController.viewRelativeDirection;
		var direction3d = playerController.inputDirectionRelativeToView;
		var transition = transitionCurve.Evaluate(magnitude);
		
		var direction2d = new Vector2(
			direction3d.x * transition,
			direction3d.z * transition
		);
		
		anim.SetFloat(directionXParam, direction2d.x, smoothTime, Time.deltaTime);
		anim.SetFloat(directionYParam, direction2d.y, smoothTime, Time.deltaTime);
	}
	
	void OnAnimatorMove(){ rootMotion = anim.deltaPosition; }
	
	void LateUpdate(){
		if(!lookAround) return;
		
		AngleToDirection();
		
		if(autoDisableLookAround){ return; }
		
		for(int i = 0; i < bones.Length; i++){
			
			float currentAngle = Mathf.Lerp(
				-maxAngle[i],
				maxAngle[i],
				angleToDirectionLerp
			);
			
			bones[i].localRotation = Quaternion.Euler(Vector3.up * currentAngle);
		}
	}
	
	#region Spine
	
	[Header("Spine")]
	public bool lookAround = true;
	
	public Transform[] bones;
	public float[] maxAngle;
	
	public float angleToDisable = 30f;
	public float angleSmoothTime = 0.15f;
	
	public AnimationCurve lerpings;
	
	float
		angleLerpRaw, // must've a range 0 to 1 only
		angleToDirectionLerp, // must have a range -1 to 1 because it depends on lerpings
		angleSmoothVelocity;
	
	bool autoDisableLookAround;
	
	const float
		oneEighty = 180f,
		middleLerp = 0.5f;
	
	void AngleToDirection(){
		var angleToDirection = Vector3.SignedAngle(
			playerTransform.forward,
			playerController.inputDirectionRelativeToView,
			Vector3.up
		);
		
		var isMoving = playerController.isMoving;
		
		autoDisableLookAround =
			!isMoving ||
			Mathf.Abs(angleToDirection) <= angleToDisable;
		
		angleLerpRaw = angleToDirection / oneEighty;
		
		var lerpNotSmoothed = lerpings.Evaluate(angleLerpRaw);
		var targetLerp = (isMoving)? lerpNotSmoothed: middleLerp;
		
		angleToDirectionLerp = Mathf.SmoothDampAngle(
			angleToDirectionLerp,
			targetLerp,
			ref angleSmoothVelocity,
			angleSmoothTime
		);
	}
	
	#endregion
}