using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI fullHealth;
    [SerializeField] private TextMeshProUGUI currentHealth;
    [SerializeField] private Image sliderValue;
    public GameObject[] closePart;
    private float _playerHealthValue;
    public float playerHealthValueCurrent;
    private float _playerHealthValueCurrentTemp;
    private float _tempDamage;
    private float _tempHeal;

    public bool walking = false;
    private bool _upgradeArea = false;
    private bool _teleporting = false;
    private bool _gethit;
    public bool _healing = false;

    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }

    private void Start()
    {
        _playerHealthValue = GameManager.current._playerArmor;
        playerHealthValueCurrent = _playerHealthValue;
        fullHealth.text = (Mathf.Round(_playerHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(playerHealthValueCurrent)).ToString();
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
        DOTween.Init();
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
            // transform.DOMove(new Vector3(24.0f, 0.63f, 26.0f), 1.0f)
            //     .OnUpdate((() =>
            //     {
            //         upgradeArea = true;
            //     }))
            //     .OnComplete((() =>
            //     {
            //         upgradeArea = false;
            //         walking = false;
            //     }));
            
        }
        
    }

    public void OnPlayerTeleport(bool teleport)
    {
        _teleporting = teleport;
    }

    void FixedUpdate()
    {
        if (joystick.isActiveAndEnabled && _virtualCamera != null && !_upgradeArea && !_teleporting)
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
                currentHealth.text = playerHealthValueCurrent.ToString("00");
                sliderValue.fillAmount = (1 * (playerHealthValueCurrent / _playerHealthValue));
            }
            else
            {
                var collider = GetComponent<Collider>();
                collider.isTrigger = false;
                for (int i = 0; i < closePart.Length; i++)
                {
                    closePart[i].SetActive(true);
                }
            }
        }

        if (_healing)
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
        // if (cam == 3)
        // {
        //     _virtualCamera = virtualCameras[cam];
        // }
        // else
        // {
        //     _virtualCamera = virtualCameras[cam];
        // }
    }

    private void Movement()
    {
        var speed = GameManager.current.playerSpeed;
        Vector3 inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        inputVector = Vector3.ClampMagnitude(inputVector, speed);
        if (inputVector != Vector3.zero)
        {
            var cameraTransform = _virtualCamera.transform;
            var forward = cameraTransform.forward;
            var cameraForwardHorizontal = new Vector3(forward.x, 0f, forward.z).normalized;

            var right = cameraTransform.right;
            var cameraRightHorizontal = new Vector3(right.x, 0f, right.z).normalized;

            var movementVector = inputVector.x * cameraRightHorizontal + inputVector.z * cameraForwardHorizontal;
            
            Vector3 temp = transform.position + transform.forward* (speed * Time.fixedDeltaTime);
            /* UnityEngine.AI.NavMeshHit hit;
            // bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(temp, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            // if (isvalid)
            // {
            //     if ((transform.position - hit.position).magnitude >= 0.1f)
            //     {
            //         transform.position = temp;
            //         var lerp = Vector3.Lerp(transform.forward, movementVector, speed * Time.fixedDeltaTime);
            //         transform.rotation = Quaternion.LookRotation(lerp);
            //         walking = true;
            //     }
            // }
            // else
            // {
            //     walking = false;
             }*/
            transform.position = temp;
            var lerp = Vector3.Lerp(transform.forward, movementVector, (speed *1.1f)* Time.fixedDeltaTime);
            transform.rotation = Quaternion.LookRotation(lerp);
            walking = true;
        }
        else
        {
            walking = false;
        }
    }

    public void Hit(float damage)
    {
        if (gameObject.activeInHierarchy)
        {
            _tempDamage = damage;
            _playerHealthValueCurrentTemp = Mathf.Clamp(playerHealthValueCurrent - _tempDamage, 0, _playerHealthValue);
            _gethit = true;
        }
    }

    public void Healing(float heal , bool healArea)
    {
        if (gameObject.activeInHierarchy)
        {
            if (healArea)
            {
                _tempHeal = heal;
                _playerHealthValueCurrentTemp = Mathf.Clamp(playerHealthValueCurrent + _tempHeal, 0, _playerHealthValue);
                _healing = healArea;
            }
            else
            {
                _healing = false;
            }
        }
    }
}
