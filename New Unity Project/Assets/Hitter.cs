using UnityEngine;

public class Hitter : MonoBehaviour
{
	public Animator anim;
	public string hit = "hit";
	public ParticleSystem parti;
	public int emit = 25;
	
	public void Daamge(){
		parti.Emit(emit);
		anim.Play(hit);
	}
}