using UnityEngine;

public class Destructibles : MonoBehaviour, IDamageables
{
	public GameObject defaultBody;
	public GameObject[] fragments;
	
	public float despawnTime = 5f;
				
	public void SetTransforms(
		Vector3 newPostition,
		Quaternion newRotation
	){
		defaultBody.transform.position = newPostition;
		defaultBody.transform.rotation = newRotation;
	}
	
	public void TakeDamage(){
		defaultBody.SetActive(false);
		
		foreach(var f in fragments){
			f.transform.parent = this.transform;
			f.SetActive(true);
		}
		Destroy(gameObject, despawnTime);
	}
	
	public void TakeDamage(Vector3 hitPos){}
}