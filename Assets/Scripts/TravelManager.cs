using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TravelManager : MonoBehaviour
{
    public TravelType travelType;
    [SerializeField] private Transform yellowLevel;
    [SerializeField] private Transform greenLevel;
    [SerializeField] private Transform purpleLevel;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject[] openPart;
    [SerializeField] private GameObject firstTimePlay;
    [SerializeField] private BallController ballController;
    [SerializeField] private float travelDelay;

    private Collider _collider;
    private Transform _travelPoint;

    public bool active;
    private bool _once = false;
    private PlayerController _player;
    

    void Start()
    {
        PlayerPrefs.GetInt("yellow", 0);
        PlayerPrefs.GetInt("purple", 0);
        _collider = GetComponent<Collider>();
        
        if (travelType == TravelType.YellowStage)
        {
            _travelPoint = yellowLevel;
            if (PlayerPrefs.GetInt("yellow")==1)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }
        else if (travelType == TravelType.GreenStage)
        {
            _travelPoint = greenLevel;
            
        }
        else if (travelType == TravelType.PurpleStage)
        {
            _travelPoint = purpleLevel;
            if (PlayerPrefs.GetInt("purple") == 1)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }

        if (active)
        {
            _collider.enabled = true;
            foreach (var particle in openPart)
            {
                particle.SetActive(true);
            }
        }
        else
        {
            foreach (var particle in openPart)
            {
                particle.SetActive(false);
            }
            _collider.enabled = false;
        }
    }
    
    private void Update()
    {
        if (active)
        {
            if (!_once)
            {
                _collider.enabled = true;
                foreach (var particle in openPart)
                {
                    particle.SetActive(true);
                }

                if (travelType == TravelType.YellowStage)
                {
                    PlayerPrefs.SetInt("yellow", 1);
                }
                else if (travelType == TravelType.PurpleStage)
                {
                    PlayerPrefs.SetInt("purple",1);
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            firstTimePlay.SetActive(true);
            _player = other.GetComponent<PlayerController>();
            _player.OnPlayerTeleport(true);
            foreach (var closeObject in _player.closePart)
            {
                closeObject.SetActive(false);
            }
            StartCoroutine(TravelWaiter());
            fillImage.DOFillAmount(1, travelDelay).OnComplete((() => fillImage.fillAmount = 0));
            firstTimePlay.transform.DOLocalMoveY(0.43f,travelDelay);
            
        }
    }

    public enum TravelType
    {
        YellowStage,
        GreenStage,
        PurpleStage,
    }

    IEnumerator TravelWaiter()
    {
        yield return new WaitForSeconds(travelDelay);
        var travelPos = _travelPoint.transform.position;
        _player.gameObject.transform.DOMove(new Vector3(travelPos.x,travelPos.y+.5f,travelPos.z), 2.0f)
            .OnComplete((() =>
            {
                firstTimePlay.SetActive(false);
                _player.transform.forward = Vector3.forward;
                ballController.GoTravelPoint();
                _player.OnPlayerTeleport(false);
                foreach (var closeObject in _player.closePart)
                {
                    closeObject.SetActive(true);
                }
            }));
        
    }
}
