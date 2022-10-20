using System.Collections;
using DG.Tweening;
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
    [SerializeField] private GameObject wing;
    [SerializeField] private MeshRenderer closePart;
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
        var speed = GameManager.current.playerSpeed;
        agent.speed = speed;
        if (_BallValue >= 128)
        {
            ballRb.freezeRotation = true;
        }
        else
        {
            ballRb.freezeRotation = false;
        }
    }

    private void Awake()
    {
        agent.updatePosition = false;
    }

    void FixedUpdate()
    {
        if (go && targetObje != null && agent.enabled)
        {
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
                
                if (agent.isOnNavMesh)
                {
                    ballRb.isKinematic = true;
                    _collider.isTrigger = true;
                    agent.SetDestination(targetObjePos);
                    transform.position = Vector3.SmoothDamp(transform.position,agent.nextPosition, ref currentVelocity, 0.1f);
                    //agent.destination = Vector3.SmoothDamp(transform.position,targetObjePos, ref currentVelocity,.1f);
                }
                
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
                closePart = _Colors[0].GetComponent<MeshRenderer>();
                
                if (_Colors[0].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[0].GetComponent<Animator>();
                }

                break;
            }
            case 4:
            {
                _Colors[1].SetActive(true);
                closePart = _Colors[1].GetComponent<MeshRenderer>();
                if (_Colors[1].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[1].GetComponent<Animator>();
                }

                break;
            }
            case 8:
            {
                _Colors[2].SetActive(true);
                closePart = _Colors[2].GetComponent<MeshRenderer>();
                if (_Colors[2].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[2].GetComponent<Animator>();
                }

                break;
            }
            case 16:
            {
                _Colors[3].SetActive(true);
                closePart = _Colors[3].GetComponent<MeshRenderer>();
                if (_Colors[3].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[3].GetComponent<Animator>();
                }

                break;
            }
            case 32:
            {
                _Colors[4].SetActive(true);
                closePart = _Colors[4].GetComponent<MeshRenderer>();
                
                if (_Colors[4].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[4].GetComponent<Animator>();
                }

                break;
            }
            case 64:
            {
                _Colors[5].SetActive(true);
                closePart = _Colors[5].GetComponent<MeshRenderer>();
                if (_Colors[5].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[5].GetComponent<Animator>();
                }

                break;
            }
            case 128:
            {
                _Colors[6].SetActive(true);
                wing =  _Colors[6].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[6].GetComponent<MeshRenderer>();
                if (_Colors[6].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[6].GetComponent<Animator>();
                }
                

                break;
            }
            case 256:
            {
                _Colors[7].SetActive(true);
                wing =  _Colors[7].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[7].GetComponent<MeshRenderer>();
                if (_Colors[7].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[7].GetComponent<Animator>();
                }

                break;
            }
            case 512:
            {
                _Colors[8].SetActive(true);
                wing =  _Colors[8].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[8].GetComponent<MeshRenderer>();
                if (_Colors[8].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[8].GetComponent<Animator>();
                }

                break;
            }
            case 1024:
            {
                _Colors[9].SetActive(true);
                wing =  _Colors[9].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[9].GetComponent<MeshRenderer>();

                if (_Colors[9].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[9].GetComponent<Animator>();
                }

                break;
            }
            case 2048:
            {
                _Colors[10].SetActive(true);
                wing =  _Colors[10].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[10].GetComponent<MeshRenderer>();

                if (_Colors[10].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[10].GetComponent<Animator>();
                }

                break;
            }
            case 4096:
            {
                _Colors[11].SetActive(true);
                wing = _Colors[11].transform.GetChild(0).gameObject;
                wing.SetActive(true);
                closePart = _Colors[11].GetComponent<MeshRenderer>();
                if (_Colors[11].GetComponent<Animator>() != null)
                {
                    ballAnimator = _Colors[11].GetComponent<Animator>();
                }

                break;
            }
        }
    }
    
    // public void SetGoUpgrade(GameObject target)
    // {
    //     agent.enabled = false;
    //     ballRb.useGravity = false;
    //     triggerCollider.enabled = false;
    //     _collider.isTrigger = true;
    //     ballRb.isKinematic = false;
    //     gameObject.transform.parent = target.transform.parent;
    //     go = false;
    //     goMerge = true;
    //     ballAnimator.SetBool("Anim", false);
    //     dustParticle.SetActive(false);
    //     ballRb.interpolation = RigidbodyInterpolation.None;
    //     gameObject.tag = "UpgradeBall";
    //     if (gameObject.activeInHierarchy)
    //     {
    //         transform.DOMove(target.transform.position, 0.7f).SetEase(Ease.Flash)
    //             .OnUpdate((() =>
    //             {
    //                 transform.localScale -= new Vector3(.3f, .3f, .3f) * Time.fixedDeltaTime;
    //             })).OnComplete((() =>
    //             {
    //                 transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    //                 goUpgrade = false;
    //             }));
    //     }
    //     else
    //     {
    //         transform.DOKill();
    //     }
    //     
    // }

    public void SetGoTarget(Transform target)
    {
        targetObje = target.gameObject;
        goUnlock = false;
        goMerge = false;
        goTravel = false;
        
        go = true;
    }
    
    public void SetGoMerge(GameObject target,float delay)
    {
        dustParticle.SetActive(false);
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
        ballAnimator.SetBool("Anim", false);
        dustParticle.SetActive(false);
        gameObject.tag = "UnlockBall";
        go = false;
        goMerge = false;
        goTravel = false;
        goUnlock = true;
        agent.enabled = false;
        ballRb.useGravity = false;
        triggerCollider.enabled = false;
        _collider.isTrigger = true;
        ballRb.isKinematic = false;
        if (goUnlock)
        {
            goUnlock = false;
            transform.DOMove(target.position, 1.5f).SetEase(Ease.OutBounce);
        }
    }
    public void SetGoTravel()
    {
        go = false;
        goMerge = false;
        goUnlock = false;
        goTravel = true;
        closePart.enabled = false;
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
        if (_BallValue >=128)
            ballRb.isKinematic = true;
        else
            ballRb.isKinematic = false;
        ballRb.interpolation = RigidbodyInterpolation.None;
        var pos = transform.position;
        ballRb.AddForce(new Vector3(Random.Range(2,5)*2, Random.Range(3, 5)*2, Random.Range(2, 3)*2),ForceMode.VelocityChange);
        goFree = false;
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
            
            if (_BallValue>=128)
            {
                wing.SetActive(false);
            }
            GameEventHandler.current.BallMergeArea(true);
            other.transform.parent.transform.DOPunchScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InBounce)
                .OnComplete((() => other.transform.parent.transform.localScale = Vector3.one));
        }

        if (other.CompareTag("Ground"))
        {
            dustParticle.SetActive(true);
            StartCoroutine(DelayKinematic());
        }

        if (other.CompareTag("Player"))
        {
            agent.enabled = true;
            StartCoroutine(DelayKinematic());
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
        if (other.CompareTag("MergeController"))
        {
            GameEventHandler.current.BallMergeArea(false);
            if (wing !=null)
            {
                wing.SetActive(true);
            }
        }
    }

    public void StartDelay()
    {
        StartCoroutine(DelayKinematic());
    }

    private IEnumerator DelayKinematic()
    {
        //ballRb.useGravity = true;
        //agent.enabled = true;
        yield return new WaitForSeconds(0.2f);
        if (targetObje == null)
        {
            gameObject.tag = "EmptyBall";
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
        //_collider.enabled = true;
        triggerCollider.enabled = true;
        triggerCollider.radius = 2;
        triggerCollider.center = Vector3.up;

    }
    IEnumerator DelayMergeTime(float delay)
    {
        //triggerCollider.isTrigger = false;
        yield return new WaitForSeconds(.1f);
        GetComponent<Animator>().runtimeAnimatorController = merge;
        triggerCollider.isTrigger = true;
    }
}
