using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimationHandler animationHandler;
    [SerializeField] private WeaponsHit weaponsHit;
    [SerializeField] private BallController ballController;
    [SerializeField] private ChestController chestController;
    public List<FollowerList> playerFollowPoints;

    private PlayerBallCounter _playerBallCounter;
    private Vector3 _lastposition;
    
    private bool _onMergeMachine = false;
    private bool _hit = false;
    private bool _upgradeArea = false;
    private bool _unlockArea = false;
    private int i = 0;

    private void Start()
    {
        _playerBallCounter = playerController.GetComponent<PlayerBallCounter>();
    }

    private void Update()
    {
        if (chestController != null)
        {
            if (chestController._ChestHealthValueCurrent < 1)
            {
                animationHandler.CurrentPlayerHit(false);
                chestController = null;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
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
                    ballController.GoMerge();
                    //GameEventHandler.current.PlayerMergeArea(true);
                }
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            //StartCoroutine(UpgradeDelay(true));
            ballController.GoUpgrade();
            playerController.CameraChanger(3);
            gameEventHandler.PlayerUpgradeArea(true);
            if (!_upgradeArea)
            {
                _upgradeArea = true;
                _playerBallCounter.BallCountCheck();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            chestController= other.GetComponent<ChestController>();
            if (!_hit)
            {
                _hit = true;
                StartCoroutine(HitDelay());
            }
            animationHandler.CurrentPlayerHit(true);
        }
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
        
        if (other.gameObject.CompareTag("Chest"))
        {
            chestController = null;
            animationHandler.CurrentPlayerHit(false);
            //GameEventHandler.current.PlayerHit(false);
        }

        if (other.CompareTag("MergeMachine"))
        {
            _onMergeMachine = false;
            gameEventHandler.PlayerMergeArea(false);
            //GameEventHandler.current.BallMergeArea(false,null);
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            _upgradeArea = false;
            //GameEventHandler.current.BallUpgradeArea(false,null);
            //gameEventHandler.PlayerUpgradeArea(false);
            playerController.CameraChanger(0);
            _playerBallCounter.stackValue = 0;
        }
        
    }

    public void ExitUpgrade()
    {
        gameEventHandler.PlayerUpgradeArea(false);
    }
    
    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(1f);
        _hit = false;
    }
    
    private void WeaponHit()
    {
        if (chestController != null)
        {
            chestController.Hit(weaponsHit._damageValue);
            chestController.transform.parent.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce);
        }
        //animationHandler.CurrentPlayerHit(false);
    }
    
}
