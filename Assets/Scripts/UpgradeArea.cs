using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameObject blackSmith;
    [SerializeField] private Animator blackSmithAnimator;
    [SerializeField] private Transform ballSpawnPosition;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Collider ballFireCollider;

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
    
    private float _tempTime;
    private int _totalValue;
    private bool _mergeTime;
    private bool _calculate = false;
    private int _dicreaseValue;
    [SerializeField] private Animator upgradeAnimator;

    private void Start()
    {
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
    }

    public void UpgradeCalculate(int value)
    {
        _dicreaseValue = value;
        _totalValue = Mathf.Clamp(_totalValue - _dicreaseValue, 0, 4096);
        _calculate = true;
    }

    private void Update()
    {
        if (_calculate && _totalValue>1)
        {
            SetUpgrade();
        }
    }
    
    private void OnPlayerUpgradeArea(bool working, int value)
    {
        blackSmithAnimator.SetBool("working",working);
        if (working == false)
        {
            UpgradeCalculate(0);
        }
        else
        {
            _totalValue = value;
        }
    }

    public void SetUpgrade()
    {
        Debug.Log(_totalValue);
        if (_totalValue >1)
        {
            TotalValue();
        }
    }

    void TotalValue()
     {
         if (_totalValue >=4096)
         {
             GameObject go = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(4096);
             ball.agent.enabled = false;
             _totalValue -= 4096;
         }
         else if (_totalValue >= 2048)
         {
             GameObject go = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(2048);
             ball.agent.enabled = false;
             _totalValue -= 2048;
         }
         else if (_totalValue >= 1024)
         {
             GameObject go = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(1024);
             ball.agent.enabled = false;
             _totalValue -= 1024;
         }
         else if (_totalValue >= 512)
         {
             GameObject go = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(512);
             ball.agent.enabled = false;
             _totalValue -= 512;
         }
         else if (_totalValue >= 256)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(256);
             ball.agent.enabled = false;
             _totalValue -= 256;
         }
         else if (_totalValue >= 128)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(128);
             ball.agent.enabled = false;
             _totalValue -= 128;
         }
         else if (_totalValue >= 64)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(64);
             ball.agent.enabled = false;
             _totalValue -= 64;
         }
         else if (_totalValue >= 32)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(32);
             ball.agent.enabled = false;
             _totalValue -= 32;
         }
         else if (_totalValue >= 16)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, Quaternion.identity);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(16);
             ball.agent.enabled = false;
             _totalValue -= 16;
         }
         else if (_totalValue >= 8)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, ballSpawnPosition.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(8);
             ball.agent.enabled = false;
             _totalValue -= 8;
         }
         else if (_totalValue >= 4)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, ballSpawnPosition.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(4);
             ball.agent.enabled = false;
             _totalValue -= 4;
         }
         else if (_totalValue >= 2)
         {
             GameObject go = Instantiate(this.ballPrefab, ballSpawnPosition.position, ballSpawnPosition.transform.localRotation);
             var ball = go.GetComponent<Ball>();
             ball.SetValue(2);
             ball.agent.enabled = false;
             _totalValue -= 2;
         }
         else
         {
             _calculate = false;
             _totalValue = 0;
         }
     }

    

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("UpgradeBall"))
    //     {
    //         float tempvalue = other.GetComponent<Ball>().GetValue();
    //         Debug.Log(other.gameObject);
    //      
    //         Destroy(other.gameObject);
    //         if (tempvalue==2)
    //         {
    //             _totalValue += 2;
    //             _2.Add(other.gameObject);
    //            
    //         }
    //         else if (tempvalue == 4)
    //         {
    //             _totalValue += 4;
    //             _4.Add(other.gameObject);
    //            
    //         }
    //         else if (tempvalue == 8)
    //         {
    //             _totalValue += 8;
    //             _8.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 16)
    //         {
    //             _totalValue += 16;
    //             _16.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 32)
    //         {
    //             _totalValue += 32;
    //             _32.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 64)
    //         {
    //             _totalValue += 64;
    //             _64.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 128)
    //         {
    //             _totalValue += 128;
    //             _128.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 256)
    //         {
    //             _totalValue += 256;
    //             _256.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 512)
    //         {
    //             _totalValue += 512;
    //             _512.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 1024)
    //         {
    //             _totalValue += 1022;
    //             _1024.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 2048)
    //         {
    //             _totalValue += 2048;
    //             _2048.Add(other.gameObject);
    //         }
    //         else if (tempvalue == 4096)
    //         {
    //             _totalValue += 4096;
    //             _4096.Add(other.gameObject);
    //         }
    //
    //     }
    // }
}
