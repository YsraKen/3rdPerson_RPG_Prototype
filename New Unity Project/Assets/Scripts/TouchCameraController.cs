using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraController :
	MonoBehaviour,
	IDragHandler,
	IEndDragHandler
{
	public static TouchCameraController instance;
	void Awake(){ instance = this; }
	
	Vector2 pan;
	
	public Vector2 Pan{
		get{ return pan; }
	}
	
	public void OnDrag(PointerEventData data){
		var sensitivity = 0.1f; // mouse axis default sensitivity settings on Unity's InputManager
		
		pan = new Vector2(
			data.delta.x * sensitivity,
			data.delta.y * sensitivity
		);
		
	}
	
	public void OnEndDrag(PointerEventData data){
		pan = Vector2.zero;
	}
}