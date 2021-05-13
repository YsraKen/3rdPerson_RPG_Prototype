using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthAndEnergy))]
public class HealthAndEnergyEditor : Editor
{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		HealthAndEnergy script = (HealthAndEnergy) target;
		
		GUILayout.BeginHorizontal();
			if(GUILayout.Button("Damage")){ script.DamageTest(); }
			if(GUILayout.Button("DeEnergize")){ script.DeEnergizeTest(); }
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			if(GUILayout.Button("Heal")){ script.HealTest(); }
			if(GUILayout.Button("Energize")){ script.EnergizeTest(); }
		GUILayout.EndHorizontal();
		
		if(GUILayout.Button("Reset")){ script.Reset(); }
	}
}