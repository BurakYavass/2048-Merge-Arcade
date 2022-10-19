using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    private GameManager _gameManager;
    public PlayerCollisionHandler playerCollisionHandler;
    [SerializeField] public Rigidbody rb;
    [NonSerialized] public float PlayerSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI fullHealth;
    [SerializeField] private TextMeshProUGUI currentHealth;
    [SerializeField] private Image sliderValue;
    [SerializeField] private Image damageTake;
    public GameObject[] closePart;
    public Collider[] colliders;
    private float _playerHealthValue;
    public float playerHealthValueCurrent;
    private float _playerHealthValueCurrentTemp;
    private float _currentMoveMultiplier;
    private float _takenDamage;
    private float _tempHeal;

    public bool walking = false;
    private bool _upgradeArea = false;
    private bool _teleporting = false;
    private bool _gethit;
    public bool healing = false;
    public bool playerDie;
    
    
    
    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
    }

    private void Start()
    {
        _playerHealthValue = _gameManager._playerArmor;
        playerHealthValueCurrent = _playerHealthValue;
        fullHealth.text = (Mathf.Round(_playerHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(playerHealthValueCurrent)).ToString("0");
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
        DOTween.Init();
        CameraChanger(0);
    }

    private void OnDestroy()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
    }

    private void OnPlayerUpgradeArea(bool enterExit)
    {
        if (enterExit)
        {
            transform.DOMove(new Vector3(37f, 0.63f, 24.75f), 0.1f).OnUpdate((() =>
            {
                _upgradeArea = true;
                walking = true;
            }))
            .OnComplete((() =>
            {
                transform.DORotate(new Vector3(0, -66, 0), 0.1f);
                _upgradeArea = true;
                walking = false;
            }));
            
        }
        else
        {
            walking = false;
            _upgradeArea = false;
        }
        
    }

    public void OnPlayerTeleport(bool teleport)
    {
        CameraChanger(0);
        _teleporting = teleport;
    }

    void FixedUpdate()
    {
        if (joystick.isActiveAndEnabled && _virtualCamera != null && !_upgradeArea && !_teleporting && !playerDie)
        {
            Movement();
        }
        else
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }

    private void Update()
    {
      
        _playerHealthValue = GameManager.current._playerArmor;
        if (Math.Abs(_playerHealthValue - playerHealthValueCurrent) > .1f )
        {
            if (_upgradeArea)
            {
                healthBar.SetActive(false);
            }
            else
            {
                healthBar.SetActive(true);
            }
        }
        else
        {
            healthBar.SetActive(false);
        }
        
        if (_gethit)
        {
            if (playerHealthValueCurrent>=1)
            {
                playerHealthValueCurrent = Mathf.Lerp(playerHealthValueCurrent, _playerHealthValueCurrentTemp, .1f);
                currentHealth.text = playerHealthValueCurrent.ToString("0");
                sliderValue.fillAmount = (1 * (playerHealthValueCurrent / _playerHealthValue));
            }
            else
            {
                rb.isKinematic = true;
                foreach (var variCollider in colliders)
                {
                    variCollider.enabled = false;
                }
                playerCollisionHandler.enabled = false;
                playerHealthValueCurrent = 0;
                foreach (var close in closePart)
                {
                    close.SetActive(false);
                }
                playerDie = true;
            }
        }

        if (healing)
        {
            if (playerHealthValueCurrent < _playerHealthValue)
            {
                playerHealthValueCurrent = Mathf.Lerp(playerHealthValueCurrent, _playerHealthValueCurrentTemp, .1f);
                sliderValue.fillAmount = (1 * (playerHealthValueCurrent / _playerHealthValue));
            }
        }
    }

    public void CameraChanger(int cam)
    {
        _virtualCamera = virtualCameras[0];
    }

    private void Movement()
    {
        Vector3 inputVector = new Vector3(InputController.Joystick.x, 0f, InputController.Joystick.y);

        PlayerSpeed = GameManager.current.playerSpeed * inputVector.magnitude;
        //PlayerSpeed = speed + inputVector.magnitude;
        if (inputVector != Vector3.zero)
        {
            var cameraTransform = _virtualCamera.transform;
            var forward = cameraTransform.forward;
            var cameraForwardHorizontal = new Vector3(forward.x, 0f, forward.z).normalized;

            var right = cameraTransform.right;
            var cameraRightHorizontal = new Vector3(right.x, 0f, right.z).normalized;

            var movementVector = inputVector.x * cameraRightHorizontal + inputVector.z * cameraForwardHorizontal;
            
            var dot = Mathf.Clamp(Vector3.Dot(transform.forward, inputVector),0.3f,1);

            _currentMoveMultiplier = Mathf.Lerp(_currentMoveMultiplier, dot, acceleration * Time.fixedDeltaTime);
        
            var position = transform.position + transform.forward.normalized * (PlayerSpeed * dot * Time.fixedDeltaTime);
            
            //Vector3 temp = transform.position + transform.forward* (speed * Time.fixedDeltaTime);
            //transform.position = position;
            
            UnityEngine.AI.NavMeshHit hit;
            bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(position, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            if (isvalid)
            {
                transform.position = position;
                walking = true;
            }
            else
            {
                walking = false;
            }
            
            var newrotation = Quaternion.LookRotation(movementVector);
            var lerp = Quaternion.Lerp(transform.rotation, newrotation, 2 * (PlayerSpeed * Time.fixedDeltaTime));
            transform.rotation = lerp;
            walking = true;
        }
        else
        {
            walking = false;
        }
    }
    
    

    public void HitTaken(float damage)
    {
        if (gameObject.activeInHierarchy && !playerDie)
        {
            _takenDamage = damage;
            _playerHealthValueCurrentTemp = Mathf.Clamp(playerHealthValueCurrent - _takenDamage, 0, _playerHealthValue);
            _gethit = true;
            damageTake.DOKill();
            damageTake.DOFade(.2f, .5f).OnComplete((() =>
            {
                damageTake.DOFade(0, 1f);
            }));
            _virtualCamera.GetComponent<CinemachineShake>().ShakeCamera(.7f,.1f);
        }
    }

    public void Healing(float heal , bool healArea)
    {
        if (gameObject.activeInHierarchy)
        {
            if (Math.Abs(_playerHealthValue - playerHealthValueCurrent)> .1f)
            {
                _tempHeal = heal;
                _playerHealthValueCurrentTemp = Mathf.Clamp(playerHealthValueCurrent + _tempHeal, 0, _playerHealthValue);
                healing = healArea;
            }
            else
            {
                healing = false;
            }
        }
    }

    
}
