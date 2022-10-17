using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private BallController ballController;
    private ChestController _chestController;
    public List<FollowerList> playerFollowPoints;

    private PlayerBallCounter _playerBallCounter;

    private bool _active = false;
    private bool _unlockArea = false;
    private int i = 0;
    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        _playerBallCounter = GetComponent<PlayerBallCounter>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        _active = true;
    }

    private void OnDisable()
    {
        _active = false;
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
        if (_active)
        {
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
        if (!other.CompareTag("BaseLeft")&& !other.CompareTag("BaseRight"))
        {
            playerController.CameraChanger(0);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LevelWall"))
        {
            //gameEventHandler.PlayerLevelUnlockArea(false);
            _playerBallCounter.stackValue = 0;
            _unlockArea = false;
        }

    }
    
    
    private void WeaponDamage()
    {
        var playerDamage = GameManager.current.playerDamage;
        GameEventHandler.current.PlayerAnimationHit(playerDamage);
    }
}
