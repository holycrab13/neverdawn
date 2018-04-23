using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IEnterStateListener
{
    void OnEnterState();
}

public class EnterAlertBehaviour : StateMachineBehaviour {

    public string stateName;

    public IEnterStateListener enterStateListener;

	  //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enterStateListener != null)
        {
            enterStateListener.OnEnterState();
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    internal static void AlertEnterState(Animator animator, string p, IEnterStateListener characterMeleeAttackAbilityAction)
    {
        EnterAlertBehaviour exitAlerter = animator.GetBehaviours<EnterAlertBehaviour>()
           .FirstOrDefault(a => a.stateName == p);

        exitAlerter.enterStateListener = characterMeleeAttackAbilityAction;
    }
}
