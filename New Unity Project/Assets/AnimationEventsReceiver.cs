using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsReceiver : MonoBehaviour
{
	public AnimEvents eventsReceivers;
	
	[Space(10)]
	public string[] testClipPlay;
	public int playIndex = 0;
	public bool increment;	
	
	Animator anim;
	void Start(){ anim = GetComponent<Animator>(); }
	
	public void DoGrappling(){ eventsReceivers.doGrapplingReceiver?.Invoke(); }
	public void Combo(){ eventsReceivers.comboListener?.Invoke(); }
	
	public void TestClipPlay(){
		if(increment){
			playIndex ++;
			playIndex = playIndex % testClipPlay.Length;
		}
		
		anim.Play(testClipPlay[playIndex]);
	}
	
	[System.Serializable] public class AnimEvents{
		public UnityEvent
			doGrapplingReceiver = new UnityEvent(),
			comboListener = new UnityEvent();
	}
}