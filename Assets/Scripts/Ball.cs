using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] _Colors;
    [SerializeField] public int _BallValue;
    [SerializeField] private SphereCollider triggerCollider;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private RuntimeAnimatorController merge;
    [SerializeField] private BallController ballController;
    [SerializeField] private Animator ballAnimator;
    [SerializeField] private ParticleSystem dustParticle;
    public NavMeshAgent agent;
    public GameObject targetObje;
    public Rigidbody ballRb;

    private float _delayMerge;
    private float _distance;
    public bool go;
    public bool goMerge;
    public bool goUpgrade;
    public bool goUnlock;
    public bool goTravel;

    void Start()
    {
        ballController = GameObject.FindGameObjectWithTag("BallController").GetComponent<BallController>();
        BallChanger();
        StartCoroutine(DelayKinematic());
        ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
    }

    private void Awake()
    {
        agent.updatePosition = false;
    }

    void FixedUpdate()
    {
        if (go && agent.enabled && targetObje != null )
        {
            ballRb.isKinematic = true;
            var targetObjePos = targetObje.transform.position;
            var distance = Vector3.Distance(transform.position , targetObjePos);
            if (Mathf.Abs(agent.stoppingDistance - distance) > 3.0f)
            {
                var speed = GameManager.current.playerSpeed;
                agent.speed = speed;
                if (distance > 10.0f)
                {
                    agent.speed = Mathf.Clamp(agent.speed + 0.5f *Time.fixedDeltaTime,12,50);
                }
                else
                {
                    agent.speed = Mathf.Clamp(agent.speed - 0.5f* Time.fixedDeltaTime,12,50);
                }
                
                if (ballAnimator!=null)
                {
                    ballAnimator.SetBool("Anim", true);
                }
                
                var currentVelocity = Vector3.zero;
                agent.SetDestination(targetObjePos);
                transform.position = Vector3.SmoothDamp(transform.position,agent.nextPosition, ref currentVelocity, 0.1f);
                //agent.destination = Vector3.SmoothDamp(transform.position,targetObjePos, ref currentVelocity,Time.smoothDeltaTime);
            }
            else
            {
                if (ballAnimator!=null)
                {
                    ballAnimator.SetBool("Anim", false);
                }
                agent.speed = GameManager.current.playerSpeed;
            }
        }
    }
    public void SetValue(int ballvalue)
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
            _Colors[0].transform.DOLocalJump(
                new Vector3(Random.Range(0, 3), Random.Range(3, 5),Random.Range(0, 3)),
                                    1,1,2.0f);
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
            if (_Colors[6].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[6].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 256)
        {
            _Colors[7].SetActive(true);
            if (_Colors[7].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[7].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 512)
        {
            _Colors[8].SetActive(true);
            if (_Colors[8].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[8].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 1024)
        {
            _Colors[9].SetActive(true);
            if (_Colors[9].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[9].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 2048)
        {
            _Colors[10].SetActive(true);
            if (_Colors[10].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[10].GetComponent<Animator>();
            }
        }
        else if (_BallValue == 4096)
        {
            _Colors[11].SetActive(true);
            if (_Colors[11].GetComponent<Animator>() != null)
            {
                ballAnimator = _Colors[11].GetComponent<Animator>();
            }
        }
    }
    
    public void SetGoUpgrade(GameObject target)
    {
        agent.enabled = false;
        ballRb.useGravity = false;
        triggerCollider.enabled = false;
        _collider.isTrigger = true;
        ballRb.isKinematic = false;
        gameObject.transform.parent = target.transform.parent;
        go = false;
        goMerge = true;
        ballAnimator.SetBool("Anim", false);
        ballAnimator = null;
        dustParticle.Stop();
        ballRb.interpolation = RigidbodyInterpolation.None;
        gameObject.tag = "UpgradeBall";
        if (gameObject.activeInHierarchy)
        {
            transform.DOMove(target.transform.position, 2)
                .OnUpdate((() =>
                {
                    transform.localScale -= new Vector3(.3f, .3f, .3f) * Time.deltaTime;
                })).OnComplete((() =>
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    goUpgrade = false;
                }));
        }
        else
        {
            transform.DOKill();
        }
        
    }
    
    public void SetGoTarget(Transform target)
    {
        targetObje = target.gameObject;
        go = true;
        goUnlock = false;
        goMerge = false;
        goTravel = false;
        ballRb.isKinematic = true;
        agent.enabled = true;
        _collider.isTrigger = true;
        var pos = transform.position;
        transform.DOJump(new Vector3(pos.x,pos.y,pos.z), 1, 1, 1.0f)
                                                    .SetEase(Ease.OutBounce).OnComplete((() => dustParticle.Play()));
    }
    
    public void SetGoMerge(GameObject target,float delay)
    {
        agent.enabled = false;
        ballRb.useGravity = true;
        triggerCollider.enabled = true;
        ballRb.isKinematic = false;
        gameObject.transform.parent = target.transform.parent;
        go = false;
        goUnlock = false;
        goMerge = true;
        ballAnimator.SetBool("Anim", false);
        ballAnimator = null;
        ballRb.interpolation = RigidbodyInterpolation.None;
        gameObject.tag = "MergeBall";
        if (gameObject.activeInHierarchy)
        {
            transform.DOJump(target.transform.position, 2, 1, 1f).SetEase(Ease.OutQuint)
                .OnUpdate((() =>
                {
                    ballRb.mass = 0.01f;
                    transform.localScale -= new Vector3(.3f, .3f, .3f) * Time.deltaTime;
                })).OnComplete((() =>
                {
                    agent.enabled = false;
                    _collider.isTrigger = false;
                    transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    StartCoroutine(DelayMergeTime(delay));
                }));
        }
        else
        {
            transform.DOKill();
        }
    }

    public void SetGoUnlock(Transform target)
    {
        dustParticle.Stop();
        ballAnimator.SetBool("Anim", false);
        ballAnimator = null;
        go = false;
        goMerge = false;
        goTravel = false;
        goUnlock = true;
        agent.enabled = false;
        ballRb.useGravity = false;
        triggerCollider.enabled = false;
        _collider.isTrigger = true;
        ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
        gameObject.tag = "UnlockBall";
        if (gameObject.activeInHierarchy)
        {
            transform.DOMove(target.position, 1.5f).SetEase(Ease.OutBounce)
                .OnUpdate((() =>
                {
                    transform.localScale -= new Vector3(.3f, .3f, .3f) * Time.deltaTime;
                })).OnComplete((() =>
                {
                    transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    //target.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f).SetEase(Ease.OutBounce);
                    goUnlock = false;
                }));
        }
        else
        {
            transform.DOKill();
        }
    }
    public void SetGoTravel()
    {
        go = false;
        goMerge = false;
        goUnlock = false;
        goTravel = true;
    }
    
    public int GetValue()
    {
        return _BallValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.enabled = true;
        }
        if (other.CompareTag("BallPool"))
        {
            //transform.DOKill();
            goMerge = false;
            GameEventHandler.current.BallMergeArea(true);
        }
        
        if (other.CompareTag("BallUpgrade"))
        {
            transform.DOKill();
            other.transform.DOKill();
            other.transform.DOPunchScale(new Vector3(0.1f,0.1f,0.1f),0.1f).SetEase(Ease.InBounce)
                .OnComplete((() =>
                {
                    other.transform.localScale = Vector3.one;
                }));
            triggerCollider.enabled = false;
            goUpgrade = false;
            
        }

        if (other.CompareTag("LevelWall"))
        {
            transform.DOKill();
            triggerCollider.enabled = false;
            goUnlock = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BallPool"))
        {
            GameEventHandler.current.BallMergeArea(false);
        }
    }

    IEnumerator DelayKinematic()
    {
        ballRb.useGravity = true;
        yield return new WaitForSeconds(1);
        triggerCollider.enabled = true;
        ballRb.interpolation = RigidbodyInterpolation.Interpolate;
    }
    IEnumerator DelayMergeTime(float delay)
    {
        //triggerCollider.isTrigger = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().runtimeAnimatorController = merge;
        triggerCollider.isTrigger = true;
    }

    // public void PlayParticle()
    // {
    //     dustParticle.Play();
    // }
}
