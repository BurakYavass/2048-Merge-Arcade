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

    public event Action<bool,int> OnPlayerLevelUnlockArea; 
    public event Action<bool> OnPlayerUpgradeArea;
    public event Action<bool> OnPlayerMergeArea; 
    public event Action<bool> OnPlayerHit;
    public event Action<bool> OnPlayerRightArea;
    public event Action<bool> OnPlayerLeftArea;
    public event Action<bool ,GameObject> OnBallMergeArea;

    public event Action<bool ,GameObject> OnBallUpgradeArea;

    public void BallUpgradeArea(bool enterExit,GameObject obje)
    {
        OnBallUpgradeArea?.Invoke(enterExit,obje);
    }

    public void BallMergeArea(bool enterExit,GameObject obje)
    {
        OnBallMergeArea?.Invoke(enterExit,obje);
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

    public void PlayerMergeArea(bool enterExit)
    {
        OnPlayerMergeArea?.Invoke(enterExit);
    }

    public void PlayerHit(bool hit)
    {
        OnPlayerHit?.Invoke(hit);
    }

    public void PlayerLevelUnlockArea(bool enterExit ,int value)
    {
        OnPlayerLevelUnlockArea?.Invoke(enterExit,value);
    }
}
