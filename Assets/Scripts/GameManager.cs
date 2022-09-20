using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    
    public float playerSpeed;
    private float _speedUpgradeRequire;
    public float playerDamage;
    private float _damageUpgradeRequire;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerBallCounter playerBallCounter;
    [SerializeField] private List<int> speedUpgradeState;
    [SerializeField] private List<int> damageUpgradeState;
    
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
        Application.targetFrameRate = 60;
        _speedUpgradeRequire = speedUpgradeState[0];
        _damageUpgradeRequire = speedUpgradeState[0];
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;   
    }

    private void OnPlayerUpgradeArea(bool openClose)
    {
        uiManager.UpgradePanel(openClose);
    }

    public void PlayerHealth()
    {
        if (playerBallCounter.stackValue >= 128)
        {
            Debug.Log("PlayerHealthIncrease");
        }
        else
        {
            Debug.Log("Not Enoughf Money");
        }
        
    }

    public void PlayerSpeed()
    {
        if (playerBallCounter.stackValue >= _speedUpgradeRequire)
        {
            playerBallCounter.stackValue -= _speedUpgradeRequire;
            Debug.Log("PlayerSpeedIncrease");
        }
        else
        {
            Debug.Log("Not Enoughf Money");
        }
    }

    public void PlayerDamage()
    {
        if (playerBallCounter.stackValue >= _damageUpgradeRequire)
        {
            playerBallCounter.stackValue -= _damageUpgradeRequire;
            Debug.Log("PlayerDamageIncrease");
        }
        else
        {
            Debug.Log("Not Enoughf Money");
        }
        
    }
}
