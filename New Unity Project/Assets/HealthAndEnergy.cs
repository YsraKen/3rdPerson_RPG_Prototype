using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthAndEnergy : MonoBehaviour
{
	public float stunPercent;
	
	public float maxHealth;
	float currentHealth, currentEnergy;
	
	public float
		healthSyncSpeed = 0.5f,
		energySyncSpeed = 1f;
	
	const float synchBandwithUpdate = 0.005f;
	public bool isBeingStunned; // please remove the public, i just put it to get rid of warning "unused" messages
	
	public Image healthBar, energyBar;
	public Gradient gradient;
	
	Color healthColor, energyColor;
	
	IEnumerator currentCoroutine;
	
	public void Reset(){
		currentHealth = maxHealth;
		currentEnergy = maxHealth;
		
		StopCurrentCoroutine();
		
		var healthPercent = currentHealth / maxHealth;
		healthColor = gradient.Evaluate(healthPercent);
		
		healthBar.fillAmount = healthPercent;
		energyBar.fillAmount = healthPercent;
		
		healthBar.color = healthColor;
		energyBar.color = healthColor;
	}
	
	public void DamageTest(){
		var damage = currentHealth * Random.value;
		currentHealth -= damage;
		
		SynchronizeHealthAndEnergy();
	}
	
	public void DeEnergizeTest(){
		var deEnergize = currentEnergy * Random.value;
		currentEnergy -= deEnergize;
		
		SynchronizeHealthAndEnergy();
	}
	
	public void HealTest(){
		var healingRange = maxHealth - currentHealth;
		var heal = healingRange * Random.value;
		
		currentHealth += heal;
		SynchronizeHealthAndEnergy();
	}
	
	public void EnergizeTest(){
		var energizeRange = maxHealth - currentEnergy;
		var energize = energizeRange * Random.value;
		
		currentEnergy += energize;
		SynchronizeHealthAndEnergy();
	}
	
	void SynchronizeHealthAndEnergy(){
		StopCurrentCoroutine();
		
		currentCoroutine = SynchHealthEnergyRoutine();
		StartCoroutine(currentCoroutine);
	}
	
	IEnumerator SynchHealthEnergyRoutine(){
		while(!isSynchronized()){
			var difference = currentEnergy - currentHealth;
			var healthSync = difference * healthSyncSpeed;
			var energySync = difference * energySyncSpeed;
			
			currentHealth += healthSync * Time.deltaTime;
			currentEnergy -= energySync * Time.deltaTime;
			
			stunPercent = Mathf.Abs(Magnitude() / currentHealth);
			
			if(stunPercent >= 1f){ isBeingStunned = true; } // no auto-false if stunPercent is below 1, should wait until synchronization is over
			
			UpdateUI(stunPercent);
			
			yield return null;
		}
		
		isBeingStunned = false; // synchronization is over, then tsaka pa lang magiging !isBeingStunned
		
		float Magnitude(){
			var maximum = Mathf.Max(currentHealth, currentEnergy);
			var minimum = Mathf.Min(currentHealth, currentEnergy);
			
			return maximum - minimum;
		}
		
		bool isSynchronized(){
			var maxValue = currentHealth + synchBandwithUpdate;
			var minValue = currentHealth - synchBandwithUpdate;
			
			var aboveMinimum = currentEnergy > minValue;
			var belowMaximum = currentEnergy < maxValue;
			
			return aboveMinimum && belowMaximum;
		}
	}
	
	void StopCurrentCoroutine(){
		if(currentCoroutine != null){
			StopCoroutine(currentCoroutine);
		}
	}
	
	void UpdateUI(float stunPercent){
		var healthPercent = currentHealth / maxHealth;
		var energyPercent = currentEnergy / maxHealth;
		
		healthBar.fillAmount = healthPercent;
		energyBar.fillAmount = energyPercent;
		
		healthColor = gradient.Evaluate(healthPercent);
		energyColor = Color.Lerp(healthColor, Color.red, stunPercent);
		
		healthBar.color = healthColor;
		energyBar.color = energyColor;
	}
}