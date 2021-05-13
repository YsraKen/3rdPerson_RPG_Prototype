using UnityEngine;

public class BudogRootMotion : MonoBehaviour
{
	public Thumbstick thumb;
	
	Vector3 rootMotion;
	
	Animator anim;
	Transform parent;
	CharacterController controller;
	
	static readonly string
		xParam = "x",
		zParam = "z";
	
	void Start(){
		anim = GetComponent<Animator>();
		parent = transform.parent;
		controller = GetComponentInParent<CharacterController>();
		
		anim.applyRootMotion = true;
	}
	
	void Update(){
		anim.SetFloat(xParam, thumb.Value().x);
		anim.SetFloat(zParam, thumb.Value().y);
		
		controller.Move(rootMotion);
	}
	
	void OnAnimatorMove(){
		rootMotion = anim.deltaPosition;
	}
}