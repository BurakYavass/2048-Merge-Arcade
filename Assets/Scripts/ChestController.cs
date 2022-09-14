using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    [SerializeField]
    GameObject _MainObje;

    [SerializeField]
    GameObject _CreatBall;
    [SerializeField]
    float _CreatValue;
    [SerializeField]
    float _CreatCount;
    [SerializeField]
    GameObject[] _ClosePart; 
    [SerializeField]
    GameObject[] _OpenPart;
    [SerializeField]
    float _ChestHealthValue; 
   
    float _ChestHealthValueCurrent;

    float _ChestHealthValueCurrentTemp;
    [SerializeField] private Animator animator;
    RuntimeAnimatorController _None;
 
    float _TempDamage;
    [SerializeField] GameObject _HealthBar;
    [SerializeField]
    TextMeshProUGUI _FullHealth;
    [SerializeField]
    TextMeshProUGUI _CurrentHealth;
    [SerializeField]
    Image _SliderValue;
    bool _Hitting;
    void Start()
    {
        _ChestHealthValueCurrent = _ChestHealthValue;
        _FullHealth.text = (Mathf.Round(_ChestHealthValue)).ToString();
        _CurrentHealth.text = (Mathf.Round(_ChestHealthValueCurrent)).ToString();
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
                StartCoroutine(CloseDelay());
                for (int i = 0; i < _ClosePart.Length; i++)
                {
                    _ClosePart[i].transform.localScale = Vector3.Lerp(_ClosePart[i].transform.localScale,Vector3.zero,.5f);
          
                } 
                for (int i = 0; i < _OpenPart.Length; i++)
                {
                    _OpenPart[i].SetActive(true);
                }
               
                
            }
             
        }
    }
  public void Hit(float damage)
    {
        _TempDamage = damage;
        _ChestHealthValueCurrentTemp = _ChestHealthValueCurrent - _TempDamage;
        _Hitting = true;
        _HealthBar.SetActive(true);
        StartCoroutine(HitClose());
    }

    IEnumerator HitClose()
    {
        animator.SetBool("Hit",true);
        yield return new WaitForSeconds(7);
        animator.SetBool("Hit",false);
        _Hitting = false;
        _HealthBar.SetActive(false);
    }

    IEnumerator CloseDelay()
    {
        _MainObje.GetComponent<CloseDelay>().CloseObje();
        animator.SetBool("Hit",false);
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < _ClosePart.Length; i++)
        {
            _ClosePart[i].SetActive(false);
        }
        for (int i = 0; i < _CreatCount; i++)
        {
          //  GameObject go = Instantiate(_CreatBall, transform.position + new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2)), Quaternion.Euler(0,180,0));
            GameObject go = Instantiate(_CreatBall, transform.position, Quaternion.Euler(0,180,0));
            go.GetComponent<Ball>();
            go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,5), Random.Range(0, 5), Random.Range(0, 5))) ;

        }
    }
}
