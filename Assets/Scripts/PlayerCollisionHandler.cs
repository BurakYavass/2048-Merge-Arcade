using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BallController ballController;
    [SerializeField] private TravelManager travelManager;
    private PlayerBallCounter _playerBallCounter;
    private ChestController _chestController;
    public List<FollowerList> playerFollowPoints;
    
    public bool inBossArea ;
    private bool _active = false;
    private int i = 0;
    

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
        NavMeshHit hit;
        var position = transform.position;
        bool inBase = NavMesh.SamplePosition(position, out hit, .3f, NavMesh.GetAreaFromName("Base"));
        //bool bossArea = NavMesh.SamplePosition(position, out hit, .3f, NavMesh.GetAreaFromName("BossArea"));
        if (!inBase)
        {
            UIManager.current.PlayerHomeTravelButton(false);
            
        }
        else
        {
            if (travelManager.homeButtonActiveNow)
            {
                UIManager.current.PlayerHomeTravelButton(true);
            }
        }
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

        if (other.CompareTag("BossArea"))
        {
            inBossArea = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LevelWall"))
        {
            //gameEventHandler.PlayerLevelUnlockArea(false);
            _playerBallCounter.stackValue = 0;
        }
        
        if (other.CompareTag("BossArea"))
        {
            inBossArea = false;
        }

    }
    
    
    private void WeaponDamage()
    {
        var playerDamage = GameManager.current.playerDamage;
        GameEventHandler.current.PlayerAnimationHit(playerDamage);
    }
}
