using UnityEngine;

public class CharacterStats : MonoBehaviour{
	
	public static CharacterStats instance;
	void Awake(){ instance = this; }
	
	public int maxHealth;
	public int currentHealth;
	
	void Start(){
		currentHealth = maxHealth;
	}
	
	public void IncreaseHealth(int amount){
		currentHealth += amount;
		Debug.Log("currentHealth increased! +" +amount, gameObject);
	}
}