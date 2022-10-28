using System;
using System.Collections;
using UnityEngine;

public class RestoreHeal : MonoBehaviour
{
    [SerializeField] private TutorialStage tutorialStage;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float healTemp;
    public bool tutorial;

    private void Update()
    {
        // if (playerController != null)
        // {
        //     playerController.Healing(healTemp,true);
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                playerController = other.GetComponent<PlayerController>();
                playerController.Healing(healTemp,true);
                StopCoroutine(Delay());
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialControl.Instance.CompleteStage(tutorialStage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.Healing(0, false);
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.5f);
        playerController = null;
    }
}
