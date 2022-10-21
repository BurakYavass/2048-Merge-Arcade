using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    [SerializeField] private GameObject _Ball;
    [SerializeField] private GameObject _ballSpawn;
    [SerializeField] private GameObject _Machine;
    [SerializeField] private Animator machineAnimator;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private BallController _ballController;
    
    private float _TempTime;
    private int _IsDone;
    public int totalValue;
    private int j;

    private void Start()
    {
        GameEventHandler.current.OnBallMergeArea += OnBallMergeArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnBallMergeArea -= OnBallMergeArea;
    }
    
    private void Update()
    {
        if (totalValue != 0 && GameObject.FindGameObjectsWithTag("MergeBall").Length<1)
        {
            TotalValue();
        }
    }
    private void OnBallMergeArea(bool enterExit)
    {
        machineAnimator.SetBool("vibration" , true);
    }
    
     void TotalValue()
     {
         if (totalValue == 0)
         {
             machineAnimator.SetBool("vibration" , false);
             return;
         }
         var sayi = 4096;
         while (totalValue>0)
         {
             
             if(sayi>totalValue)
             {
                 sayi /= 2;
                 continue;
             }

             if (j > 2)
             {
                 j = 0;
             }
             else
             {
                 var follower = _playerFollowerList.playerFollowPoints[j];
                 var last = follower.ReturnLast();
                 var lastPosition = last.transform.position;
                 GameObject go = Instantiate(_Ball, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(sayi);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 ball.StartDelay();
                 totalValue -= sayi;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         if (totalValue == 0)
         {
             machineAnimator.SetBool("vibration" , false);
         }
         // _IsDone = 0;
         // if (_TotalValue >=4096)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(4096);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 4096;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 2048)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(2048);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 2048;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 1024)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(1024);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 1024;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 512)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(512);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 512;
         //     _IsDone= 1;
         // }
         // else if (_TotalValue >= 256)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(256);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 256;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 128)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(128);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 128;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 64)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(64);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 64;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 32)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(32);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 32;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 16)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(16);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 16;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 8)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(8);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 8;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 4)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(4);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 4;
         //     _IsDone = 1;
         // }
         // else if (_TotalValue >= 2)
         // {
         //     GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
         //     var ball = go.GetComponent<Ball>();
         //     ball.SetValue(2);
         //     ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
         //     ball.agent.enabled = false;
         //     _TotalValue -= 2;
         //     _IsDone = 1;
         // }
         // else
         // {
         //     _TotalValue = 0;
         //     _IsDone = 0;
         //     machineAnimator.SetBool("vibration" , false);
         // }
     }
     private void OnTriggerEnter(Collider other)
    {
        //machineAnimator.SetBool("vibration" , true);
        if (other.CompareTag("MergeBall"))
        {
            int tempvalue = other.GetComponent<Ball>().GetValue();
            totalValue += tempvalue;
            Destroy(other.gameObject);
            
        }
    }
}
