using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] _Colors;
    [SerializeField] float _BallValue;
    [SerializeField] private SphereCollider triggerCollider;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private RuntimeAnimatorController merge;
    [SerializeField] private BallController ballController;
    [SerializeField] private Animator ballAnimator;
    public NavMeshAgent agent;
    public GameObject targetObje;
    public Rigidbody ballRb;

    private float _DelayMerge;
    private float _distance;
    public  bool _Go;
    public  bool _GoMerge;
    public  bool _GoUpgrade;
    private bool _OnStackpos;

    void Start()
    {
        ballController = GameObject.FindGameObjectWithTag("BallController").GetComponent<BallController>();
        BallChanger();
        StartCoroutine(DelayKinematic());
        ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
    }
    void FixedUpdate()
    {
        if (_Go && agent.enabled)
        {
            var distance = Vector3.Distance(transform.position , targetObje.transform.position);
            if (distance < 4.0f)
            {
                agent.isStopped = true;
                if (ballAnimator!=null)
                {
                    ballAnimator.SetBool("Jump", false);
                }
                agent.speed = GameManager.current.playerSpeed;
            }
            else
            {
                var speed = GameManager.current.playerSpeed;
                if (distance > 6)
                {
                    agent.speed = speed +3;
                }
                else
                {
                    agent.speed = speed +1;
                }

                if (ballAnimator!=null)
                {
                    ballAnimator.SetBool("Jump", true);
                }
                agent.isStopped = false;
                var currentVelocity = agent.velocity;
                agent.destination =
                    Vector3.SmoothDamp(transform.position, targetObje.transform.position, ref currentVelocity,
                        Time.fixedDeltaTime);
            }
            
        }
        
        if (_GoMerge)
        {
            transform.position = Vector3.Lerp(transform.position, targetObje.transform.position, .03f   );
            transform.localScale = Vector3.Lerp(transform.localScale, targetObje.transform.localScale, .03f   );
            if (Vector3.Distance(transform.position, targetObje.transform.position) < _distance * .02f)
            {
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
                triggerCollider.enabled = true;
                _GoMerge = false;
                StartCoroutine(DelayMergeTime());
            }
        }
        
        if (_GoUpgrade)
        {
            Vector3 rndm = new Vector3(Random.Range(-2, 2), Random.Range(2, 4), 0);
            if (Vector3.Distance(transform.position, targetObje.transform.position) < _distance * .05f)
            {
                //ballRb.isKinematic = false;
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
            if (_Colors[0].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[0].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 4)
        {
            _Colors[1].SetActive(true);
            if (_Colors[1].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[1].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 8)
        {
            _Colors[2].SetActive(true);
            if (_Colors[2].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[2].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 16)
        {
            _Colors[3].SetActive(true);
            if (_Colors[3].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[3].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 32)
        {
            _Colors[4].SetActive(true);
            if (_Colors[4].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[4].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 64)
        {
            _Colors[5].SetActive(true);
            if (_Colors[5].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[5].GetComponent<Animator>();
            }
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
        //ballRb.isKinematic = true;
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
        ballRb.isKinematic = true;
    }
    
    public void SetGoMerge(GameObject target,float delay)
    {
        agent.enabled = false;
        ballRb.useGravity = false;
        ballRb.isKinematic = true;
        triggerCollider.enabled = false;
        if (ballAnimator!=null)
        {
            ballAnimator.SetBool("Jump", false);
            ballAnimator = null;
        }
        ballRb.interpolation = RigidbodyInterpolation.None;
        _Go = false;
        _GoMerge = true;
        _DelayMerge = delay;
        gameObject.transform.parent = target.transform.parent;
        targetObje = target;
        _distance = Vector3.Distance(transform.position, targetObje.transform.position);
        gameObject.tag = "MergeBall";
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
             _collider.isTrigger = true;
             agent.enabled = false;
            GameEventHandler.current.BallMergeArea(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BallPool")
        {
            //triggerCollider.isTrigger = true;
            //ballRb.isKinematic = true;
        }
    }

    IEnumerator DelayKinematic()
    {
        ballRb.isKinematic = false;
        ballRb.useGravity = true;
        yield return new WaitForSeconds(1);
        triggerCollider.enabled = true;
        yield return new WaitForSeconds(2);
        ballRb.interpolation = RigidbodyInterpolation.Interpolate;
        agent.enabled = true;
        ballRb.isKinematic = true;
        ballRb.useGravity = true;
    }
    IEnumerator DelayMergeTime( )
    {
        triggerCollider.isTrigger = false;
        yield return new WaitForSeconds(_DelayMerge);
        GetComponent<Animator>().runtimeAnimatorController = merge;
        triggerCollider.isTrigger = true;

    }
}
