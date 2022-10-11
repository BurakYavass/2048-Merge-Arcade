using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Transform rayCastObject;
    private PlayerController _playerController;

    [SerializeField] private float enemyDamage;
    [SerializeField] private int[] creatValue;
    [SerializeField] private float creatCount;
    [SerializeField] private float enemyHealthValue; 
    public float enemyHealthValueCurrent;
    private float _enemyHealthValueCurrentTemp;
    private float _tempDamage;
    
    private bool _getHitting;
    private bool _hitting = false;
    
    public float wanderRadius;
    public float wanderTimer;
 
    private Transform target;
    private float timer;
    
    void OnEnable () {
        
    }
    
    void Start()
    {
        enemyHealthValueCurrent = enemyHealthValue;
        fullHealth.text = (Mathf.Round(enemyHealthValue)).ToString();
        currentHealth.text = (Mathf.Round(enemyHealthValueCurrent)).ToString();
        _playerController = PlayerController.Current;
        wanderTimer = Random.Range(3, 10);
        timer = wanderTimer;
    }

 
    void Update()
    {
        timer += Time.deltaTime;
        
        if (_getHitting)
        {
            if (enemyHealthValueCurrent>=1)
            {
                enemyHealthValueCurrent = Mathf.Lerp(enemyHealthValueCurrent, _enemyHealthValueCurrentTemp, .1f);
                currentHealth.text = enemyHealthValueCurrent.ToString("00");
                sliderValue.fillAmount = (1 * (enemyHealthValueCurrent / enemyHealthValue));
            }
            else
            {
                var collider = GetComponent<Collider>();
                collider.isTrigger = false;
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

        var distance = Vector3.Distance(transform.position, _playerController.transform.position);
        if (distance < 15.0f && !_hitting)
        {
            healthBar.SetActive(true);
            enemyAgent.destination = _playerController.transform.position;
            enemyAnimator.SetBool("Walking",true);
            enemyAnimator.SetBool("Idle",false);
        }
        else
        {
            healthBar.SetActive(false);
            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                enemyAgent.SetDestination(newPos);
                enemyAnimator.SetBool("Idle",false);
                enemyAnimator.SetBool("Attack",false);
                enemyAnimator.SetBool("Walking",true);
                timer = 0;
            }
            else
            {
                enemyAnimator.SetBool("Walking",false);
                enemyAnimator.SetBool("Attack",false);
                enemyAnimator.SetBool("Idle",true);
            }
        }
    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
    
    public void GetHit(float damage)
    {
        if (gameObject.activeInHierarchy)
        {
            _tempDamage = damage;
            _enemyHealthValueCurrentTemp = Mathf.Clamp(enemyHealthValueCurrent - _tempDamage, 0, enemyHealthValue);
            _getHitting = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerTransform = other.transform.position;
            enemyAgent.transform.LookAt(playerTransform,Vector3.up);
            enemyAnimator.SetBool("Walking",false);
            enemyAnimator.SetBool("Idle",false);
            enemyAnimator.SetBool("Attack",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAgent.updateRotation = true;
            enemyAgent.isStopped = false;
            enemyAnimator.SetBool("Attack",false);
            _hitting = false;
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
            //go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,3), Random.Range(3, 5)*3, Random.Range(0, 3)));
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

    private void WeaponHit()
    {
        if (_playerController != null)
        {
            _playerController.Hit(enemyDamage);
            _playerController.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).SetEase(Ease.InBounce);
        }
    }
}
