using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController _playerController;
    
    void LateUpdate()
    {
        if (_playerController.walking)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Idle", false);
        }
    }
}
