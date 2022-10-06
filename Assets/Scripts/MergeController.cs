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
    private float _TempTime;
    private int _IsDone;
    public int _TotalValue;
    public bool _MergeTime;

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
        if (_TotalValue != 0&& GameObject.FindGameObjectsWithTag("MergeBall").Length<1)
        {
            SetMerge();
        }
    }
    private void OnBallMergeArea(bool enterExit)
    {
        machineAnimator.SetBool("vibration" , true);
    }

    public void SetMerge()
    {
        _MergeTime = true;
        TotalValue();
    }
     void TotalValue()
     {
         _IsDone = 0;
         if (_TotalValue >=4096)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(4096);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 4096;
             _IsDone = 1;
         }
         else if (_TotalValue >= 2048)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(2048);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 2048;
             _IsDone = 1;
         }
         else if (_TotalValue >= 1024)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(1024);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 1024;
             _IsDone = 1;
         }
         else if (_TotalValue >= 512)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(512);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 512;
             _IsDone= 1;
         }
         else if (_TotalValue >= 256)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(256);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 256;
             _IsDone = 1;
         }
         else if (_TotalValue >= 128)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(128);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 128;
             _IsDone = 1;
         }
         else if (_TotalValue >= 64)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(64);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 64;
             _IsDone = 1;
         }
         else if (_TotalValue >= 32)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(32);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 32;
             _IsDone = 1;
         }
         else if (_TotalValue >= 16)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(16);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 16;
             _IsDone = 1;
         }
         else if (_TotalValue >= 8)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(8);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 8;
             _IsDone = 1;
         }
         else if (_TotalValue >= 4)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(4);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 4;
             _IsDone = 1;
         }
         else if (_TotalValue >= 2)
         {
             GameObject go = Instantiate(_Ball, _ballSpawn.transform.position, _ballSpawn.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(2);
             ball.ballRb.AddForce(Vector3.left*3,ForceMode.Impulse);
             ball.agent.enabled = false;
             _TotalValue -= 2;
             _IsDone = 1;
         }
         else
         {
             _TotalValue = 0;
             _IsDone = 0;
         }
         if (_IsDone <= 0)
         {
             _MergeTime = false;
             machineAnimator.SetBool("vibration" , false);
         }
     }
     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MergeBall"))
        {
            float tempvalue = other.GetComponent<Ball>().GetValue();
         
            Destroy(other.gameObject);
            if (tempvalue==2)
            {
                _TotalValue += 2;
                _2.Add(other.gameObject);
               
            }
            else if (tempvalue == 4)
            {
                _TotalValue += 4;
                _4.Add(other.gameObject);
               
            }
            else if (tempvalue == 8)
            {
                _TotalValue += 8;
                _8.Add(other.gameObject);
            }
            else if (tempvalue == 16)
            {
                _TotalValue += 16;
                _16.Add(other.gameObject);
            }
            else if (tempvalue == 32)
            {
                _TotalValue += 32;
                _32.Add(other.gameObject);
            }
            else if (tempvalue == 64)
            {
                _TotalValue += 64;
                _64.Add(other.gameObject);
            }
            else if (tempvalue == 128)
            {
                _TotalValue += 128;
                _128.Add(other.gameObject);
            }
            else if (tempvalue == 256)
            {
                _TotalValue += 256;
                _256.Add(other.gameObject);
            }
            else if (tempvalue == 512)
            {
                _TotalValue += 512;
                _512.Add(other.gameObject);
            }
            else if (tempvalue == 1024)
            {
                _TotalValue += 1022;
                _1024.Add(other.gameObject);
            }
            else if (tempvalue == 2048)
            {
                _TotalValue += 2048;
                _2048.Add(other.gameObject);
            }
            else if (tempvalue == 4096)
            {
                _TotalValue += 4096;
                _4096.Add(other.gameObject);
            }
        }
    }
}
