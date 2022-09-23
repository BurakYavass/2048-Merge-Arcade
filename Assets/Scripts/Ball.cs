using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] _Colors;
    [SerializeField] float _BallValue;
    [SerializeField] private SphereCollider triggerCollider;
    [SerializeField] private RuntimeAnimatorController merge;
    [SerializeField] private BallController ballController;
    public NavMeshAgent agent;
    private PlayerController _playerController;
    public GameObject targetObje;
    public Rigidbody ballRb;
    private Collider _collider;
    
    private float _DelayMerge;
    private float _distance;
    public  bool _Go;
    public  bool _GoMerge;
    public  bool _GoUpgrade;
    private bool _OnStackpos;
    
    
    void Start()
    {
        //ballController = GameObject.FindGameObjectWithTag("BallController").GetComponent<BallController>();
        _playerController = PlayerController.Current;
        BallChanger();
        StartCoroutine(DelayKinematic());
    }
  void FixedUpdate()
    {
        if (_Go)
        {
            agent.speed = GameManager.current.playerSpeed;
            var distance = Vector3.Distance(transform.position , targetObje.transform.position);
            if (distance < 3.0f && !_playerController.walking)
            {
                agent.enabled = false;
                ballRb.isKinematic = true;
                //_Go = false;
                _OnStackpos = true;
            }
            else
            {
                ballRb.isKinematic = false;
                agent.enabled = true;
                agent.destination = targetObje.transform.position;
            }
        }
        else if (_GoMerge)
        {
            transform.position = Vector3.Lerp(transform.position, targetObje.transform.position, .03f   );
            transform.localScale = Vector3.Lerp(transform.localScale, targetObje.transform.localScale, .03f   );
            if (Vector3.Distance(transform.position, targetObje.transform.position) < _distance * .05f)
            {
                ballRb.isKinematic = false;
                _collider.enabled = false;
                _GoMerge = false;
                StartCoroutine(DelayMergeTime());
            }
        }
        else if (_GoUpgrade)
        {
            Vector3 rndm = new Vector3(Random.Range(-2, 2), Random.Range(2, 4), 0);
            if (Vector3.Distance(transform.position, targetObje.transform.position) < _distance * .05f)
            {
                ballRb.isKinematic = false;
                _collider.enabled = false;
                _GoUpgrade = false;
            }
            else if (Vector3.Distance(transform.position, targetObje.transform.position+ rndm) < _distance * .8f)
            {
                transform.position = Vector3.Lerp(transform.position, targetObje.transform.position+ rndm, .01f);
                transform.localScale = Vector3.Lerp(transform.localScale, targetObje.transform.localScale, .01f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetObje.transform.position, .03f);
                transform.localScale = Vector3.Lerp(transform.localScale, targetObje.transform.localScale, .03f);
            }
        }
    }
    public void SetValue(float ballvalue)
    {
        _BallValue = ballvalue;
        BallChanger();
    }
    void BallChanger()
    {
        for (int i = 0; i < _Colors.Length; i++)
        {
            _Colors[i].SetActive(false);
        }
        if (_BallValue==2)
        {
            _Colors[0].SetActive(true);
        }
        else if (_BallValue == 4)
        {
            _Colors[1].SetActive(true);
        }
        else if (_BallValue == 8)
        {
            _Colors[2].SetActive(true);
        }
        else if (_BallValue == 16)
        {
            _Colors[3].SetActive(true);
        }
        else if (_BallValue == 32)
        {
            _Colors[4].SetActive(true);
        }
        else if (_BallValue == 64)
        {
            _Colors[5].SetActive(true);
        }
        else if (_BallValue == 128)
        {
            _Colors[6].SetActive(true);
        }
        else if (_BallValue == 256)
        {
            _Colors[7].SetActive(true);
        }
        else if (_BallValue == 512)
        {
            _Colors[8].SetActive(true);
        }
        else if (_BallValue == 1024)
        {
            _Colors[9].SetActive(true);
        }
        else if (_BallValue == 2048)
        {
            _Colors[10].SetActive(true);
        }
        else if (_BallValue == 4096)
        {
            _Colors[11].SetActive(true);
        }
    }
    
    public void SetGoUpgrade(GameObject target, float delay)
    {
        ballRb.isKinematic = true;
        targetObje = target;
        _distance = Vector3.Distance(transform.position, targetObje.transform.position);
        _collider.isTrigger = true;
        _GoUpgrade = true;
    }
    
    public void SetGoTarget(Transform target)
    {
        //ballController.SetNewBall(gameObject);
        //_rb.isKinematic = true;
        targetObje = target.gameObject;
        _Go = true;
    }
    
    public void SetGoMerge(GameObject target,float delay)
    {
        _Go = false;
        agent.enabled = false;
        _DelayMerge = delay;
        gameObject.transform.parent = target.transform.parent;
        ballRb.isKinematic = true;
        targetObje = target;
        _distance = Vector3.Distance(transform.position, targetObje.transform.position);
        gameObject.tag = "MergeBall";
        _collider.isTrigger = true;
        _GoMerge = true;
    }
    
    public bool GetStackPos()
    {
        return _OnStackpos;
    }
    public float GetValue()
    {
        return _BallValue;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name=="BallPool")
        {
            ballRb.isKinematic = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BallPool")
        {
            ballRb.isKinematic = true;
        }
    }

    IEnumerator DelayKinematic()
    {
        yield return new WaitForSeconds(1);
        triggerCollider.enabled = true;
        //triggerCollider.isTrigger = true;
        yield return new WaitForSeconds(2);
        ballRb.isKinematic = true;
        agent.enabled = true;
    }
    IEnumerator DelayMergeTime( )
    {
        _collider.isTrigger = false;
        yield return new WaitForSeconds(_DelayMerge);
        GetComponent<Animator>().runtimeAnimatorController = merge;
        //_rb.isKinematic = true;
        _collider.isTrigger = true;

    }
}
