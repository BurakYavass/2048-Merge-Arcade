using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            other.GetComponent<ChestController>().Hit(GameManager.current.playerDamage);
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log(collision.gameObject);
    //     if (collision.gameObject.CompareTag("Chest"))
    //     {
    //         
    //         collision.gameObject.GetComponent<ChestController>().Hit(GameManager.current.playerDamage);
    //     }
    // }

    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.gameObject.CompareTag("Chest"))
    //     {
    //         
    //         hit.gameObject.GetComponent<ChestController>().Hit(GameManager.current.playerDamage);
    //     }
    // }
}
