using UnityEngine;

public class Ragdoll : MonoBehaviour
{
	[SerializeField] float
		explode = 10f,
		radius = 5f;
		
	[SerializeField] private Rigidbody[] rbs;
	
	const float upward = 3.0f;
	
	public void Explode(Vector3 pos){
		
		foreach(var r in rbs){
			r.AddExplosionForce(explode, pos, radius, upward);
		}
	}
}