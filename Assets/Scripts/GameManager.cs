using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    
    public float playerSpeed;
    private int _speedUpgradeRequire;
    public int playerDamage;
    private int _damageUpgradeRequire;
    private int _armorUpgradeRequire;

    private int _speedState = 0;
    private int _damageState = 0;
    private int _armorState = 0;

    private int playerMoney;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerBallCounter playerBallCounter;
    [SerializeField] private BallController ballController;
    [SerializeField] private UpgradeArea upgradeArea;
    [SerializeField] private List<int> speedUpgradeState;
    [SerializeField] private List<int> damageUpgradeState;
    [SerializeField] private List<int> armorUpgradeState;

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
        _speedUpgradeRequire = speedUpgradeState[_speedState];
        _damageUpgradeRequire = damageUpgradeState[_damageState];
        _armorUpgradeRequire = armorUpgradeState[_armorState];
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;   
    }

    private void OnPlayerUpgradeArea(bool openClose,int value)
    {
        uiManager.UpgradePanel(openClose);
        playerMoney = playerBallCounter.stackValue;
    }

    public void PlayerHealth()
    {
        if (playerBallCounter.stackValue >= 128)
        {
            upgradeArea.UpgradeCalculate(_speedUpgradeRequire);
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
            upgradeArea.UpgradeCalculate(_speedUpgradeRequire);
            playerBallCounter.stackValue -= _speedUpgradeRequire;
            playerSpeed += 2.0f;
            _speedState++;
            if (_speedState==1)
            {
                _speedUpgradeRequire = speedUpgradeState[_speedState];
            }
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
            upgradeArea.UpgradeCalculate(_damageUpgradeRequire);
            playerDamage += 5;
            playerBallCounter.stackValue -= _damageUpgradeRequire;
            Debug.Log("PlayerDamageIncrease");
        }
        else
        {
            Debug.Log("Not Enoughf Money");
        }
        
    }
}
