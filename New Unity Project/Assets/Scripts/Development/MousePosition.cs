using UnityEngine;
using UnityEditor;

public class MousePosition : MonoBehaviour
{
	public Vector2 bektor, normalizedBektor;
	
	public void Calculate(){
		normalizedBektor = Vector2.ClampMagnitude(bektor, 1f);
	}
}

[CustomEditor(typeof(MousePosition))]
public class MousePositionEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		MousePosition script = (MousePosition) target;
		
		if(GUILayout.Button("Calculate")){ script.Calculate(); }
	}
}