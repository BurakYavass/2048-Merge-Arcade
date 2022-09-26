using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public static GameEventHandler current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    public event Action<bool> OnPlayerUpgradeArea;
    public event Action<bool> OnPlayerHit;
    public event Action<bool> OnPlayerRightArea;
    public event Action<bool> OnPlayerLeftArea;
    public event Action<bool> OnBallMergeArea;

    public void BallMergeArea(bool enterExit)
    {
        OnBallMergeArea?.Invoke(enterExit);
    }

    public void PlayerLeftArea(bool enterExit)
    {
        OnPlayerLeftArea?.Invoke(enterExit);
    }
    
    public void PlayerRightArea(bool enterExit)
    {
        OnPlayerRightArea?.Invoke(enterExit);
    }

    public void PlayerUpgradeArea(bool onUpgrade)
    {
        OnPlayerUpgradeArea?.Invoke(onUpgrade);
    }

    public void PlayerHit(bool hit)
    {
        OnPlayerHit?.Invoke(hit);
    }
}
