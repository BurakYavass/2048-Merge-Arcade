using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameObject blackSmith;
    [SerializeField] private Animator blackSmithAnimator;

    private void Start()
    {
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
    }

    private void OnPlayerUpgradeArea(bool working)
    {
        blackSmithAnimator.SetBool("working",working);
    }
}
