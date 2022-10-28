using System;
using UnityEngine;

public class TargetDetected : MonoBehaviour
{
    public bool targetEnterExit;
    [SerializeField] private PlayerController playerController;
    [SerializeField] public PlayerAnimationHandler playerAnimationHandler;
    //[SerializeField] private ChestController chestController;
    

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.playerDie)
        {
            if (other.gameObject.CompareTag("Chest"))
            {
                if (!other.GetComponent<ChestController>().chestClose)
                {
                    playerAnimationHandler.HitAnimation(true);
                    targetEnterExit = true;
                }
            }

            if (other.gameObject.CompareTag("Enemy"))
            {
                if (!other.GetComponent<EnemyController>().enemyDie)
                {
                    playerAnimationHandler.HitAnimation(true);
                    targetEnterExit = true;
                }
            }

            if (other.gameObject.CompareTag("Boss"))
            {
                playerAnimationHandler.HitAnimation(true);
                targetEnterExit = true;
            }
        }
        else
        {
            playerAnimationHandler.HitAnimation(false);
            targetEnterExit = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            playerAnimationHandler.HitAnimation(false);
            targetEnterExit = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            playerAnimationHandler.HitAnimation(false);
            targetEnterExit = false;
        }
    }
}
