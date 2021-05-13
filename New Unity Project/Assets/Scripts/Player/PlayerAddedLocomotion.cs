using UnityEngine;

public class PlayerAddedLocomotion : MonoBehaviour
{
	Animator anim;
	
	static readonly string
		onStopWalkRunParam = "onStopWalkRun";
		
	void Start(){
		anim =  GetComponent<Animator>();
	}
	
	public void OnStopWalkRun(){ // to be called on Thumbstick or input release
		anim.SetTrigger(onStopWalkRunParam);
	}
}