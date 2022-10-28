using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttack : StateMachineBehaviour
{
    private NavMeshAgent _bossAgent;
    private BossController _bossController;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossAgent = animator.GetComponent<NavMeshAgent>();
        _bossController = animator.GetComponent<BossController>();
        _bossController.transform.LookAt(PlayerController.Current.transform.position);
        _bossAgent.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossAgent.enabled = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossAgent.enabled = true;
    }
    
}
