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

    private void Update()
    {
        
    }

    private void OnPlayerLeftArea(bool enterExit)
    {
        if (enterExit)
        {
            //playerNormalCam.transform.rotation = Quaternion.Euler(30,-30,0);
            //_transposer.m_FollowOffset = new Vector3(7, 15, -20);
            playerBaseLeftCam.Priority = 10;
        }
        else
        {
            //playerNormalCam.transform.rotation = Quaternion.Euler(30,0,0);
            //_transposer.m_FollowOffset = new Vector3(0, 15, -20);
            playerBaseLeftCam.Priority = 4;
        }
    }

    private void OnPlayerRightArea(bool enterExit)
    {
        if (enterExit)
        {
            //playerNormalCam.transform.rotation = Quaternion.Euler(30,30,0);
            //_transposer.m_FollowOffset = new Vector3(-7, 15, -20);
            //playerNormalCam.Priority = 4;
            playerBaseRightCam.Priority = 10;
        }
        else
        {
            //playerNormalCam.transform.rotation = Quaternion.Euler(30,0,0);
            //_transposer.m_FollowOffset = new Vector3(0, 15, -20);
            //playerNormalCam.Priority = 7;
            playerBaseRightCam.Priority = 4;
        }
    }
}
