using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStateLogic))]
public class EnemyStateLogicEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		
		EnemyStateLogic script = (EnemyStateLogic) target;
		
		if(GUILayout.Button("Damage")){ script.Damage(); }
	}
}