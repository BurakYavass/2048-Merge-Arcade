using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private WeaponsHit _weaponsHit;
    private ChestController _chestController;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            if (other.gameObject.activeInHierarchy)
            {
                _chestController= other.GetComponent<ChestController>();
                GameEventHandler.current.PlayerHit(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            GameEventHandler.current.PlayerHit(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EmptyBall"))
        {
            Debug.Log("Ball");
        }
    }

    private void WeaponHit()
    {
        _chestController.Hit(_weaponsHit._DamageValue);
        GameEventHandler.current.PlayerHit(false);
    }
}
