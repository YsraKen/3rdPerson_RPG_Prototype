using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] float speed = 5f;
	
	float Speed{
		get{ return speed * Time.deltaTime; }
	}
	
	void LateUpdate(){
		var targetPos = target.position;
		var direction = targetPos - transform.position;
		
		transform.position += direction * Speed;
	}
}