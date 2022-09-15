using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackList : MonoBehaviour
{
    public static PlayerStackList Current;
    [SerializeField] public Transform stackPoint;

    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
