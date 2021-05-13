using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
	#region initialization
	#region variables
	
	Vector2 clickPosition;
	
	protected Vector3 faceDirection;
	protected float angle;
	
	static PlayerCombatStateMachine stateMachine;
	
	protected static Camera cam{ get; private set; }
	protected static LayerMask targetsCheckLayer{ get; private set; }
	
	protected static Animator anim{ get; private set; }
	protected static PlayerController playerController{ get; private set; }
	
	protected static Transform playerTransform{ get{ return playerController.transform; }}
	
	protected static bool isGrounded{ get{ return playerController.isGrounded; }}

	public delegate void OnCombatStateUpdateChanged();
	public OnCombatStateUpdateChanged onCombatStateExit;
	
	#endregion
	
	protected virtual void Start(){
		stateMachine = GetComponentInParent<PlayerCombatStateMachine>();
		anim = stateMachine.GetAnimator();
		
		playerController = stateMachine.GetController();
		
		cam = Camera.main;
		targetsCheckLayer = stateMachine.GetTargetsCheckLayer();
	}
	
	#endregion
	
	public virtual void Attack(Vector2 clickPos){
		// Target based on mouse click at screen
		// if there's no available target, don't rotate but still play the attack animation
		
		clickPosition = clickPos;
		OnAttacking();
	}
	
	protected virtual void OnAttacking(){
		Ray ray = cam.ScreenPointToRay(clickPosition);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100, targetsCheckLayer)){
			IDamageables damageable = hit.collider.GetComponent<IDamageables>();
			
			var targetPos = (damageable == null)? hit.point: hit.transform.position;
			var playerPos = playerTransform.position;
			
			targetPos.y = playerPos.y;
			faceDirection = targetPos - playerPos;
			
			angle = Vector3.Angle(faceDirection, playerTransform.forward);
			
			playerTransform.forward = faceDirection;
		}
	}
	
	protected virtual void OnCombatStateExit(){ onCombatStateExit?.Invoke(); }
}