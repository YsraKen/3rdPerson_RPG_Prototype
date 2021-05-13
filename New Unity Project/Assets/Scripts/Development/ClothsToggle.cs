using UnityEngine;

public class ClothsToggle : MonoBehaviour
{
	public Cloth[] cloths;
	bool toggle;
	
	public GameObject cap, cape, ragdoll;
	
	public void ToggleCloths(){
		toggle = !toggle;
		
		foreach(var c in cloths){ c.enabled = toggle; }
	}
	
	public void ToggleCloths(bool newToggle){
		foreach(var c in cloths){ c.enabled = newToggle; }
	}
	
	public void ToggleCap(bool newToggle){
		cap.SetActive(newToggle);
	}
	
	public void ToggleCape(bool newToggle){
		cape.SetActive(newToggle);
	}
	
	public void ToggleRagdoll(bool newToggle){
		ragdoll.SetActive(newToggle);
	}
	
	public void SetTimeSpeed(float amount){
		Time.timeScale = Mathf.Clamp01(amount);
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
}