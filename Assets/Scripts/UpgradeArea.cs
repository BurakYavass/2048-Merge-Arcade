using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameObject blackSmith;
    [SerializeField] private Animator blackSmithAnimator;
    [SerializeField] private GameObject ballPrefab;

    public int _totalValue;
    private bool _calculate = false;
    private int _dicreaseValue;
    [SerializeField] private Animator upgradeAnimator;
    private int j;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private BallController _ballController;

    public void UpgradeCalculate(int value)
    {
        _dicreaseValue = value;
        _totalValue = Mathf.Clamp(_totalValue - _dicreaseValue, 0, 4096);
        blackSmithAnimator.SetBool("working",true);
    }

    private void Update()
    {
        if (_calculate)
        {
            TotalValue();
        }
    }

    public void UpgradeComplete(bool resume)
    {
        _calculate = resume;
        blackSmithAnimator.SetBool("working", false);
    }

    void TotalValue()
     {
         
         if (_totalValue == 0)
         {
             blackSmithAnimator.SetBool("working", false);
             _calculate = false;
             return;
         }
         var sayi = 4096;
         while (_totalValue>0)
         {
             if(sayi>_totalValue)
             {
                 sayi /= 2;
                 continue;
             }
             
             if (j >2)
                j = 0;

             if (j <= 2)
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(ballPrefab, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(sayi);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 _totalValue -= sayi;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         
         // if (_totalValue >=4096)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(4096);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 4096;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 2048)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(2048);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 2048;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 1024)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(1024);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 1024;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 512)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(512);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 512;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 256)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(256);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 256;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 128)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(128);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 128;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 64)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(64);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 64;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 32)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(32);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 32;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 16)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(16);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 16;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else if (_totalValue >= 8)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(8);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 8;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         //
         // }
         // else if (_totalValue >= 4)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(4);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 4;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         //
         // }
         // else if (_totalValue >= 2)
         // {
         //     if (j >2)
         //     {
         //         j = 0;
         //     }
         //
         //     if (j <= 2)
         //     {
         //         var follower = _playerFollowerList.playerFollowPoints[j];
         //         var last = follower.ReturnLast();
         //         var lastPosition = last.transform.position;
         //         GameObject go = Instantiate(ballPrefab,
         //             new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
         //             _ballController.gameObject.transform);
         //         go.tag = "StackBall";
         //         var ball = go.GetComponent<Ball>();
         //         ball.SetValue(2);
         //         follower.SaveBall(ball.gameObject);
         //         ball.SetGoTarget(last.transform);
         //         _totalValue -= 2;
         //         _ballController.SetNewBall(go);
         //         j++;
         //     }
         // }
         // else
         // {
         //     _calculate = false;
         //     _totalValue = 0;
         // }
     }
    
}
