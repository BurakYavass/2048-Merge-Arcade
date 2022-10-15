using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBallCounter : MonoBehaviour
{
    [SerializeField] private BallController _ballController;
    [SerializeField] private UpgradeArea _upgradeArea;

    public int stackValue;

    private void Update()
    {
        stackValue = _upgradeArea._totalValue;
    }
    
    public float GetValue()
    {
        return stackValue;
    }
}
