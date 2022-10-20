using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject mainObje;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private NavMeshAgent enemyAgent;
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
    
    public bool boss;
    
    private float _enemyHealthValueCurrentTemp;
    private float _tempDamage;
    private float _timer;
    private bool _getHitting;
    private bool _hitting = false;
    private bool _hittable;
    private bool _playerHit;
    
    void Start()
    {
        enemyHealthValueCurrent = enemyHealthValue;
        fullHealth.text = (Mathf.Round(enemyHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(enemyHealthValueCurrent)).ToString("0");
        wanderTimer = Random.Range(5, 15);
        _playerTransform = PlayerController.Current.transform;
        _timer = wanderTimer;
        
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
            var playerTransform = other.transform.position;
            enemyAgent.transform.LookAt(playerTransform,Vector3.up);
            enemyAnimator.SetBool("Walking",false);
            enemyAnimator.SetBool("Idle",false);
            enemyAnimator.SetBool("Attack",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitPoint"))
        {
            GameEventHandler.current.OnPlayerHit -= HitTaken;
            //_playerHit = false;
        }
        if (other.CompareTag("Player"))
        {
            _playerController = null;
            enemyAgent.updateRotation = true;
            enemyAnimator.SetBool("Attack",false);
            _hitting = false;
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
                var colliders = GetComponents<Collider>();
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
                StartCoroutine(CloseDelay());
                transform.DOScale(Vector3.zero, 0.5f);
                for (int i = 0; i < closePart.Length; i++)
                {
                    closePart[i].transform.localScale = Vector3.Lerp(closePart[i].transform.localScale,Vector3.zero,.1f);
          
                } 
                for (int i = 0; i < openPart.Length; i++)
                {
                    openPart[i].SetActive(true);
                }
            }
        }

        if (enemyAgent.enabled)
        {
            var distance = Vector3.Distance(transform.position, _playerTransform.position);
            if (distance < 15.0f && !_hitting)
            {
                healthBar.SetActive(true);
                enemyAgent.destination = _playerTransform.position;
                enemyAnimator.SetBool("Walking",true);
                enemyAnimator.SetBool("Idle",false);
            }
            else
            {
                healthBar.SetActive(false);
                if (_timer >= wanderTimer)
                {
                    if (!boss)
                    {
                        _newPos = RandomNavSphere(transform.position, Random.Range(5f,wanderRadius), -1);
                        enemyAgent.SetDestination(_newPos);
                    }
                    _timer = 0;
                }
            
                var dist = Vector3.Distance(transform.position, _newPos);
                if (dist <= 5f)
                {
                    enemyAnimator.SetBool("Walking",false);
                    enemyAnimator.SetBool("Attack",false);
                    enemyAnimator.SetBool("Idle",true);
                }
                else
                {
                    if (!boss)
                    {
                        enemyAnimator.SetBool("Walking",true);
                        enemyAnimator.SetBool("Idle", false);
                    }
                    else
                    {
                        enemyAnimator.SetBool("Walking",false);
                        enemyAnimator.SetBool("Idle", true);
                    }
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
        for (int i = 0; i < closePart.Length; i++)
        {
            closePart[i].SetActive(false);
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
        mainObje.SetActive(false);
        //mainObje.GetComponent<CloseDelay>().CloseObje();
    }

    private void EnemyHit()
    {
        if (_playerController != null)
        {
            _playerController.HitTaken(enemyDamage);
        }
    }
}
