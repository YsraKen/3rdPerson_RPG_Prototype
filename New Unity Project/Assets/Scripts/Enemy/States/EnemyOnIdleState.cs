using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnIdleState : StateMachineBehaviour
{
	// public EnemyStateLogic stateLogic;
	
	[SerializeField] float idleTimer = 15f;
	
	float timer;
	float newTimer{ get{ return idleTimer * Random.value; }}
	
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		/* if(stateLogic == null){
			stateLogic = animator.GetComponentInParent<EnemyStateLogic>();
		} */
		timer = newTimer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if(timer <= 0){
			animator.SetTrigger(EnemyStateLogic.approachParam);
			// stateLogic.Approach();
			timer = newTimer;
		}else{
			timer -= Time.fixedDeltaTime;
		}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
