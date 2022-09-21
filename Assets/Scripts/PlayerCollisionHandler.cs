using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponsHit weaponsHit;
    [SerializeField] private BallController ballController;
    private ChestController _chestController;
    private PlayerBallCounter _playerBallCounter;

    private bool _onUpgrade = false;
    private bool _onMergeMachine = false;
    private bool _hit = false;

    private void Start()
    {
        _playerBallCounter = playerController.GetComponent<PlayerBallCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EmptyBall"))
        {
            other.gameObject.GetComponent<Ball>().SetGoTarget(ballController.LastObje());
            other.tag = "StackBall";
        }

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
                if (ballController._Balls.Count > 2)
                {
                    ballController.GoMerge();
                    GameEventHandler.current.PlayerMergeArea(true);
                }
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            if (!_onUpgrade)
            {
                _onUpgrade = true;
                //ballController.GoUpgrade();
                _playerBallCounter.BallCountCheck();
                GameEventHandler.current.PlayerUpgradeArea(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
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
            //GameEventHandler.current.PlayerMergeArea(false);
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            _onUpgrade = false;
            GameEventHandler.current.PlayerUpgradeArea(false);
            _playerBallCounter.stackValue = 0;
        }
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
