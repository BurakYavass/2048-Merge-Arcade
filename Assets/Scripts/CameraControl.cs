using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerNormalCam;
    private CinemachineTransposer _transposer;
    [SerializeField] private CinemachineVirtualCamera playerBaseRightCam;
    [SerializeField] private CinemachineVirtualCamera playerBaseLeftCam;
    
    void Start()
    {
        GameEventHandler.current.OnPlayerRightArea += OnPlayerRightArea;
        GameEventHandler.current.OnPlayerLeftArea += OnPlayerLeftArea;
        _transposer = playerNormalCam.GetCinemachineComponent<CinemachineTransposer>();
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
            playerBaseRightCam.Priority = 4;
        }
    }
}
