using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController _playerController;

    private bool playerHit = false;

    private void Start()
    {
        GameEventHandler.current.OnPlayerHit += CurrentPlayerHit;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerHit -= CurrentPlayerHit;
    }

    private void CurrentPlayerHit(bool hit)
    {
        playerHit = hit;
    }

    void LateUpdate()
    {
        if (_playerController.walking)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
            animator.SetBool("Hit", false);
        }
        else if (playerHit)
        {
            animator.SetBool("Hit", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);
            animator.SetBool("Hit", false);
        }
    }
}
