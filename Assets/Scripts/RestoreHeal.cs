using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestoreHeal : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float healTemp;

    private void Update()
    {
        if (playerController != null)
        {
            playerController.Healing(healTemp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                playerController = other.GetComponent<PlayerController>();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
        }
    }
}
