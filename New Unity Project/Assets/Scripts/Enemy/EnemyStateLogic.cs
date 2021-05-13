using UnityEngine;

public class EnemyStateLogic : MonoBehaviour
{
	public Transform target;
	
	[SerializeField] float
		stoppingDistance = 5f,
		attackingDistance = 0.65f;
	
	Vector3 direction;
	float distance;
	bool atStoppingDistance, atAttackingDistance;
	
	public bool AtAttackDistance{ get{ return atAttackingDistance; }}
	
	Animator anim;
	EnemyController controller;
	// ObstacleAvoidance obstacleAvoid;
	
	public static readonly string
		startParam = "isStarting",
		atStopDistParam = "atStopDistance",
		// atAttackDistParam = "atAttackDistance",
		approachParam = "Approach",
		attackParam = "Attack",
		wasDamagedParam = "wasDamaged"; // ref Approach StateBehaviour
	
	void Start(){
		anim = GetComponentInChildren<Animator>();
		controller = GetComponent<EnemyController>();
		// obstacleAvoid = GetComponentInChildren<ObstacleAvoidance>();
	}
	
	void Update(){
		direction = target.position - transform.position;
		distance = direction.magnitude;
		
		atStoppingDistance = distance <= stoppingDistance;
		atAttackingDistance = distance <= attackingDistance;
		
		anim.SetBool(atStopDistParam, atStoppingDistance);
		// anim.SetBool(atAttackDistParam, atAttackingDistance);
	}
	
	public void MoveTowardsTarget(){
		controller.Move();
		FaceTarget();
	}
	
	void FaceTarget(){
		var lookRotation = Quaternion.LookRotation(FaceDirection());
		var rotationSpeed = 5f * Time.deltaTime;
		
		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			lookRotation,
			rotationSpeed
		);
	}
	
	Vector3 FaceDirection(){
		return new Vector3(direction.x, 0f, direction.z);
	}
	
	public void Damage(){
		anim.SetBool(wasDamagedParam, true);
	}
}