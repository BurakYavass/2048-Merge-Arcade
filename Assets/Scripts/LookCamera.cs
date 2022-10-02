using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public Transform _camera;

    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward );
    }
}
