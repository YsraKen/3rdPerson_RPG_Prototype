using UnityEngine;
using System.Collections;

public class Orb : Interactable
{
	[SerializeField] private Item reference;
	[SerializeField] private GameObject onGetOrbEffect;
	
	[SerializeField] float despawnTime = 30f;
	
	Inventory inventory;
	
	static readonly string player = "Player";
	
	void Start(){
		inventory = Inventory.instance;
		StartCoroutine(DespawnTimer());
	}
	
	void OnTriggerEnter(Collider other){
		if(other.CompareTag(player)){
			Interact();
		}
	}
	
	public override void Interact(){
		base.Interact();
		
		inventory.Add(reference);
		
		Despawn();
	}
	
	void Despawn(){
		var newEffect = Instantiate(
			onGetOrbEffect,
			transform.position,
			Quaternion.identity
		);
		
		newEffect.SetActive(true);
		this.gameObject.SetActive(false);
		
		Destroy(newEffect, 5);
		Destroy(gameObject, 5);
	}
	
	IEnumerator DespawnTimer(){
		yield return new WaitForSeconds(despawnTime);
		
		Despawn();
	}
}