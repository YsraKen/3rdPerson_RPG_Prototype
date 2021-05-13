using UnityEngine;

public class DynamicAnimator : MonoBehaviour
{
	public Transform[]
		targetBones,
		myBones;
	
	[Range(0,1)]
	public float weight;
	
	void Update(){
		for(int i = 0; i < myBones.Length; i++){
			var bone = myBones[i];
			var targetRotation = targetBones[i].rotation;
			
			bone.rotation = Quaternion.Lerp(
				bone.rotation,
				targetRotation,
				weight
			);
		}
	}
}