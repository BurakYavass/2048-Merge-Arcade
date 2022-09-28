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

    [NonSerialized]
    public int _speedState = 0;
    [NonSerialized]
    public int _damageState = 0;
    [NonSerialized]
    public int _armorState = 0;

    private int playerMoney;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerBallCounter playerBallCounter;
    [SerializeField] private BallController ballController;
    [SerializeField] private UpgradeArea upgradeMachine;
    [SerializeField] public List<int> speedUpgradeState;
    [SerializeField] public List<int> damageUpgradeState;
    [SerializeField] public List<int> armorUpgradeState;

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

    private void OnPlayerUpgradeArea(bool openClose)
    {
        uiManager.UpgradePanel(openClose);
        playerMoney = playerBallCounter.stackValue;
    }

    public void PlayerArmor()
    {
        if (playerBallCounter.stackValue >= 128)
        {
            upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
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
            upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
            playerBallCounter.stackValue -= _speedUpgradeRequire;
            playerSpeed += 2.0f;
            _speedState++;
            if (_speedState==1)
            {
                _speedUpgradeRequire = speedUpgradeState[_speedState];
            }
            else if (_speedState == 2)
            {
                _speedUpgradeRequire = speedUpgradeState[_speedState];
            }
            else
            {
                //_speedUpgradeRequire = speedUpgradeState[_speedState];
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
        if (playerBallCounter.stackValue >= _damageUpgradeRequire )
        {
            upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
            playerDamage += 5;
            playerBallCounter.stackValue -= _damageUpgradeRequire;
            _damageState++;
            if (_damageState==1)
            {
                _damageState = damageUpgradeState[_damageState];
            }
            else if (_damageState == 2)
            {
                _damageUpgradeRequire = damageUpgradeState[_damageState];
            }
            else
            {
                Debug.Log("MaxLevel");
                //_damageUpgradeRequire = damageUpgradeState[_damageState];
            }
            Debug.Log("PlayerDamageIncrease");
        }
        else
        {
            Debug.Log("Not Enoughf Money");
        }
        
    }
    
    public int ReturnSpeedState()
    {
        return speedUpgradeState[_speedState];
    }
    public int ReturnArmorState()
    {
        return armorUpgradeState[_armorState];
    }
    public int ReturnDamageState()
    {
        return damageUpgradeState[_damageState];
    }
}
