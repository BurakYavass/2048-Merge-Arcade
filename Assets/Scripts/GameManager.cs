using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [SerializeField] private float startSpeed;
    [NonSerialized] public float playerSpeed;

    [SerializeField] private float startArmor;
    [NonSerialized] public float _playerArmor;

    [SerializeField] private float startDamage;
    [NonSerialized] public float playerDamage;
    private int _speedUpgradeRequire;
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
    public int _armorState;

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
        Application.targetFrameRate = 60;
        _speedUpgradeRequire = speedUpgradeState[_speedState];
        _damageUpgradeRequire = damageUpgradeState[_damageState];
        _armorUpgradeRequire = armorUpgradeState[_armorState];

        playerSpeed = PlayerPrefs.GetFloat("PlayerSpeed",startSpeed);
        _speedState = PlayerPrefs.GetInt("SpeedState", 0);
        playerDamage = PlayerPrefs.GetFloat("PlayerDamage", startDamage);
        _damageState = PlayerPrefs.GetInt("damageState", 0);
        _playerArmor = PlayerPrefs.GetFloat("PlayerArmor", startArmor);
        _armorState = PlayerPrefs.GetInt("ArmorState", 0);
        if (_armorState == 3)
        {
            armorMax = true;
        }
        if (_speedState == 2)
        {
            speedMax = true;
        }
        if (_damageState == 2)
        {
            damageMax = true;
        }
        playerUpgradableItems.ArmorChanger(_armorState);
        playerUpgradableItems.WeaponChanger(_damageState);
        
    }

    private void Update()
    {
        playerMoney = playerBallCounter.stackValue;
    }

    private void OnDisable()
    {
        gameEventHandler.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
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
            playerBallCounter.stackValue -= _armorUpgradeRequire;
            _armorState++;
            if (_armorState==1)
            {
                _playerArmor += 50f;
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                playerUpgradableItems.ArmorChanger(_armorState);
                PlayerPrefs.SetInt("ArmorState",_armorState);
            }
            else if (_armorState== 2)
            {
                _playerArmor += 50f;
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                playerUpgradableItems.ArmorChanger(_armorState);
                PlayerPrefs.SetInt("ArmorState",_armorState);
                
            }
            else if (_armorState == 3)
            {
                _playerArmor += 50f;
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                playerUpgradableItems.ArmorChanger(_armorState);
                PlayerPrefs.SetInt("ArmorState",_armorState);
                armorMax = true;
                Debug.Log("max");
            }
            else
            {
                
            }
        }
    }

    public void PlayerSpeed()
    {
        if (playerBallCounter.stackValue >= _speedUpgradeRequire)
        {
            _speedUpgradeRequire = speedUpgradeState[_speedState];
            playerBallCounter.stackValue -= _speedUpgradeRequire;
            if (_speedState==0)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerSpeed",playerSpeed);
                _speedState++;
                PlayerPrefs.SetInt("SpeedState",_speedState);
            }
            else if (_speedState == 1)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerSpeed",playerSpeed);
                _speedState++;
                PlayerPrefs.SetInt("SpeedState",_speedState);
            }
            else if (_speedState == 2)
            {
                playerSpeed += 2.0f;
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerSpeed",playerSpeed);
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
            playerBallCounter.stackValue -= _damageUpgradeRequire;
            if (_damageState==0)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
                _damageState++;
                PlayerPrefs.SetInt("damageState",_damageState);
            }
            else if (_damageState == 1)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
                playerUpgradableItems.WeaponChanger(_damageState);
                _damageState++;
                PlayerPrefs.SetInt("damageState",_damageState);
            }
            else if(_damageState == 2)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
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