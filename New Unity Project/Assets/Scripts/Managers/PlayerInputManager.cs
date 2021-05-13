using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	public static PlayerInputManager instance;
	void Awake(){ instance = this; }
	
	public Thumbstick thumbstick;
	
	public Vector2 Move(){ return thumbstick.Value(); }
	
	static readonly string
		jumpButton = "Jump",
		fire1Button = "Fire1",
		fire2Button = "Fire2";
	
	public bool Fire1(){ return Input.GetButtonDown(fire1Button); }
	public bool Fire2(){ return Input.GetButtonDown(fire2Button); }
	
	public bool Fire1Hold(){ return Input.GetButton(fire1Button); }
	public bool Fire2Hold(){ return Input.GetButton(fire2Button); }
	
	public bool Jump(){
		var outA = Input.GetButtonDown(jumpButton);
		// var outB = ui button jump;
		
		return outA;
	}
	
	public bool Evade(){ return Input.GetKeyDown("e"); }
	public bool EvadeBoost(){ return Input.GetKeyDown("e"); } // angel evade in DmC13
	
	public bool EnableGrapplingMode(){ return Input.GetKeyDown("q"); }
	public bool EnableGrapplingPullMode(){ return Input.GetKeyDown("x"); }
}