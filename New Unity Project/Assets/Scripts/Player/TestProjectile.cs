using UnityEngine;
using System.Collections;

public class TestProjectile : MonoBehaviour
{
	[SerializeField] float
		speed = 5f,
		lifeTime = 2f,
		areaOfEffect = 1.5f;
	
	[SerializeField] private GameObject explosionParticle;
	
	void Start(){ StartCoroutine(LifeTime()); }
	
	void Update(){
		transform.position += transform.forward * speed * Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other){
		AreaOfEffect();
		
		var newExplode = Instantiate(
			explosionParticle,
			transform.position,
			Quaternion.identity
		);
		
		newExplode.SetActive(true);
		this.gameObject.SetActive(false);
		
		Destroy(newExplode, 5);
		Destroy(gameObject, 5);
	}
	
	void AreaOfEffect(){
		Collider[] cols = Physics.OverlapSphere(
			transform.position,
			areaOfEffect
		);
		
		foreach(var c in cols){
			var destructible = c.GetComponentInParent<IDamageables>();
			
			destructible?.TakeDamage();
			destructible?.TakeDamage(transform.position);
		}
	}
	
	IEnumerator LifeTime(){
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}