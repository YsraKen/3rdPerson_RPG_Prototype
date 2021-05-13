using UnityEngine;
using UnityEditor;

public class InventoryTest : MonoBehaviour{
	
	public HealthPotion healthPotion;
	
	public void UsePotion(){ healthPotion.Use(); }
}

[CustomEditor(typeof(InventoryTest))]
public class InventoryTestEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		InventoryTest script = (InventoryTest) target;
		
		if(GUILayout.Button("Use Potion")){ script.UsePotion(); }
	}
}