using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IExitStateListener
{
    void OnExitState();
}

public class ExitAlertBehaviour : StateMachineBehaviour {

    public string triggerName;

    public List<IExitStateListener> exitStateListeners;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (exitStateListeners != null)
        {
            foreach (IExitStateListener subscriber in exitStateListeners)
            {
                subscriber.OnExitState();
            }

            exitStateListeners.Clear();
        }
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    internal static void AlertExitState(Animator animator, string p, IExitStateListener stateListener)
    {
        ExitAlertBehaviour exitAlerter = animator.GetBehaviours<ExitAlertBehaviour>()
           .FirstOrDefault(a => a.triggerName == p);

        if (exitAlerter.exitStateListeners == null)
            exitAlerter.exitStateListeners = new List<IExitStateListener>();

        exitAlerter.exitStateListeners.Add(stateListener);
    }
}
