using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Espada : MonoBehaviour
{
	void OnTriggerEnter(Collider col){
		var httr = col.GetComponent<Hitter>();
		
		if(DashStrike.isCurrentlyAttacking){ httr?.Daamge(); }
	}
}