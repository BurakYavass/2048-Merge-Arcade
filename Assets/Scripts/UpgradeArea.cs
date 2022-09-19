using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameObject blackSmith;
    [SerializeField] private Animator blackSmithAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventHandler.current.PlayerUpgradeArea();
        }
    }
}
