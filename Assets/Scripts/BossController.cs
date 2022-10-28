using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
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
    [SerializeField] private float enemyDamage;
    [SerializeField] private int[] creatValue;
    [SerializeField] private float creatCount;
    [SerializeField] private float enemyHealthValue; 
    
    private Transform _playerTransform;
    private PlayerController _playerController;
    private Transform _target;
    private Vector3 _newPos;
    
    public float enemyHealthValueCurrent;

    [Header("Boss")]
    private bool _bossActive;
    [SerializeField] public GameObject bossJumpArea;
    [SerializeField] private float bossSkillCooldown;
    [SerializeField] public RectTransform fillImage;
    [SerializeField] public ParticleSystem dustParticle;

    private float _enemyHealthValueCurrentTemp;
    private float _tempDamage;
    private float _timer;
    private bool _getHitting;
    private bool _hittable;
    private bool _playerHit;
    public bool skillActive = false;
    private bool _once = false;
    private bool _bossDie;

    void Start()
    {
        enemyHealthValueCurrent = enemyHealthValue;
        fullHealth.text = (Mathf.Round(enemyHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(enemyHealthValueCurrent)).ToString("0");
        _playerController = PlayerController.Current;
        StartCoroutine(SkillCooldown());
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
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _hittable = true;
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
            _hittable = false;
            enemyAnimator.SetBool("Attack" , false);
        }
    }
    
    private void HitTaken(float damage)
    {
        _tempDamage = damage;
        bloodParticle.Play();
        _enemyHealthValueCurrentTemp = Mathf.Clamp(enemyHealthValueCurrent - _tempDamage, 0, enemyHealthValue);
        _getHitting = true;
        transform.DOKill();
        var monsterScale = transform.lossyScale;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce).OnComplete((() =>
        {
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
                healthBar.SetActive(false);
                var colliders = GetComponents<Collider>();
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
                enemyAnimator.SetBool("Dying",true);
                if (!_bossDie)
                {
                    _bossDie = true;
                    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Boss");
                }
            }
        }

        if (_playerController.playerCollisionHandler.inBossArea && !_playerController.playerDie && !_bossDie)
        {
            enemyAnimator.SetBool("BossArea",true);
            //var playerPosition = _playerTransform.position;
            
            var anim = enemyAnimator.GetAnimatorTransitionInfo(0).IsUserName("walking");
            healthBar.SetActive(true);
            if (anim)
            {
                _bossActive = true;
            }
            
            if (_bossActive)
            {
                if (!skillActive)
                {
                    enemyAgent.enabled = true;
                    if (enemyAgent.enabled)
                    {
                        enemyAgent.destination = _playerController.transform.position;
                    }
                    
                    fillImage.transform.DOKill();
                    transform.DOKill();

                    if (_hittable)
                    {
                        // var direction = _playerController.transform.position - transform.position;
                        // var playerLook = Quaternion.LookRotation(direction,Vector3.up);
                        // var lerp = Quaternion.Lerp(transform.rotation, playerLook, 1f * Time.deltaTime);
                        // transform.rotation = lerp;
                        enemyAnimator.SetBool("Attack" , true);
                        enemyAgent.enabled = false;
                    }
                }
                else
                {
                    if (!_once)
                    {
                        transform.LookAt(_playerController.transform.position);
                        enemyAnimator.SetBool("Attack" , false);
                        enemyAnimator.SetTrigger("JumpAttack");
                        enemyAgent.enabled = false;
                        StartCoroutine(SkillCooldown());
                        _once = true;
                    }
                }
            }
        }
        else
        {
            fillImage.localScale = Vector3.zero;
            _bossActive = false;
            enemyAgent.enabled = false;
            healthBar.SetActive(false);
            bossJumpArea.SetActive(false);
            enemyAnimator.SetBool("BossArea",false);
            enemyAnimator.SetBool("Attack", false);
        }
    }
    
    
    private void BossDying()
    {
        
        StartCoroutine(CloseDelay());
        for (int i = 0; i < closePart.Length; i++)
        {
            closePart[i].transform.localScale = Vector3.Lerp(closePart[i].transform.localScale,Vector3.zero,.1f);
          
        } 
        for (int i = 0; i < openPart.Length; i++)
        {
            openPart[i].SetActive(true);
        } 
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
        if (_hittable)
        {
            _playerController.HitTaken(enemyDamage);
        }
    }

    private void ParticlePlay()
    {
        dustParticle.Play();
    }

   

    IEnumerator SkillCooldown()
    {
        fillImage.transform.DOKill();
        transform.DOKill();
        yield return new WaitForSeconds(bossSkillCooldown);
        skillActive = true;
        _once = false;
    }
}
