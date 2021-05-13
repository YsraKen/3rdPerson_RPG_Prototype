using UnityEngine;

public class TestSharedTip : MonoBehaviour
{
	Animator anim;
	
	public string clipName;
	
	void Start(){
		anim = GetComponent<Animator>();
	}
	
	void Update(){
		if(Input.GetButtonDown("Jump")){
			anim.Play(clipName);
		}
	}
}