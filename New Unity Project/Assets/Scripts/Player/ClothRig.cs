using UnityEngine;

public class ClothRig : MonoBehaviour
{
	[System.Serializable] public class Bone{
		public Transform reference, target;
		
		public void Sync(){
			reference.position = target.position;
			reference.rotation = target.rotation;
		}
	}
	
	[SerializeField] private Bone[] target;
	
	void Update(){
		foreach(var t in target){ t.Sync(); }
	}
}