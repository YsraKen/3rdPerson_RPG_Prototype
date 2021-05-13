using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
	[SerializeField] float
		moveSpeed = 3f,
		moveSmoothness = 0.2f,
		gravity = -10;
		
	float velocityY, currentSpeed, currentSpeedSmoothRef;
	bool isGrounded;
	
	CharacterController controller;
	
	void Start(){
		controller = GetComponent<CharacterController>();
	}
	
	void Update(){
		velocityY += Time.deltaTime * gravity;
		controller.Move(Vector3.up * velocityY * Time.deltaTime);
		
		isGrounded = controller.isGrounded;
		if(isGrounded){ velocityY = 0; }
	}
	
	public void Move(){
		currentSpeed = Mathf.SmoothDamp(
			currentSpeed,
			moveSpeed,
			ref currentSpeedSmoothRef,
			moveSmoothness
		);
		
		var velocity = transform.forward * currentSpeed;
		var newVelocity = new Vector3(velocity.x, controller.velocity.y, velocity.z);
		
		controller.Move (newVelocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
	}
}