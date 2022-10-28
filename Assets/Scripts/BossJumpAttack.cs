using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BossJumpAttack : StateMachineBehaviour
{
    private BossController _bossController;
    private NavMeshAgent _bossAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossController = animator.GetComponent<BossController>();
        _bossAgent = animator.GetComponent<NavMeshAgent>();
        
        _bossAgent.enabled = false;
        var playerPosition = PlayerController.Current.transform.position;
        _bossController.bossJumpArea.transform.position = new Vector3(playerPosition.x,.6f,playerPosition.z);
        _bossController.bossJumpArea.SetActive(true);
        _bossController.fillImage.DOScale(1, 3.0f);
        _bossController.transform.DOMove(playerPosition, 3.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Rigidbody>().MovePosition(PlayerController.Current.transform.position);
        _bossAgent.enabled = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossController.fillImage.localScale = Vector3.zero;
        _bossController.fillImage.DOKill();
        _bossController.bossJumpArea.SetActive(false);
        _bossController.transform.DOKill();
        _bossAgent.enabled = true;
        _bossController.skillActive = false;
    }
    
}
