using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameObject blackSmith;
    [SerializeField] private Animator blackSmithAnimator;
    [SerializeField] private GameObject ballPrefab;

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
    }

    private void Update()
    {
        if (_calculate)
        {
            blackSmithAnimator.SetBool("working",true);
            TotalValue();
        }
    }

    public void UpgradeComplete(bool resume)
    {
        _calculate = resume;
    }

    void TotalValue()
     {
         blackSmithAnimator.SetBool("working",false);
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
             _calculate = false;
             _totalValue = 0;
         }
     }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpgradeBall"))
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
}
