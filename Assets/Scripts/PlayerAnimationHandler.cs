using System;
using System.Collections;
using System.Collections.Generic;
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
        animator.SetFloat("WalkingSpeed" , playerController.PlayerSpeed);
        if (playerController.walking)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
            animator.SetBool("Hit", false);
            //stackPointAnimation.Play("StackPointAnim");
        }
        else if (playerHit)
        {
            //playerHit = false;
            animator.SetBool("Hit", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
            //stackPointAnimation.Stop("StackPointAnim");
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);
            animator.SetBool("Hit", false);
            //stackPointAnimation.Stop("StackPointAnim");
        }
    }
}
