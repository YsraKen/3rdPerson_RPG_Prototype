using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationEventsReceiver))]
public class AnimationEventsReceiverEditor : Editor {
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		AnimationEventsReceiver script = (AnimationEventsReceiver) target;
		
		if(GUILayout.Button("Play Test Clip")){ script.TestClipPlay(); }
	}
}