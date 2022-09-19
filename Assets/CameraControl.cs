using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerNormalCam;
    [SerializeField] private CinemachineVirtualCamera playerBaseRightCam;
    [SerializeField] private CinemachineVirtualCamera playerBaseLeftCam;
    
    void Start()
    {
        GameEventHandler.current.OnPlayerRightArea += OnPlayerRightArea;
        GameEventHandler.current.OnPlayerLeftArea += OnPlayerLeftArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerRightArea -= OnPlayerRightArea;
        GameEventHandler.current.OnPlayerLeftArea -= OnPlayerLeftArea;
    }
    
    private void OnPlayerLeftArea(bool enterExit)
    {
        if (enterExit)
        {
            playerNormalCam.Priority = 4;
            playerBaseLeftCam.Priority = 10;
        }
        else
        {
            playerNormalCam.Priority = 7;
            playerBaseLeftCam.Priority = 4;
        }
    }

    private void OnPlayerRightArea(bool enterExit)
    {
        if (enterExit)
        {
            playerNormalCam.Priority = 4;
            playerBaseRightCam.Priority = 10;
        }
        else
        {
            playerNormalCam.Priority = 7;
            playerBaseRightCam.Priority = 4;
        }
    }
}
