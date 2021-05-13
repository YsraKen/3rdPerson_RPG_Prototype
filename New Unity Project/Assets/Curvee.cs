using UnityEngine;
using UnityEditor;

public class Curvee : MonoBehaviour
{
	public AnimationClip _clip;
	// public string boneName;
	
	public string
		_posX, _posY, _posZ,
		_rotX, _rotY, _rotZ, _rotW;
	
	public AnimationCurve
		posX, posY, posZ,
		rotX, rotY, rotZ, rotW;
	
	public EditorCurveBinding[] curveBindings;
	
	public bool debugLog;
	
	public void GetCurves(){
		curveBindings = AnimationUtility.GetCurveBindings(_clip);
		
		foreach(var curveBinding in curveBindings){
			if(debugLog){
				Debug.Log(curveBinding.path + ", " + curveBinding.propertyName);
			}else{
				if(curveBinding.propertyName == _posX){ posX = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				if(curveBinding.propertyName == _posY){ posY = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				if(curveBinding.propertyName == _posZ){ posZ = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				
				if(curveBinding.propertyName == _rotX){ rotX = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				if(curveBinding.propertyName == _rotY){ rotY = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				if(curveBinding.propertyName == _rotZ){ rotZ = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
				if(curveBinding.propertyName == _rotW){ rotW = AnimationUtility.GetEditorCurve(_clip, curveBinding); }
			}
		}
	}
}

[CustomEditor(typeof(Curvee))]
public class CurveeEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		Curvee script = (Curvee) target;
		
		if(GUILayout.Button("Get Curves")){ script.GetCurves(); }
	}
}