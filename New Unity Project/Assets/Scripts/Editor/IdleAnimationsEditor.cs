using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IdleAnimations))]
public class IdleAnimationsEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		IdleAnimations script = (IdleAnimations) target;
		
		if(GUILayout.Button("Randomize")){}
	}
}