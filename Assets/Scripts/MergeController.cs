using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    [SerializeField] private TutorialStage tutorialStage;
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject _ballSpawn;
    [SerializeField] private GameObject _Machine;
    [SerializeField] private Animator machineAnimator;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private BallController _ballController;
    
    private float _TempTime;
    private int _IsDone;
    public int totalValue;
    private int j;
    
    public bool tutorial;

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
            
            if (tutorial)
            {
                tutorial = false;
                TutorialControl.Instance.CompleteStage(tutorialStage);
            }
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
         var value = 4096;
         while (totalValue>0)
         {
             
             if(value>totalValue)
             {
                 value /= 2;
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
                 var go = Instantiate(this.ball, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3), Quaternion.identity,
                     _ballController.transform);
                 go.tag = "StackBall";
                 var ball = go.GetComponent<Ball>();
                 ball.SetValue(value);
                 follower.SaveBall(ball.gameObject);
                 ball.SetGoTarget(last.transform);
                 ball.StartDelay();
                 totalValue -= value;
                 _ballController.SetNewBall(go);
                 j++;
             }
         }
         if (totalValue == 0)
         {
             machineAnimator.SetBool("vibration" , false);
         }
         
         // if (_TotalValue >= 2)
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
