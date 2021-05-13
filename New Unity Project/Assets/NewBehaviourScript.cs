using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	public Vector2 massScale = new Vector2(0.1f, 10f);
	
	float newMass{
		get{ return Random.Range(massScale.x, massScale.y); }
	}
	
	Rigidbody[] rbs;
	
	void Start(){
		rbs = GetComponentsInChildren<Rigidbody>();
		foreach(var rb in rbs){ rb.mass = newMass; }
	}
}