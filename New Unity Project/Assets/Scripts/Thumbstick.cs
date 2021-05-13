using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Collections;

public class Thumbstick :
	MonoBehaviour,
	IDragHandler,
	IPointerUpHandler,
	IPointerDownHandler
{
	public bool autoReset = true;
	
	public Text magnitude;
	
	[SerializeField] Image
		threshold = null,
		touch = null;
	
	public UnityEvent
		onJoystickRelease = new UnityEvent(); // for stop walk/run animation on player character
		
	Vector2 inputVector;
	
	static readonly string
		horizontal = "Horizontal",
		vertical = "Vertical";

	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos;
		
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
			threshold.rectTransform,
			ped.position,
			ped.pressEventCamera,
			out pos
		)){
			pos.x = (pos.x / threshold.rectTransform.sizeDelta.x);
			pos.y = (pos.y / threshold.rectTransform.sizeDelta.y);
			
			inputVector = new Vector2(pos.x * 2 + 1, pos.y * 2 - 1);
			
			inputVector = (inputVector.magnitude > 1.0f)?
				inputVector.normalized:
				inputVector;
			
			
			touch.rectTransform.anchoredPosition = 
				new Vector2(inputVector.x * (threshold.rectTransform.sizeDelta.x / 3),
							inputVector.y * (threshold.rectTransform.sizeDelta.y / 3));
		} 
	}
	
	public virtual void OnPointerDown(PointerEventData ped){ OnDrag(ped); }
	
	public virtual void OnPointerUp(PointerEventData ped){
		if(autoReset){
			inputVector = Vector3.zero;
			touch.rectTransform.anchoredPosition = Vector3.zero;
		}
		
		onJoystickRelease?.Invoke();
	}
	
	public Vector2 Value(){
		var output = new Vector2(
			(inputVector.x != 0)? inputVector.x: Input.GetAxis(horizontal),
			(inputVector.y != 0)? inputVector.y: Input.GetAxis(vertical)
		);
		
		var magni = Mathf.Round(output.normalized.magnitude * 100f);
		magnitude.text = magni.ToString();
		
		return output;
	}
}