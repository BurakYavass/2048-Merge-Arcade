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

    private Transform _travelPoint;
    
    
    // Start is called before the first frame update
    void Start()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var travelPos = _travelPoint.transform.position;
            other.gameObject.SetActive(false);
            other.gameObject.transform.DOMove(new Vector3(travelPos.x,travelPos.y+0.5f,travelPos.z), 1.0f)
                .OnComplete((() => other.gameObject.SetActive(true)));
        }
    }

    public enum TravelType
    {
        YellowStage,
        GreenStage,
        PurpleStage,
    }
}
