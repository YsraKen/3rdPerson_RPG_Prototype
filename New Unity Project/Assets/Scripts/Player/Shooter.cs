using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{
	[SerializeField] private GameObject projectile;
	[SerializeField] private Transform shotPoint;
	
	[SerializeField] private LayerMask environmentLayer;
	
	Camera cam;
	Inventory inventory;
	
    void Start(){
		WorldObjectsClicker.onClick += Shoot;
		cam = Camera.main;
		
		inventory = Inventory.instance;
	}
	
	void Shoot(Vector2 clickPos){
		if(inventory.GetOrb()){
			Ray ray = cam.ScreenPointToRay(clickPos);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 100, environmentLayer)){
				var newProjectile = Instantiate(
					projectile,
					shotPoint.position,
					Quaternion.identity
				);
				
				newProjectile.SetActive(true);
				newProjectile.transform.LookAt(hit.point); 
			}
		}
	}
}
