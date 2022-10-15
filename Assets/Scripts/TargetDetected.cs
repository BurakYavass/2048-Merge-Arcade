using System;
using UnityEngine;

public class TargetDetected : MonoBehaviour
{
    public bool targetEnterExit;
    [SerializeField] private PlayerCollisionHandler playerCollisionHandler;
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;
    [SerializeField] private ChestController chestController;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            //chestController = other.GetComponent<ChestController>();
            playerAnimationHandler.HitAnimation(true);
            targetEnterExit = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            //playerCollisionHandler.enemyController = other.GetComponent<EnemyController>();
            playerAnimationHandler.HitAnimation(true);
            targetEnterExit = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            //chestController = null;
            playerAnimationHandler.HitAnimation(false);
            targetEnterExit = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
           //playerCollisionHandler.enemyController = null;
           playerAnimationHandler.HitAnimation(false);
            targetEnterExit = false;
        }
    }
}
