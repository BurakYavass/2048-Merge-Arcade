using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TravelManager : MonoBehaviour
{
    public TravelType travelType;
    [SerializeField] private Transform yellowLevel;
    [SerializeField] private Transform greenLevel;
    [SerializeField] private Transform purpleLevel;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject[] openPart;
    [SerializeField] private GameObject firstTimePlay;
    [SerializeField] private BallController ballController;

    [SerializeField] private float travelDelay;

    private Collider _collider;
    private Transform _travelPoint;

    public bool active;
    private PlayerCollisionHandler _player;

    void Start()
    {
        _collider = GetComponent<Collider>();
        
        if (travelType == TravelType.YellowStage)
        {
            _travelPoint = yellowLevel;
        }
        else if (travelType == TravelType.GreenStage)
        {
            _travelPoint = greenLevel;
        }
        else if (travelType == TravelType.PurpleStage)
        {
            _travelPoint = purpleLevel;
        }
    }
    
    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            firstTimePlay.SetActive(true);
           
            _player = other.GetComponent<PlayerCollisionHandler>();
            _player.closePart[0].SetActive(false);
            _player.closePart[1].SetActive(false);
            StartCoroutine(TravelWaiter());
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
        _player.gameObject.transform.DOMove(new Vector3(travelPos.x,travelPos.y+0.6f,travelPos.z), 2.0f)
            .OnComplete((() =>
            {
                firstTimePlay.SetActive(false);
                _player.transform.forward = Vector3.forward;
                ballController.GoTravelPoint();
                _player.closePart[0].SetActive(true);
                _player.closePart[1].SetActive(true);
            }));
        
    }
}
