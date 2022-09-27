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
                    GameEventHandler.current.BallMergeArea(true);
                }
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            var playerpos = playerController.gameObject.transform.position;
            _lastposition = new Vector3(playerpos.x, playerpos.y, playerpos.z);
            ballController.GoUpgrade();
            if (!upgradeArea)
            {
                upgradeArea = true;
                _playerBallCounter.BallCountCheck();
            }
            GameEventHandler.current.PlayerUpgradeArea(true,_playerBallCounter.stackValue);
            playerController.gameObject.transform.position = new Vector3(36.95f, 0.63f, 24.75f);
            playerController.gameObject.transform.rotation = Quaternion.Euler(0,-66,0);
            playerController.CameraChanger(5);
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
            GameEventHandler.current.BallMergeArea(false);
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            upgradeArea = false;
            GameEventHandler.current.BallUpgradeArea(false);
        }
        
    }

    public void ExitUpgrade()
    {
        GameEventHandler.current.PlayerUpgradeArea(false,0);
        _playerBallCounter.stackValue = 0;
        playerController.gameObject.transform.position = new Vector3(_lastposition.x, _lastposition.y, _lastposition.z);
        playerController.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        playerController.CameraChanger(0);
        
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
