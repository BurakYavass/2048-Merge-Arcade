using System.Collections;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject _MainObje;

    [SerializeField] private GameObject _CreatBall;
    [SerializeField] private GameObject[] _ClosePart; 
    [SerializeField] private GameObject[] _OpenPart;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private GameObject _HealthBar;
    [SerializeField] private TextMeshProUGUI _FullHealth;
    [SerializeField] private TextMeshProUGUI _CurrentHealth;
    [SerializeField] private Image _SliderValue;

    private Transform playerTransform;
    
    [SerializeField] private float[] _CreatValue;
    [SerializeField] private float _CreatCount;
    [SerializeField] private float _ChestHealthValue; 
    private float _ChestHealthValueCurrent;
    private float _ChestHealthValueCurrentTemp;
    private float _TempDamage;
    
    bool _Hitting;
    void Start()
    {
        _ChestHealthValueCurrent = _ChestHealthValue;
        _FullHealth.text = (Mathf.Round(_ChestHealthValue)).ToString();
        _CurrentHealth.text = (Mathf.Round(_ChestHealthValueCurrent)).ToString();
        playerTransform = PlayerController.Current.transform;
    }

 
    void Update()
    {
        if (_Hitting)
        {
            if (_ChestHealthValueCurrent>=1)
            {
                _ChestHealthValueCurrent = Mathf.Lerp(_ChestHealthValueCurrent, _ChestHealthValueCurrentTemp, .1f);
                _CurrentHealth.text = (Mathf.Round(_ChestHealthValueCurrent)).ToString();
                _SliderValue.fillAmount = (1 * (_ChestHealthValueCurrent / _ChestHealthValue));
            }
            else
            {
                var collider = GetComponent<Collider>();
                collider.isTrigger = false;
                StartCoroutine(CloseDelay());
                transform.DOScale(Vector3.zero, 1f);
                for (int i = 0; i < _ClosePart.Length; i++)
                {
                    _ClosePart[i].transform.localScale = Vector3.Lerp(_ClosePart[i].transform.localScale,Vector3.zero,.1f);
          
                } 
                for (int i = 0; i < _OpenPart.Length; i++)
                {
                    _OpenPart[i].SetActive(true);
                }
            }
        }

        var distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < 10f)
        {
            _HealthBar.SetActive(true);
        }
        else
        {
            _HealthBar.SetActive(false);
        }
    }
    public void Hit(int damage)
    {
        if (gameObject.activeInHierarchy)
        {
            _TempDamage = damage;
            _ChestHealthValueCurrentTemp = _ChestHealthValueCurrent - _TempDamage;
            _Hitting = true;
            //_HealthBar.SetActive(true);
            StartCoroutine(HitClose());
        }
    }

    IEnumerator HitClose()
    {
        animator.SetBool("Hit",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Hit",false);
        _Hitting = false;
    }

    IEnumerator CloseDelay()
    {
        animator.SetBool("Hit",false);
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < _ClosePart.Length; i++)
        {
            _ClosePart[i].SetActive(false);
        }
        for (int i = 0; i < _CreatCount; i++)
        {
            GameObject go = Instantiate(_CreatBall, transform.position, Quaternion.Euler(0,180.0f,0));
            go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,3), Random.Range(3, 5), Random.Range(0, 3)));
            if (_CreatValue.Length > 1)
            {
                int rdnm=Random.Range(0,100);
                if (rdnm>=70)
                {
                    go.GetComponent<Ball>().SetValue(_CreatValue[0]);
    
                }
                else
                {
                    go.GetComponent<Ball>().SetValue(_CreatValue[1]);

                }
            }
            else
            {
                go.GetComponent<Ball>().SetValue(_CreatValue[0]);
            }
            
        }
        _MainObje.GetComponent<CloseDelay>().CloseObje();
    }
}
