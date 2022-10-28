using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChestController : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private TutorialStage tutorialStage;
    [SerializeField] private GameObject _MainObje;
    [SerializeField] private GameObject _CreatBall;
    [SerializeField] private GameObject[] _ClosePart; 
    [SerializeField] private GameObject[] _OpenPart;
    [SerializeField] private GameObject _HealthBar;
    [SerializeField] private TextMeshProUGUI _FullHealth;
    [SerializeField] private TextMeshProUGUI _CurrentHealth;
    [SerializeField] private Image _SliderValue;
    
    [SerializeField] private int[] _CreatValue;
    [SerializeField] private float _CreatCount;
    [SerializeField] private float _ChestHealthValue; 
    public float chestHealthCurrent;
    private float _chestHealthValueCurrentTemp;
    private float _tempDamage;

    public bool chestClose = false;
    private bool _hitTake;
    private bool _playerHit;
    public bool tutorial;
    void Start()
    {
        chestHealthCurrent = _ChestHealthValue;
        _FullHealth.text = (Mathf.Round(_ChestHealthValue)).ToString();
        _CurrentHealth.text = (Mathf.Round(chestHealthCurrent)).ToString("0");
        _playerTransform = PlayerController.Current.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitPoint"))
        {
            GameEventHandler.current.OnPlayerHit += HitTaken;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitPoint"))
        {
            GameEventHandler.current.OnPlayerHit -= HitTaken;
        }
    }

    private void HitTaken(float damage)
    {
        _tempDamage = damage;
        _chestHealthValueCurrentTemp = Mathf.Clamp(chestHealthCurrent - _tempDamage, 0, _ChestHealthValue);
        _hitTake = true;
        transform.DOKill();
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce).OnComplete((() =>
        {
            transform.localScale = Vector3.one;
        }));
    }


    void Update()
    {
        if (_hitTake)
        {
            if (chestHealthCurrent>=1)
            {
                chestHealthCurrent = Mathf.Lerp(chestHealthCurrent, _chestHealthValueCurrentTemp, .1f);
                _CurrentHealth.text = chestHealthCurrent.ToString("0");
                _SliderValue.fillAmount = (1 * (chestHealthCurrent / _ChestHealthValue));
            }
            else
            {
                if (tutorial)
                {
                    tutorial = false;
                    TutorialControl.Instance.CompleteStage(tutorialStage);
                }
                chestClose = true;
                var collider = GetComponent<Collider>();
                collider.isTrigger = false;
                StartCoroutine(CloseDelay());
                transform.DOScale(Vector3.zero, 0.5f);
                foreach (var close in _ClosePart)
                {
                    close.transform.localScale = Vector3.Lerp(close.transform.localScale,Vector3.zero,.1f);
                } 
                foreach (var open in _OpenPart)
                {
                    open.SetActive(true);
                }
            }
        }

        var distance = Vector3.Distance(transform.position, _playerTransform.position);
        if (distance < 10f)
        {
            _HealthBar.SetActive(true);
        }
        else
        {
            _HealthBar.SetActive(false);
        }
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < _ClosePart.Length; i++)
        {
            _ClosePart[i].SetActive(false);
        }
        for (int i = 0; i < _CreatCount; i++)
        {
            GameObject go = Instantiate(_CreatBall, transform.position, Quaternion.LookRotation(Vector3.forward));
            go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,2), Random.Range(1, 3)*3, Random.Range(0, 2)),ForceMode.Impulse);
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
