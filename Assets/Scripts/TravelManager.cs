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

    private Collider _collider;
    private Transform _travelPoint;

    public bool active;
    
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
        }
        else
        {
            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var travelPos = _travelPoint.transform.position;
            var player = other.GetComponent<PlayerCollisionHandler>();
            player.closePart[0].SetActive(false);
            player.closePart[1].SetActive(false);
            player.gameObject.transform.DOMove(new Vector3(travelPos.x,travelPos.y+0.5f,travelPos.z), 1.0f)
                .OnComplete((() =>
                {
                    player.closePart[0].SetActive(true);
                    player.closePart[1].SetActive(true);
                }));
        }
    }

    public enum TravelType
    {
        YellowStage,
        GreenStage,
        PurpleStage,
    }
}
