using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnStartState : StateMachineBehaviour
{
	[SerializeField] float startDuration = 5f;
	float timer;
	
	float NewDuration{ get{ return startDuration * Random.value; }}
	string startParam{ get{ return EnemyStateLogic.startParam; }}
	
    override public void OnStateEnter(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		animator.SetBool(EnemyStateLogic.startParam, true);
		timer = NewDuration;
    }

    override public void OnStateUpdate(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		timer -= Time.deltaTime;
		if(timer <= 0){ animator.SetBool(startParam, false); }
    }

    override public void OnStateExit(
		Animator animator,
		AnimatorStateInfo stateInfo,
		int layerIndex
	){
		animator.SetBool(startParam, false);
    }
}