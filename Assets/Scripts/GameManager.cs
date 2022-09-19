using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [SerializeField] private UIManager uiManager;

    public float playerSpeed;
    public float playerDamage;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;   
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;   
    }

    private void OnPlayerUpgradeArea(bool openClose)
    {
        uiManager.UpgradePanel(openClose);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerSpeed()
    {
        
    }

    public void PlayerDamage()
    {
        
    }
}
