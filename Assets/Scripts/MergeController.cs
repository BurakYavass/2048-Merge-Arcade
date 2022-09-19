using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    [SerializeField] private GameObject _Ball;
    [SerializeField] private GameObject _BallSpawn;
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
    float _TempTime;
    int _IsDone;
    public int _TotalValue;
    public bool _MergeTime;

    private void Start()
    {
        GameEventHandler.current.OnPlayerMergeArea += OnPlayerMergeArea;
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerMergeArea -= OnPlayerMergeArea;
    }
    
    private void Update()
    {
       
        if (!_MergeTime)
        {
            if (_TotalValue != 0&& GameObject.FindGameObjectsWithTag("StackBall").Length<1)
            {
                SetMerge();
            }
        }
       
        _TempTime += Time.deltaTime;
        
        if (_IsDone!=0 && _MergeTime)
        {
            if (_TempTime>.2f)
            {
                CalculateMerge();
                //TotalValue();
                _TempTime = 0;
            }
        }
    }
    
    private void OnPlayerMergeArea(bool enterExit)
    {
        machineAnimator.SetBool("vibration" , true);
    }

    public void SetMerge()
    {
        _MergeTime = true;
        //TotalValue();
        CalculateMerge();
    }
    void TotalValue()
    {
        _IsDone = 0;
        if (_TotalValue >=4096)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(4096);
            _TotalValue -= 4096;
            _IsDone = 1;
        }
        else if (_TotalValue >= 2048)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(2048);
            _TotalValue -= 2048;
            _IsDone = 1;
        }
        else if (_TotalValue >= 1024)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(1024);
            _TotalValue -= 1024;
            _IsDone = 1;
        }
        else if (_TotalValue >= 512)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(512);
            _TotalValue -= 512;
            _IsDone= 1;
        }
        else if (_TotalValue >= 256)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(256);
            _TotalValue -= 256;
            _IsDone = 1;
        }
        else if (_TotalValue >= 128)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(128);
            _TotalValue -= 128;
            _IsDone = 1;
        }
        else if (_TotalValue >= 64)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(64);
            _TotalValue -= 64;
            _IsDone = 1;
        }
        else if (_TotalValue >= 32)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(32);
            _TotalValue -= 32;
            _IsDone = 1;
        }
        else if (_TotalValue >= 16)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(16);
            _TotalValue -= 16;
            _IsDone = 1;
        }
        else if (_TotalValue >= 8)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(8);
            _TotalValue -= 8;
            _IsDone = 1;
        }
        else if (_TotalValue >= 4)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(4);
            _TotalValue -= 4;
            _IsDone = 1;
        }
        else if (_TotalValue >= 2)
        {
            GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
            go.GetComponent<Ball>().SetValue(2);
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
        }
    }
    void CalculateMerge()
    {
        _IsDone = 0;
        if (_2.Count>0)
        {
            if (_2.Count>=2)
            {
                 _2.RemoveAt(_2.Count-1);
                 _2.RemoveAt(_2.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(4);
                _IsDone += 1;
            }
            else if (_2.Count == 1)
            {
                _2.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(2);
                _IsDone += 1;
            }                
        } 
        
        else  if (_4.Count>0)
        {
            if (_4.Count>=2)
            {
                 _4.RemoveAt(_4.Count-1);
                 _4.RemoveAt(_4.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(8);
                _IsDone += 1;
            }
            else if (_4.Count == 1)
            {
                _4.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(4);
                _IsDone += 1;
            }                
        }

        else if (_8.Count>0)
        {
            if (_8.Count>=2)
            {
                 _8.RemoveAt(_8.Count-1);
                 _8.RemoveAt(_8.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(16);
                _IsDone += 1;
            }
            else if (_8.Count == 1)
            {
                _8.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(8);
                _IsDone += 1;
            }                
        }

        else if (_16.Count>0)
        {
            if (_16.Count>=2)
            {
                 _16.RemoveAt(_16.Count-1);
                 _16.RemoveAt(_16.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(32);
                _IsDone += 1;
            }
            else if (_16.Count == 1)
            {
                _16.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(16);
                _IsDone += 1;
            }                
        }

        else if (_32.Count>0)
        {
            if (_32.Count>=2)
            {
                 _32.RemoveAt(_32.Count-1);
                 _32.RemoveAt(_32.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(64);
                _IsDone += 1;
            }
            else if (_32.Count == 1)
            {
                _32.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(32);
                _IsDone += 1;
            }                
        }


        else if (_64.Count>0)
        {
            if (_64.Count>=2)
            {
                 _64.RemoveAt(_64.Count-1);
                 _64.RemoveAt(_64.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(128);
                _IsDone += 1;
            }
            else if (_64.Count == 1)
            {
                _64.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(64);
                _IsDone += 1;
            }                
        }


        else if (_128.Count>0)
        {
            if (_128.Count>=2)
            {
                 _128.RemoveAt(_128.Count-1);
                 _128.RemoveAt(_128.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(256);
                _IsDone += 1;
            }
            else if (_128.Count == 1)
            {
                _128.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(128);
                _IsDone += 1;
            }                
        }


        else if (_256.Count>0)
        {
            if (_256.Count>=2)
            {
                _256.RemoveAt(_256.Count-1);
                _256.RemoveAt(_256.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(512);
                _IsDone += 1;
            }
            else if (_256.Count == 1)
            {
                _256.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(256);
                _IsDone += 1;
            }                
        }


        else if (_512.Count>0)
        {
            if (_512.Count>=2)
            {
                _512.RemoveAt(_512.Count-1);
                _512.RemoveAt(_512.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(1024);
                _IsDone += 1;
            }
            else if (_512.Count == 1)
            {
                _512.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(512);
                _IsDone += 1;
            }                
        }


        else if (_1024.Count>0)
        {
            if (_1024.Count>=2)
            {
                _1024.RemoveAt(_1024.Count-1);
                _1024.RemoveAt(_1024.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(2048);
                _IsDone += 1;
            }
            else if (_1024.Count == 1)
            {
                _1024.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(1024);
                _IsDone += 1;
            }                
        }


        else if (_2048.Count>0)
        {
            if (_2048.Count>=2)
            {
                _2048.RemoveAt(_2048.Count-1);
                _2048.RemoveAt(_2048.Count-1);
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(4096);
                _IsDone += 1;
            }
            else if (_2048.Count == 1)
            {
                _2048.Clear();
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(2048);
                _IsDone += 1;
            }                
        }


        else if (_4096.Count>0)
        {
            if (_4096.Count >= 1)
            {
                _4096.RemoveAt(_4096.Count - 1);
              
                GameObject go = Instantiate(_Ball, _BallSpawn.transform.position, Quaternion.identity);
                go.GetComponent<Ball>().SetValue(4096);
                _IsDone += 1;
            }                
        }

        if (_IsDone==0)
        {
            _MergeTime = false;
            machineAnimator.SetBool("vibration" , false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackBall"))
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
