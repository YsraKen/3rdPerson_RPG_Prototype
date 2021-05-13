using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
	[SerializeField] float distToAvoid = 1.5f, angle = 25f;
	
	[SerializeField] private Transform
		rayPoint,
		leftSeek,
		rightSeek;
	
	float  leftSeekDistance, rightSeekDistance;
	
	public Vector3 turnDirection{ get; private set; }
	
	void Start(){
		leftSeek.localEulerAngles = Vector3.down * angle;
		rightSeek.localEulerAngles = Vector3.up * angle;
	}
	
	// to be called from chase state
	public bool IsAvoiding(){
		bool rayHit = Physics.Raycast(
			rayPoint.position,
			rayPoint.forward,
			distToAvoid
		);
		
		if(rayHit){
			turnDirection = leftSeek.forward;
		}
		
		return rayHit;
	}
}