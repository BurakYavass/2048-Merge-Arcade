using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimationHandler animationHandler;
    [SerializeField] private WeaponsHit weaponsHit;
    [SerializeField] private BallController ballController;
    public List<FollowerList> playerFollowPoints;
    public GameObject[] closePart;
    [SerializeField] private ChestController _chestController;
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
        if (_chestController != null)
        {
            if (_chestController._ChestHealthValueCurrent < 1)
            {
                animationHandler.CurrentPlayerHit(false);
                _chestController = null;
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
        if (other.CompareTag("BaseLeft"))
        {
            gameEventHandler.PlayerLeftArea(true);
            playerController.CameraChanger(2);
        }
        if (other.CompareTag("Ground"))
        {
            gameEventHandler.PlayerLeftArea(false);
            gameEventHandler.PlayerRightArea(false);
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
            ballController.GoUpgrade();
            gameEventHandler.PlayerUpgradeArea(true);
            if (!_upgradeArea)
            {
                _upgradeArea = true;
                _playerBallCounter.BallCountCheck();
                playerController.CameraChanger(3);
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
            _chestController= other.GetComponent<ChestController>();
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
        
        if (other.CompareTag("BaseLeft"))
        {
            gameEventHandler.PlayerLeftArea(false);
            playerController.CameraChanger(0);
        }

        if (other.CompareTag("LevelWall"))
        {
            //gameEventHandler.PlayerLevelUnlockArea(false);
            _playerBallCounter.stackValue = 0;
            _unlockArea = false;
        }
        
        if (other.gameObject.CompareTag("Chest"))
        {
            _chestController = null;
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
            gameEventHandler.PlayerUpgradeArea(false);
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
        if (_chestController != null)
        {
            _chestController.Hit(weaponsHit._damageValue);
            _chestController.transform.parent.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce);
        }
        //animationHandler.CurrentPlayerHit(false);
    }
}
