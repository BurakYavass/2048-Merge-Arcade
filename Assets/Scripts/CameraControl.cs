using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerNormalCam;
    [SerializeField] private CinemachineVirtualCamera playerBaseRightCam;
    [SerializeField] private CinemachineVirtualCamera playerBaseLeftCam;
    [SerializeField] private CinemachineVirtualCamera UpgradeAreaCam;
    
    void Start()
    {
        GameEventHandler.current.OnPlayerRightArea += OnPlayerRightArea;
        GameEventHandler.current.OnPlayerLeftArea += OnPlayerLeftArea;
        GameEventHandler.current.OnPlayerUpgradeArea += OnplayerUpgradeArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerRightArea -= OnPlayerRightArea;
        GameEventHandler.current.OnPlayerLeftArea -= OnPlayerLeftArea;
        GameEventHandler.current.OnPlayerUpgradeArea -= OnplayerUpgradeArea;
    }

    private void OnPlayerLeftArea(bool enterExit)
    {
        if (enterExit)
        {
            playerBaseLeftCam.Priority = 10;
        }
        else
        {
            playerBaseLeftCam.Priority = 4;
        }
    }

    private void OnPlayerRightArea(bool enterExit)
    {
        if (enterExit)
        {
            playerBaseRightCam.Priority = 10;
        }
        else
        {
            playerBaseRightCam.Priority = 3;
        }
    }

    private void OnplayerUpgradeArea(bool enterExit,int value)
    {
        if (enterExit)
        {
            //playerNormalCam.enabled = false;
            UpgradeAreaCam.Priority = 10;
        }
        else
        {
            //playerNormalCam.enabled = true;
            UpgradeAreaCam.Priority = 2;
        }
    }
}
