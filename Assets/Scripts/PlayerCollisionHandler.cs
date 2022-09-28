using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponsHit weaponsHit;
    [SerializeField] private BallController ballController;
    public List<FollowerList> playerFollowPoints;
    private ChestController _chestController;
    private PlayerBallCounter _playerBallCounter;
    private Vector3 _lastposition;
    
    private bool _onMergeMachine = false;
    private bool _hit = false;
    private bool upgradeArea = false;
    private int i = 0;

    private void Start()
    {
        _playerBallCounter = playerController.GetComponent<PlayerBallCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseRight"))
        {
            GameEventHandler.current.PlayerRightArea(true);
            playerController.CameraChanger(1);
        }
        else if (other.CompareTag("BaseLeft"))
        {
            GameEventHandler.current.PlayerLeftArea(true);
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
                    GameEventHandler.current.BallMergeArea(true,null);
                }
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            ballController.GoUpgrade();
            GameEventHandler.current.PlayerUpgradeArea(true);
            if (!upgradeArea)
            {
                upgradeArea = true;
                _playerBallCounter.BallCountCheck();
                playerController.CameraChanger(3);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EmptyBall"))
        {
            if (i <= 2)
            {
                var lastObje = playerFollowPoints[i].ReturnLast();
                if (other.gameObject.GetComponent<Ball>() != null)
                {
                    other.gameObject.GetComponent<Ball>().SetGoTarget(lastObje.transform);
                }
                playerFollowPoints[i].SaveBall(other.transform.gameObject);
                ballController.SetNewBall(other.gameObject);
                other.tag = "StackBall";
                i++;
            }
            else
            {
                i = 0;
            }
        }
        
        if (other.gameObject.CompareTag("Chest") && !_hit)
        {
            _hit = true;
            _chestController= other.GetComponent<ChestController>();
            GameEventHandler.current.PlayerHit(true);
            StartCoroutine(HitDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseRight"))
        {
            GameEventHandler.current.PlayerRightArea(false);
            playerController.CameraChanger(0);
        }

        if (other.CompareTag("BaseLeft"))
        {
            GameEventHandler.current.PlayerLeftArea(false);
            playerController.CameraChanger(0);
        }
        

        if (other.gameObject.CompareTag("Chest"))
        {
            _chestController = null;
            GameEventHandler.current.PlayerHit(false);
        }

        if (other.CompareTag("MergeMachine"))
        {
            _onMergeMachine = false;
            //GameEventHandler.current.BallMergeArea(false,null);
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            upgradeArea = false;
            //GameEventHandler.current.BallUpgradeArea(false,null);
            GameEventHandler.current.PlayerUpgradeArea(false);
            playerController.CameraChanger(0);
            _playerBallCounter.stackValue = 0;
        }
        
    }

    public void ExitUpgrade()
    {
        GameEventHandler.current.PlayerUpgradeArea(false);
    }
    
    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _hit = false;
    }

    private void WeaponHit()
    {
        if (_chestController != null)
        {
            _chestController.Hit(weaponsHit._damageValue);
        }
        GameEventHandler.current.PlayerHit(false);
    }
}
