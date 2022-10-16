using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [SerializeField] private float playerReviveDelay;
    [SerializeField] private float startSpeed;
    [NonSerialized] public float playerSpeed;

    [SerializeField] private float startArmor;
    public float _playerArmor;

    [SerializeField] private float startDamage;
    [NonSerialized] public float playerDamage;
    private int _speedUpgradeRequire;
    private int _damageUpgradeRequire;
    private int _armorUpgradeRequire;
    public bool speedMax =false;
    public bool armorMax =false;
    public bool damageMax =false;

    private bool _once = false;
    

    [NonSerialized]
    public int _speedState = 0;
    [NonSerialized]
    public int _damageState = 0;
    [NonSerialized]
    public int _armorState;

    private int playerMoney;

    [SerializeField] private GameObject player;
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BallController ballController;
    [SerializeField] private UpgradeArea upgradeMachine;
    [SerializeField] public List<int> speedUpgradeState = new List<int>();
    [SerializeField] public List<int> damageUpgradeState = new List<int>();
    [SerializeField] public List<int> armorUpgradeState = new List<int>();
    
    private PlayerBallCounter _playerBallCounter;
    private UpgradableItem _playerUpgradableItems;
    private ParticlesController _playerParticles;
    private PlayerController _playerController;
   

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        
        _playerBallCounter = player.GetComponent<PlayerBallCounter>();
        _playerUpgradableItems = player.GetComponent<UpgradableItem>();
        _playerParticles = player.GetComponent<ParticlesController>();
        _playerController = player.GetComponent<PlayerController>();
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
        _playerUpgradableItems.ArmorChanger(_armorState);
        _playerUpgradableItems.WeaponChanger(_damageState);
    }
    
    void Start()
    {
        
        
    }
    private void OnDisable()
    {
        gameEventHandler.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
    }

    private void Update()
    {
        playerMoney = _playerBallCounter.stackValue;
        if (_playerController.playerDie)
        {
            // if ("reklam izlersen vb.")
            // {
            //     PlayerRevive();
            // }
            // else
            // {
            //     StartCoroutine(ReviveDelay(playerReviveDelay));
            // }
            
            if (!_once)
            {
                _once = true;
                StartCoroutine(ReviveDelay(playerReviveDelay));
                uiManager.PlayerRevive(true,playerReviveDelay);
                ballController.GoFree();
            }

            var playerFollowerList = _playerController.playerCollisionHandler.playerFollowPoints;

            for (int i = 0; i < playerFollowerList.Count; i++)
            {
                playerFollowerList[i].clearList = true;
            }
        }
       
    }
    
    private IEnumerator ReviveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerRevive();
    }

    private void PlayerRevive()
    {
        if (_once)
        {
            _playerController.transform.DOMove(new Vector3(0, 0.6f, 9.26f), 1).OnComplete((() =>
            {
                _playerController.playerHealthValueCurrent = _playerArmor-10;
                uiManager.PlayerRevive(false,playerReviveDelay);
                _playerController.playerDie = false;
                _playerController.playerCollisionHandler.enabled = true;
                
                var colliders = _playerController.GetComponents<Collider>();
                foreach (var cllider in colliders)
                {
                    cllider.enabled = true;
                }
                _playerController.rb.isKinematic = false;
                for (int i = 0; i < _playerController.closePart.Length; i++)
                {
                    _playerController.closePart[i].SetActive(true);
                }
                _once = false;
            
            }));
        }
        
    }

    private void OnPlayerUpgradeArea(bool openClose)
    {
        uiManager.UpgradePanel(openClose);
    }

    public void PlayerArmor()
    {
        if (_playerBallCounter.stackValue >=_armorUpgradeRequire)
        {
            _armorUpgradeRequire = armorUpgradeState[_armorState];
            _playerBallCounter.stackValue -= _armorUpgradeRequire;
            _armorState++;
            if (_armorState==1)
            {
                _playerArmor += 50f;
                _playerParticles.PlayerUpgrade();
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                _playerUpgradableItems.ArmorChanger(_armorState);
                _playerController.playerHealthValueCurrent = _playerArmor;
                PlayerPrefs.SetInt("ArmorState",_armorState);
            }
            else if (_armorState== 2)
            {
                _playerArmor += 50f;
                _playerParticles.PlayerUpgrade();
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                _playerUpgradableItems.ArmorChanger(_armorState);
                _playerController.playerHealthValueCurrent = _playerArmor;
                PlayerPrefs.SetInt("ArmorState",_armorState);
                
            }
            else if (_armorState == 3)
            {
                _playerArmor += 50f;
                _playerParticles.PlayerUpgrade();
                upgradeMachine.UpgradeCalculate(_armorUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerArmor",_playerArmor);
                _playerUpgradableItems.ArmorChanger(_armorState);
                _playerController.playerHealthValueCurrent = _playerArmor;
                PlayerPrefs.SetInt("ArmorState",_armorState);
                armorMax = true;
                Debug.Log("max");
            }
        }
    }

    public void PlayerSpeed()
    {
        if (_playerBallCounter.stackValue >= _speedUpgradeRequire)
        {
            _speedUpgradeRequire = speedUpgradeState[_speedState];
            _playerBallCounter.stackValue -= _speedUpgradeRequire;
            if (_speedState==0)
            {
                playerSpeed += 2.0f;
                _playerParticles.PlayerUpgrade();
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerSpeed",playerSpeed);
                _speedState++;
                PlayerPrefs.SetInt("SpeedState",_speedState);
            }
            else if (_speedState == 1)
            {
                playerSpeed += 2.0f;
                _playerParticles.PlayerUpgrade();
                upgradeMachine.UpgradeCalculate(_speedUpgradeRequire);
                PlayerPrefs.SetFloat("PlayerSpeed",playerSpeed);
                _speedState++;
                PlayerPrefs.SetInt("SpeedState",_speedState);
            }
            else if (_speedState == 2)
            {
                playerSpeed += 2.0f;
                _playerParticles.PlayerUpgrade();
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
        if (_playerBallCounter.stackValue >= _damageUpgradeRequire)
        {
            _damageUpgradeRequire = damageUpgradeState[_damageState];
            _playerBallCounter.stackValue -= _damageUpgradeRequire;
            if (_damageState==0)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                _playerParticles.PlayerUpgrade();
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
                _damageState++;
                PlayerPrefs.SetInt("damageState",_damageState);
            }
            else if (_damageState == 1)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                _playerParticles.PlayerUpgrade();
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
                _playerUpgradableItems.WeaponChanger(_damageState);
                _damageState++;
                PlayerPrefs.SetInt("damageState",_damageState);
            }
            else if(_damageState == 2)
            {
                upgradeMachine.UpgradeCalculate(_damageUpgradeRequire);
                playerDamage += 5.01f;
                _playerParticles.PlayerUpgrade();
                PlayerPrefs.SetFloat("PlayerDamage",playerDamage);
                _playerUpgradableItems.WeaponChanger(_damageState);
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