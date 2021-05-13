using UnityEngine;

public class Enemy : MonoBehaviour, IDamageables
{
	[SerializeField] private Ragdoll ragdoll;
	
	float hp = 1f;
	
	EnemyStateLogic stateLogic;
	
	void Start(){
		stateLogic = GetComponent<EnemyStateLogic>();
	}
	
	public void TakeDamage(){}
	
	public void TakeDamage(Vector3 hitPosition){
		stateLogic.Damage();
		hp -= Random.value;
		
		if(hp <= 0){ Die(hitPosition); }
	}
	
	void Die(Vector3 hitPos){
		var newRagdoll = Instantiate(
			ragdoll,
			transform.position,
			transform.rotation
		);
		
		newRagdoll.Explode(hitPos);
		newRagdoll.gameObject.SetActive(true);
		
		Destroy(newRagdoll.gameObject, 5f);
		Destroy(gameObject, 5f);
		
		gameObject.SetActive(false);
		
	}
}