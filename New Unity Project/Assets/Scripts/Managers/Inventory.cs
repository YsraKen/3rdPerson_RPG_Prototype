using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory instance;
	void Awake(){ instance = this; }
	
	[SerializeField] int capacity = 20;
	
	public Text orbsCount; // DEVBUILD
	
	public List<Item> items = new List<Item>();
	
	public void Add(Item newItem){
		if(items.Count == capacity){ return; }
		
		// identify if same type then stack
		items.Add(newItem);
		UpdateOrbText();
	}
	
	public bool GetOrb(){ // development mode
		if(items.Count <= 0){ return false; }
		
		var item = items[0];
		items.Remove(item);
		
		UpdateOrbText();
		
		return true;
	}
	
	void UpdateOrbText(){
		orbsCount.text = "Orbs: " + items.Count;
	}
}