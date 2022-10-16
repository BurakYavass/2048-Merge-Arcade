using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TriggerArea : MonoBehaviour
{
    public enum Area
    {
        Upgrade,
        Merge,
        BaseRight,
        BaseLeft,
    }

    public Area areaType;
    private PlayerController _playerController;
    [SerializeField] private Image filledImage;
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private BallController ballController;
    private PlayerBallCounter _playerBallCounter;
    private Coroutine _upgradeTriggerDelay;
    private Coroutine _mergeTriggerDelay;
    
    private bool _upgradeArea;
    private bool _mergeMachine;


    private void Awake()
    {
        //_playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (areaType)
        {
            case Area.Upgrade:
                break;
            case Area.Merge:
                break;
            case Area.BaseRight:
                break;
        }

        //playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //_playerBallCounter = _playerController.GetComponent<PlayerBallCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Area Type(Area type)
    // {
    //     type = type == Area.Merge ? Area.Merge : Area.Upgrade;
    //
    //     return type;
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            _playerBallCounter = _playerController.GetComponent<PlayerBallCounter>();
            if (areaType == Area.Upgrade)
            {
                if (!_upgradeArea)
                {
                    //_playerBallCounter.BallCountCheck();
                    _upgradeTriggerDelay = StartCoroutine(UpgradeAreaDelay(true));
                }
            }
            else if(areaType == Area.Merge)
            {
                if (!_mergeMachine)
                {
                    _mergeMachine = true;
                    if (ballController.balls.Count > 1)
                    {
                        _mergeTriggerDelay = StartCoroutine(MergeAreaDelay(true));
                            
                    }
                }
            }
            else if (areaType == Area.BaseRight)
            {
                gameEventHandler.PlayerRightArea(true);
                _playerController.CameraChanger(1);
            }
            else if (areaType == Area.BaseLeft)
            {
                gameEventHandler.PlayerLeftArea(true);
                _playerController.CameraChanger(2);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (areaType == Area.Merge)
            {
                filledImage.fillAmount = 0;
                if (_mergeTriggerDelay != null)
                {
                    StopCoroutine(_mergeTriggerDelay);
                }
                filledImage.DOKill();
                _mergeMachine = false;
            }
            else if (areaType == Area.Upgrade)
            {
                filledImage.fillAmount = 0;
                _playerBallCounter.stackValue = 0;
                StopCoroutine(_upgradeTriggerDelay);
                filledImage.DOKill();
                _upgradeArea = false;
            }
            else if (areaType == Area.BaseRight)
            {
                gameEventHandler.PlayerRightArea(false);
                _playerController.CameraChanger(0);
            }
            else if (areaType == Area.BaseLeft)
            {
                gameEventHandler.PlayerLeftArea(false);
                _playerController.CameraChanger(0);
            }
        }
    }
    
    public void ExitUpgrade()
    {
        _playerBallCounter.stackValue = 0;
        _upgradeArea = false;
        StopCoroutine(_upgradeTriggerDelay);
        gameEventHandler.PlayerUpgradeArea(false);
        _playerController.CameraChanger(0);
    }
    
    IEnumerator UpgradeAreaDelay(bool inOut)
    {
        _upgradeArea = inOut;
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.7f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            ballController.GoUpgrade();
            _playerController.CameraChanger(3);
            gameEventHandler.PlayerUpgradeArea(true);
        }));
    }
    
    IEnumerator MergeAreaDelay(bool inOut)
    {
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            filledImage.fillAmount = 0;
            ballController.GoMerge();
        }));
    }
}
