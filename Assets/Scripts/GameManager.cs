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
    public bool speedMax =false;
    public bool armorMax =false;
    public bool damageMax =false;
    

    [NonSerialized]
    public int _speedState = 0;
    [NonSerialized]
    public int _damageState = 0;
    [NonSerialized]
    public int _armorState = 0;

    private int playerMoney;

    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerBallCounter playerBallCounter;
    [SerializeField] private UpgradableItem playerUpgradableItems;
    [SerializeField] private BallController ballController;
    [SerializeField] private UpgradeArea upgradeMachine;
    [SerializeField] public List<int> speedUpgradeState = new List<int>();
    [SerializeField] public List<int> damageUpgradeState = new List<int>();
    [SerializeField] public List<int> armorUpgradeState = new List<int>();

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }
    
    void Start()
    {
        gameEventHandler.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
        gameEventHandler.OnPlayerLevelUnlockArea += PlayerLevelUnlockArea;
        Application.targetFrameRate = 60;
        _speedUpgradeRequire = speedUpgradeState[_speedState];
        _damageUpgradeRequire = damageUpgradeState[_damageState];
        _armorUpgradeRequire = armorUpgradeState[_armorState];
    }

    private void Update()
    {
        playerMoney = playerBallCounter.stackValue;
    }

    private void OnDisable()
    {
        gameEventHandler.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
        gameEventHandler.OnPlayerLevelUnlockArea -= PlayerLevelUnlockArea;
    }
    
    private void PlayerLevelUnlockArea(bool openClose)
    {
        uiManager.LevelUnlockPanel(openClose);
    }

    private void OnPlayerUpgradeArea(bool openClose)
    {
        uiManager.UpgradePanel(openClose);
    }

    public void PlayerArmor()
    {
        if (playerBallCounter.stackValue >=_armorUpgradeRequire)
        {
            _armorUpgradeRequire = armorUpgradeState[_armorState];
            if (_armorState==0)
            {
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                playerBallCounter.stackValue -= _armorUpgradeRequire;
                playerUpgradableItems.ArmorChanger(_armorState);
                _armorState++;
            }
            else if (_armorState== 1)
            {
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                playerBallCounter.stackValue -= _armorUpgradeRequire;
                playerUpgradableItems.ArmorChanger(_armorState);
                _armorState++;
            }
            else if (_armorState == 2)
            {
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                playerBallCounter.stackValue -= _armorUpgradeRequire;
                playerUpgradableItems.ArmorChanger(_armorState);
                armorMax = true;
                Debug.Log("max");
            }
        }
    }

    public void PlayerSpeed()
    {
        if (playerBallCounter.stackValue >= _speedUpgradeRequire)
        {
            _speedUpgradeRequire = speedUpgradeState[_speedState];
            if (_speedState==0)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                playerBallCounter.stackValue -= _speedUpgradeRequire;
                _speedState++;
            }
            else if (_speedState == 1)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                playerBallCounter.stackValue -= _speedUpgradeRequire;
                _speedState++;
            }
            else if (_speedState == 2)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                playerBallCounter.stackValue -= _speedUpgradeRequire;
                speedMax = true;
                Debug.Log("max");
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
            _damageUpgradeRequire = damageUpgradeState[_damageState];
            if (_damageState==0)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5;
                playerBallCounter.stackValue -= _damageUpgradeRequire;
                _damageState++;
            }
            else if (_damageState == 1)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5;
                playerBallCounter.stackValue -= _damageUpgradeRequire;
                playerUpgradableItems.WeaponChanger(_damageState);
                _damageState++;
            }
            else if(_damageState == 2)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5;
                playerBallCounter.stackValue -= _damageUpgradeRequire;
                playerUpgradableItems.WeaponChanger(_damageState);
                damageMax = true;
                Debug.Log("MaxLevel");
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
        if (speedMax)
        {
            return (int.Parse("Max"));
        }
        else
        {
            return speedUpgradeState[_speedState];
        }
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