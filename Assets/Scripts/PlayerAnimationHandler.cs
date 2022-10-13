using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //[SerializeField] private Animation stackPointAnimation;
    [SerializeField] private PlayerController playerController;

    private bool playerHit = false;
    

    public void CurrentPlayerHit(bool hit)
    {
        playerHit = hit;
    }

    void Update()
    {
        //animator.SetLayerWeight(1,playerHit ? 1 : 0);
        animator.SetBool("Hit", playerHit);
        

        animator.SetFloat("WalkingSpeed" , playerController.PlayerSpeed/10);
        if (playerController.PlayerSpeed > .1f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);

        }
    }
}
