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

    public event Action<bool> OnPlayerHit;

    public void PlayerHit(bool hit)
    {
        OnPlayerHit?.Invoke(hit);
    }
}