using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    private bool _once = false;


    public void HitAnimation(bool hittable)
    {
        animator.SetBool("Hit", hittable);
        
    }
    
    private void AnimationOver()
    {
        animator.SetBool("Hit", false);
    }

    void Update()
    {
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
