using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public bool lockCursor, smoothFollow = true;
	public Transform target;
	
	public float
		mouseSensitivity = 5,
		dstFromTarget = 3,
		followSpeed = 5f;
		
	public Vector2 pitchMinMax = new Vector2 (-40, 85);
	
	public float rotationSmoothTime = .12f;
	
	Vector3
		rotationSmoothVelocity,
		currentRotation;
	
	float yaw, pitch;
	
	float FollowSpeed{
		get{ return followSpeed * Time.deltaTime; }
	}
	
	Transform camFollow;
	
	[SerializeField] TouchCameraController touchController = null;
	
	void Start(){
		if(lockCursor){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		camFollow = new GameObject("camFollow").transform;
	}
	
	void Update(){
		if(smoothFollow){ SmoothFollow(); }
	}
	
    void LateUpdate()
    {
        yaw += touchController.Pan.x * mouseSensitivity; // tango
		pitch -= touchController.Pan.y * mouseSensitivity; // lingon
		pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
		
		currentRotation = Vector3.SmoothDamp(
			currentRotation,
			new Vector3 (pitch, yaw),
			ref rotationSmoothVelocity,
			rotationSmoothTime
		);
		
		transform.eulerAngles = currentRotation;
		
		if(smoothFollow){ MoveTowards(camFollow.position); }
		else{ MoveTowards(target.position); }
    }
	
	void SmoothFollow(){
		var direction = target.position - camFollow.position;
		camFollow.position += direction * FollowSpeed;
	}
	
	void MoveTowards(Vector3 newPosition){
		transform.position = newPosition - transform.forward * dstFromTarget;
	}
}
