using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BallController ballController;
    [SerializeField] private Image filledImage;
    private ChestController _chestController;
    public List<FollowerList> playerFollowPoints;

    private PlayerBallCounter _playerBallCounter;
    private Vector3 _lastposition;

    private Coroutine _triggerDelay;
    private Coroutine _mergeTriggerDelay;
    
    private bool _onMergeMachine = false;
    private bool _upgradeArea = false;
    private bool _unlockArea = false;
    private int i = 0;
    [SerializeField] private PlayerAnimationHandler animationHandler;


    private void OnEnable()
    {
        _playerBallCounter = playerController.GetComponent<PlayerBallCounter>();
        
    }

    private void Update()
    {
        // if (_chestController != null)
        // {
        //     if (_chestController.chestHealthCurrent < 1)
        //     {
        //         animationHandler.HitAnimation(false);
        //         _chestController = null;
        //     }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            if (other.CompareTag("BaseRight"))
            {
                gameEventHandler.PlayerRightArea(true);
                playerController.CameraChanger(1);
            }
            else if (other.CompareTag("BaseLeft"))
            {
                gameEventHandler.PlayerLeftArea(true);
                playerController.CameraChanger(2);
            }
            else
            {
                playerController.CameraChanger(0);
            }

            if (other.CompareTag("MergeMachine"))
            {
                if (!_onMergeMachine)
                {
                    _onMergeMachine = true;
                    if (ballController.balls.Count > 1)
                    {
                        filledImage = other.GetComponent<TriggerArea>().filledImage;
                        _mergeTriggerDelay = StartCoroutine(MergeAreaDelay(true));

                        //GameEventHandler.current.PlayerMergeArea(true);
                    }
                }
            }
        
            if (other.CompareTag("UpgradeTrigger"))
            {
                if (!_upgradeArea)
                {
                    //_playerBallCounter.BallCountCheck();
                    filledImage = other.GetComponent<TriggerArea>().filledImage;
                    _triggerDelay = StartCoroutine(UpgradeAreaDelay(true));
                }
            }
        
            if (other.CompareTag("EmptyBall"))
            {
                if (i>2)
                {
                    i = 0;
                }
                if (i < 3)
                {
                    other.tag = "StackBall";
                    other.gameObject.GetComponent<Ball>().SetGoTarget(playerFollowPoints[i].ReturnLast().transform);
                    playerFollowPoints[i].SaveBall(other.gameObject);
                    i++;
                    ballController.SetNewBall(other.gameObject);
                }
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.CompareTag("Chest"))
        // {
        //     _chestController= other.GetComponent<ChestController>();
        //     animationHandler.HitAnimation(true);
        // }
    }
        


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseRight"))
        {
            gameEventHandler.PlayerRightArea(false);
            playerController.CameraChanger(0);
        }
        else if (other.CompareTag("BaseLeft"))
        {
            gameEventHandler.PlayerLeftArea(false);
            playerController.CameraChanger(0);
        }
        else if (other.CompareTag("LevelWall"))
        {
            //gameEventHandler.PlayerLevelUnlockArea(false);
            _playerBallCounter.stackValue = 0;
            _unlockArea = false;
        }
        if (other.CompareTag("MergeMachine"))
        {
            if (filledImage != null)
                filledImage.fillAmount = 0;
            
            if (_mergeTriggerDelay != null)
            {
                StopCoroutine(_mergeTriggerDelay);
            }
            filledImage.DOKill();
            filledImage = null;
            _onMergeMachine = false;
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            if (filledImage != null)
                filledImage.fillAmount = 0;
            
            _playerBallCounter.stackValue = 0;
            if (_triggerDelay != null)
            {
                StopCoroutine(_triggerDelay);
            }
            filledImage.DOKill();
            filledImage = null;
            _upgradeArea = false;
        }
        
    }

    public void ExitUpgrade()
    {
        _playerBallCounter.stackValue = 0;
        _upgradeArea = false;
        if (_triggerDelay != null)
        {
            StopCoroutine(_triggerDelay);
        }
        gameEventHandler.PlayerUpgradeArea(false);
        playerController.CameraChanger(0);
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

    IEnumerator UpgradeAreaDelay(bool inOut)
    {
        _upgradeArea = inOut;
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.7f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            ballController.GoUpgrade();
            playerController.CameraChanger(3);
            gameEventHandler.PlayerUpgradeArea(true);
        }));
    }
    
    private void WeaponDamage()
    {
        var playerDamage = GameManager.current.playerDamage;
        GameEventHandler.current.PlayerAnimationHit(playerDamage);
    }
}
