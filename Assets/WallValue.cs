using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallValue : MonoBehaviour
{
    public int unlockRequire;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Animator wallAnimator;
    [SerializeField] private GameObject ballPrefab;
    private PlayerCollisionHandler _playerFollowerList;
    private PlayerBallCounter _playerBallCounter;
    private BallController _ballController;
    

    [SerializeField]
    List<GameObject> _2 = new List<GameObject>(); 
    [SerializeField]
    List<GameObject> _4 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _8 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _16 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _32 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _64 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _128 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _256 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _512 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _1024 = new List<GameObject>();     
    [SerializeField]
    List<GameObject> _2048 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _4096 = new List<GameObject>();
    
    private int _totalValue;
    private bool _calculate = false;
    private bool unlockWall;
    private bool _playerWallArea = false;
    private int _dicreaseValue;
    private int j;
    

    private void Start()
    {
        _ballController = GameObject.FindWithTag("BallController").GetComponent<BallController>();
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
    }

    public void UnlockCalculate(bool unlock)
    {
        var playerValue = _playerBallCounter;
        var delay = _ballController.balls.Count;
        if (unlock && playerValue.stackValue >= unlockRequire)
        {
            playerValue.stackValue -= unlockRequire;
            _uiManager.LevelUnlockPanel(false);
            GameEventHandler.current.BallWallArea(true);
            _ballController.GoUnlock(transform);
            StartCoroutine(Delay(delay));
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

    private void Update()
    {
        if (_calculate)
        {
            TotalValue();
        }
        else if (unlockWall)
        {
            TotalValue();
        }
    }

    public void UnlockComplete(bool resume)
    {
        _calculate = resume;
    }

    void TotalValue()
     {
         if (_totalValue >=4096)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(4096);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 4096;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 2048)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(2048);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 2048;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 1024)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(1024);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 1024;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 512)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(512);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 512;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 256)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(256);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 256;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 128)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(128);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 128;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 64)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(64);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 64;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 32)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(32);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 32;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 16)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(16);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 16;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else if (_totalValue >= 8)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(8);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 8;
                 _ballController.SetNewBall(go);
                 j++;
             }

         }
         else if (_totalValue >= 4)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(4);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 4;
                 _ballController.SetNewBall(go);
                 j++;
             }

         }
         else if (_totalValue >= 2)
         {
             if (j >2)
             {
                 j = 0;
             }

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab,
                     new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.gameObject.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(2);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= 2;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         else
         {
             if (unlockWall)
             {
                 gameObject.SetActive(true);
             }
             _calculate = false;
             _totalValue = 0;
         }
     }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_playerWallArea)
            {
                _playerWallArea = true;
                _playerFollowerList = other.GetComponent<PlayerCollisionHandler>();
                _playerBallCounter = other.GetComponent<PlayerBallCounter>();
                _playerBallCounter.BallCountCheck();
                if (_playerBallCounter.stackValue >= unlockRequire)
                {
                    GameEventHandler.current.PlayerLevelUnlockArea(true);
                }
            }
        }
        
        if (other.CompareTag("UnlockBall"))
        {
            float tempvalue = other.GetComponent<Ball>().GetValue();
            Destroy(other.gameObject);
            if (tempvalue==2)
            {
                _totalValue += 2;
                _2.Add(other.gameObject);
               
            }
            else if (tempvalue == 4)
            {
                _totalValue += 4;
                _4.Add(other.gameObject);
               
            }
            else if (tempvalue == 8)
            {
                _totalValue += 8;
                _8.Add(other.gameObject);
            }
            else if (tempvalue == 16)
            {
                _totalValue += 16;
                _16.Add(other.gameObject);
            }
            else if (tempvalue == 32)
            {
                _totalValue += 32;
                _32.Add(other.gameObject);
            }
            else if (tempvalue == 64)
            {
                _totalValue += 64;
                _64.Add(other.gameObject);
            }
            else if (tempvalue == 128)
            {
                _totalValue += 128;
                _128.Add(other.gameObject);
            }
            else if (tempvalue == 256)
            {
                _totalValue += 256;
                _256.Add(other.gameObject);
            }
            else if (tempvalue == 512)
            {
                _totalValue += 512;
                _512.Add(other.gameObject);
            }
            else if (tempvalue == 1024)
            {
                _totalValue += 1022;
                _1024.Add(other.gameObject);
            }
            else if (tempvalue == 2048)
            {
                _totalValue += 2048;
                _2048.Add(other.gameObject);
            }
            else if (tempvalue == 4096)
            {
                _totalValue += 4096;
                _4096.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playerWallArea)
            {
                _playerWallArea = false;
                GameEventHandler.current.PlayerLevelUnlockArea(false);
            }
        }
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.DOMoveY(-6f, 2.0f).SetEase(Ease.InBounce).
                            OnComplete((() =>
                            {
                                _totalValue = Mathf.Clamp(_totalValue - unlockRequire, 0, 4096);
                                GetComponent<Collider>().enabled = false;
                                unlockWall = true;
                                gameObject.GetComponent<MeshRenderer>().enabled = false;
                            }));
    }
}
