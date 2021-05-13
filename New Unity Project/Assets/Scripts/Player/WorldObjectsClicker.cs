using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class WorldObjectsClicker :
	MonoBehaviour,
	IPointerDownHandler,
	IPointerClickHandler,
	IBeginDragHandler,
	IPointerUpHandler
{
	public float pointerDownDelay = 0.1f;
	
	#region delegates
	public delegate void OnClickerUpdateChanged(Vector2 clickPos);
	
	public static OnClickerUpdateChanged
	
		onDownDelayed,
		onLeftDownDelayed,
		onMidDownDelayed,
		onRightDownDelayed,
		
		onClick,
		onLeftClick,
		onMidClick,
		onRightClick,
		
		onUp,
		onLeftUp,
		onMidUp,
		onRightUp;
	#endregion
	
	bool isDragged;
	
	IEnumerator currentOnPointerDownDelayCoroutine;
	
	public void OnPointerDown(PointerEventData data){
		isDragged = false;
		
		if(currentOnPointerDownDelayCoroutine != null){
			StopCoroutine(currentOnPointerDownDelayCoroutine);
		}
		currentOnPointerDownDelayCoroutine = Delay(data);
		StartCoroutine(currentOnPointerDownDelayCoroutine);
	}	
	
	IEnumerator Delay(PointerEventData data){
		yield return new WaitForSeconds(pointerDownDelay);
		
		if(!isDragged){
			var pos = data.position;
			onDownDelayed?.Invoke(pos);
			// Debug.Log("down");
			
			var button = data.button;
			var isLeftClick = button == PointerEventData.InputButton.Left;
			var isMiddleClick = button == PointerEventData.InputButton.Middle;
			var isRightClick = button == PointerEventData.InputButton.Right;
			
			if(isLeftClick){ onLeftDownDelayed?.Invoke(pos); }
			else if(isMiddleClick){ onMidDownDelayed?.Invoke(pos); }
			else if(isRightClick){ onRightDownDelayed?.Invoke(pos); }
		}
	}
		
	public void OnBeginDrag(PointerEventData data){ isDragged = true; }
	
	public void OnPointerClick(PointerEventData data){
		if(isDragged){ return; }
		
		var pos = data.position;
		onClick?.Invoke(pos);
		
		var button = data.button;
		var isLeftClick = button == PointerEventData.InputButton.Left;
		var isMiddleClick = button == PointerEventData.InputButton.Middle;
		var isRightClick = button == PointerEventData.InputButton.Right;
		
		if(isLeftClick){ onLeftClick?.Invoke(pos); }
		else if(isMiddleClick){ onMidClick?.Invoke(pos); }
		else if(isRightClick){ onRightClick?.Invoke(pos); }
    }
	
	public void OnPointerUp(PointerEventData data){
		if(isDragged){ return; }
		
		var pos = data.position;
		onUp?.Invoke(pos);
		
		var button = data.button;
		var isLeftClick = button == PointerEventData.InputButton.Left;
		var isMiddleClick = button == PointerEventData.InputButton.Middle;
		var isRightClick = button == PointerEventData.InputButton.Right;
		
		if(isLeftClick){ onLeftUp?.Invoke(pos); }
		else if(isMiddleClick){ onMidUp?.Invoke(pos); }
		else if(isRightClick){ onRightUp?.Invoke(pos); }
    }
}