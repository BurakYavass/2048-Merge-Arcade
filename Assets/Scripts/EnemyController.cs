using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private TutorialStage tutorialStage;
    [SerializeField] private GameObject mainObje;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private GameObject creatBall;
    [SerializeField] private GameObject[] closePart; 
    [SerializeField] private GameObject[] openPart;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI fullHealth;
    [SerializeField] private TextMeshProUGUI currentHealth;
    [SerializeField] private Image sliderValue;
    [SerializeField] private ParticleSystem bloodParticle;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float enemyDamage;
    [SerializeField] private int[] creatValue;
    [SerializeField] private float creatCount;
    [SerializeField] private float enemyHealthValue; 
    
    private Transform _playerTransform;
    private PlayerController _playerController;
    private Transform _target;
    private Vector3 _newPos;
    
    public float enemyHealthValueCurrent;
    public float wanderRadius;
    public float wanderTimer;
    public float waitTime;

    private float _enemyHealthValueCurrentTemp;
    private float _tempDamage;
    private float _timer;
    private bool _getHitting;
    private bool _hittable;
    private bool _playerHit;
    public bool enemyDie;
    private bool _bossDie;
    private bool _once;
    public bool tutorial;


    void Start()
    {
        enemyHealthValueCurrent = enemyHealthValue;
        fullHealth.text = (Mathf.Round(enemyHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(enemyHealthValueCurrent)).ToString("0");
        wanderTimer = Random.Range(5, 15);
        _timer = wanderTimer;
        _playerController = PlayerController.Current;
        colliders = GetComponents<Collider>();

    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerHit -= HitTaken;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitPoint"))
        {
            GameEventHandler.current.OnPlayerHit += HitTaken;
            //_playerHit = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            enemyAnimator.SetBool("Walking",false);
            enemyAnimator.SetBool("Idle",false);
            enemyAnimator.SetBool("Attack",true);
            _hittable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitPoint"))
        {
            GameEventHandler.current.OnPlayerHit -= HitTaken;
        }
        if (other.CompareTag("Player"))
        {
            enemyAgent.updateRotation = true;
            enemyAnimator.SetBool("Attack",false);
            _hittable = false;
        }
    }
    
    private void HitTaken(float damage)
    {
        _tempDamage = damage;
        bloodParticle.Play();
        enemyAgent.updatePosition = false;
        enemyAgent.enabled = false;
        _rb.isKinematic = false;
        var damageState = GameManager.current._damageState;
        if (damageState == 1)
        {
            _rb.AddForce(-transform.forward*damageState,ForceMode.Impulse);
        }
        else if (damageState > 1)
        {
            _rb.AddForce(-transform.forward*damageState,ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(-transform.forward,ForceMode.Impulse);
        }
        _enemyHealthValueCurrentTemp = Mathf.Clamp(enemyHealthValueCurrent - _tempDamage, 0, enemyHealthValue);
        _getHitting = true;
        transform.DOKill();
        var monsterScale = transform.lossyScale;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce).OnComplete((() =>
        {
            _rb.isKinematic = true;
            enemyAgent.enabled = true;
            enemyAgent.updatePosition = true;
            transform.localScale = monsterScale;
        }));
    }


    void Update()
    {
        _timer += Time.deltaTime;
        
        if (_getHitting)
        {
            if (enemyHealthValueCurrent>=1)
            {
                enemyHealthValueCurrent = Mathf.Lerp(enemyHealthValueCurrent, _enemyHealthValueCurrentTemp, .1f);
                currentHealth.text = enemyHealthValueCurrent.ToString("0");
                sliderValue.fillAmount = (1 * (enemyHealthValueCurrent / enemyHealthValue));
            }
            else
            {
                enemyDie = true;
                if (tutorial)
                {
                    tutorial = false;
                    TutorialControl.Instance.CompleteStage(tutorialStage);
                }
                healthBar.SetActive(false);
                if (!_once)
                {
                    _once = true;
                    _getHitting = false;
                    foreach (var collider in colliders)
                    {
                        collider.enabled = false;
                    }
                    StartCoroutine(CloseDelay());
                    transform.DOScale(Vector3.zero, 0.5f);
                    foreach (var close in closePart)
                    {
                        close.transform.localScale = Vector3.Lerp(close.transform.localScale,Vector3.zero,.1f);
                    } 
                    foreach (var open in openPart)
                    {
                        open.SetActive(true);
                    } 
                }
                
            }
        }

        if (enemyAgent.enabled)
        {
            var playerPosition = _playerController.gameObject.transform.position;
            var distance = Vector3.Distance(transform.position, playerPosition);
            if (distance < 15.0f)
            {
                healthBar.SetActive(true);
                enemyAgent.destination = playerPosition;
                enemyAnimator.SetBool("Walking",true);
                enemyAnimator.SetBool("Idle",false);
            }
            else
            {
                healthBar.SetActive(false);
                if (_timer >= wanderTimer)
                {
                    _newPos = RandomNavSphere(transform.position, Random.Range(5f,wanderRadius), -1);
                    enemyAgent.SetDestination(_newPos);
                    _timer = 0;
                }

                if (enemyAgent.remainingDistance < 4)
                {
                    enemyAnimator.SetBool("Walking",false);
                    enemyAnimator.SetBool("Attack",false);
                    enemyAnimator.SetBool("Idle",true);
                }
                else
                {
                    enemyAnimator.SetBool("Walking",true);
                    enemyAnimator.SetBool("Idle", false);
                }
            }
        }
    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        
        Vector3 randDirection = Random.insideUnitCircle.normalized;

        Vector3 randomVector3 = new Vector3(randDirection.x, 0, randDirection.y) * dist;
 
        randomVector3 += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randomVector3, out navHit, dist, layermask);
 
        return navHit.position;
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var close in closePart)
        {
            close.SetActive(false);
        }
        for (int i = 0; i < creatCount; i++)
        {
            GameObject go = Instantiate(creatBall, transform.position, Quaternion.LookRotation(Vector3.forward));
            if (creatValue.Length > 1)
            {
                int rdnm=Random.Range(0,100);
                if (rdnm>=70)
                {
                    go.GetComponent<Ball>().SetValue(creatValue[0]);
    
                }
                else
                {
                    go.GetComponent<Ball>().SetValue(creatValue[1]);

                }
            }
            else
            {
                go.GetComponent<Ball>().SetValue(creatValue[0]);
            }
            
        }

        _once = true;
        mainObje.SetActive(false);
        //mainObje.GetComponent<CloseDelay>().CloseObje();
    }

    private void EnemyHit()
    {
        if (_hittable)
        {
            _playerController.HitTaken(enemyDamage);
        }
    }
}
