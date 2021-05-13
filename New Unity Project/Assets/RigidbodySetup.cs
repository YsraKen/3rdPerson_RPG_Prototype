using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodySetup : MonoBehaviour
{
	public Vector3 movement, rotation;
	
	Rigidbody rb;
	
	void Start(){
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		rb.MovePosition(movement * Time.fixedDeltaTime);
		rb.MoveRotation(Quaternion.Euler(rotation * Time.fixedDeltaTime));
	}
}