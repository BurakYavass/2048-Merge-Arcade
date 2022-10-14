using UnityEngine;

public class TargetDetected : MonoBehaviour
{
    public bool targetEnterExit;
    [SerializeField] private PlayerCollisionHandler playerCollisionHandler;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            playerCollisionHandler.chestController = other.GetComponent<ChestController>();
            targetEnterExit = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            playerCollisionHandler.enemyController = other.GetComponent<EnemyController>();
            targetEnterExit = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            playerCollisionHandler.chestController = null;
            targetEnterExit = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            playerCollisionHandler.enemyController = null;
            targetEnterExit = false;
        }
    }
}
