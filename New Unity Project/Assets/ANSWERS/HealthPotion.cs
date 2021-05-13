using UnityEngine;

[CreateAssetMenu]
public class HealthPotion : ItemTest{
	
	public int health;
	
	public override void Use(){
		base.Use();
		
		CharacterStats.instance.IncreaseHealth(health);
	}
}