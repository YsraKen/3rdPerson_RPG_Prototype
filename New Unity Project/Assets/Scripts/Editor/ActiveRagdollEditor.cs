using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActiveRagdoll))]
public class ActiveRagdollEditor : Editor
{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		ActiveRagdoll script = (ActiveRagdoll) target;
		
		if(GUILayout.Button("Setup")){ script.Setup(); }
		if(GUILayout.Button("Update Settings")){ script.UpdateSettings(); }
		
		GUILayout.BeginHorizontal();
			if(GUILayout.Button("Add Colliders")){ script.AddColliders(); }
			if(GUILayout.Button("Delete Colliders")){ script.DeleteColliders(); }
		GUILayout.EndHorizontal();	
	}
}