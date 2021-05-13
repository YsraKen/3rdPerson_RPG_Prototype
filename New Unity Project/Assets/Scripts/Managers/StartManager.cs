using UnityEngine;

public class StartManager : MonoBehaviour
{
	[SerializeField] private GameObject[] startMethods;
	
	void Start(){
		foreach(var sm in startMethods){
			IStartManager ism = sm.GetComponent<IStartManager>();
			ism.OnStartUp();
		}
	}
}