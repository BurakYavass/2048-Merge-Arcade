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
    [SerializeField] private GameObject dustParticle;
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
    public bool goFree;

    void Start()
    {
        ballController = GameObject.FindGameObjectWithTag("BallController").GetComponent<BallController>();
        BallChanger();
        //StartCoroutine(DelayKinematic());
        ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
        var speed = GameManager.current.playerSpeed;
        agent.speed = speed;
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
                if (distance > 6.0f)
                {
                    agent.speed = Mathf.Clamp(agent.speed + 0.5f* Time.fixedDeltaTime,speed,50);
                }
                else
                {
                    agent.speed = Mathf.Clamp(agent.speed - 0.5f* Time.fixedDeltaTime,speed,50);
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

        switch (_BallValue)
        {
            case 2:
            {
                _Colors[0].SetActive(true);
                _Colors[0].transform.DOLocalJump(
                    new Vector3(Random.Range(0, 3), Random.Range(3, 5),Random.Range(0, 3)),
                    1,1,2.0f);
                if (_Colors[0].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[0].GetComponent<Animator>();
                }

                break;
            }
            case 4:
            {
                _Colors[1].SetActive(true);
                if (_Colors[1].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[1].GetComponent<Animator>();
                }

                break;
            }
            case 8:
            {
                _Colors[2].SetActive(true);
                if (_Colors[2].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[2].GetComponent<Animator>();
                }

                break;
            }
            case 16:
            {
                _Colors[3].SetActive(true);
                if (_Colors[3].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[3].GetComponent<Animator>();
                }

                break;
            }
            case 32:
            {
                _Colors[4].SetActive(true);
                if (_Colors[4].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[4].GetComponent<Animator>();
                }

                break;
            }
            case 64:
            {
                _Colors[5].SetActive(true);
                if (_Colors[5].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[5].GetComponent<Animator>();
                }

                break;
            }
            case 128:
            {
                _Colors[6].SetActive(true);
                if (_Colors[6].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[6].GetComponent<Animator>();
                }

                break;
            }
            case 256:
            {
                _Colors[7].SetActive(true);
                if (_Colors[7].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[7].GetComponent<Animator>();
                }

                break;
            }
            case 512:
            {
                _Colors[8].SetActive(true);
                if (_Colors[8].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[8].GetComponent<Animator>();
                }

                break;
            }
            case 1024:
            {
                _Colors[9].SetActive(true);
                if (_Colors[9].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[9].GetComponent<Animator>();
                }

                break;
            }
            case 2048:
            {
                _Colors[10].SetActive(true);
                if (_Colors[10].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[10].GetComponent<Animator>();
                }

                break;
            }
            case 4096:
            {
                _Colors[11].SetActive(true);
                if (_Colors[11].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[11].GetComponent<Animator>();
                }

                break;
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
        dustParticle.SetActive(false);
        ballRb.interpolation = RigidbodyInterpolation.None;
        gameObject.tag = "UpgradeBall";
        if (gameObject.activeInHierarchy)
        {
            transform.DOMove(target.transform.position, 0.7f).SetEase(Ease.Flash)
                .OnUpdate((() =>
                {
                    transform.localScale -= new Vector3(.3f, .3f, .3f) * Time.fixedDeltaTime;
                })).OnComplete((() =>
                {
                    transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
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
        transform.DOJump(new Vector3(pos.x,pos.y,pos.z), 1, 1, 1.0f).SetEase(Ease.OutBounce)
            .OnComplete((() => transform.DOJump(targetObje.transform.position,1,1,0.5f)))
                                                    .SetEase(Ease.OutBounce).OnComplete((() => dustParticle.SetActive(true)));
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
            transform.DOJump(target.transform.position, 2, 1, 1.5f).SetEase(Ease.OutQuint)
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
        dustParticle.SetActive(false);
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

    public void SetGoFree()
    {
        _collider.isTrigger = false;
        targetObje = null;
        agent.enabled = false;
        go = false;
        goMerge = false;
        goUnlock = false;
        goTravel = false;
        gameObject.tag = "EmptyBall";
        dustParticle.SetActive(false);
        ballAnimator.SetBool("Anim", false);
        ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
        ballRb.AddForce(new Vector3(Random.Range(2,5), Random.Range(3, 5), Random.Range(2, 3)),ForceMode.VelocityChange);
        goFree = false;
        // var pos = transform.position;
        // transform.DOJump(
        //         new Vector3(pos.x+ Random.Range(1,3),pos.y+ Random.Range(3, 5),pos.z+ Random.Range(1, 3)), 
        //             1, 1, .5f)
        //                         .SetEase(Ease.InBounce)
        //                             .OnComplete((() => goFree =false));

    }
    
    public int GetValue()
    {
        return _BallValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallPool"))
        {
            //transform.DOKill();
            goMerge = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GameEventHandler.current.BallMergeArea(true);
            other.transform.parent.transform.DOPunchScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InBounce)
                .OnComplete((() => other.transform.parent.transform.localScale = Vector3.one));
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
            GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        if (other.CompareTag("MergeController"))
        {
            GameEventHandler.current.BallMergeArea(false);
        }
    }

    IEnumerator DelayKinematic()
    {
        ballRb.useGravity = true;
        yield return new WaitForSeconds(0.1f);
        triggerCollider.enabled = true;
        ballRb.interpolation = RigidbodyInterpolation.Interpolate;
    }
    IEnumerator DelayMergeTime(float delay)
    {
        //triggerCollider.isTrigger = false;
        yield return new WaitForSeconds(.1f);
        GetComponent<Animator>().runtimeAnimatorController = merge;
        triggerCollider.isTrigger = true;
    }
}
