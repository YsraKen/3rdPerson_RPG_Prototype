using UnityEngine;
using UnityEditor;

public class AnimPlayTest : MonoBehaviour
{
	public int maxClipIndex;
	
	Animator anim;
	
	void Start(){
		anim = GetComponent<Animator>();
	}
	
	public void Play(){
		anim.SetTrigger("newClip");
		anim.SetInteger("clipIndex", Random.Range(0, maxClipIndex));
	}
}

[CustomEditor(typeof(AnimPlayTest))]
public class AnimPlayTestEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		AnimPlayTest script = (AnimPlayTest) target;
		
		if(GUILayout.Button("Play Random")){ script.Play(); }
	}
}