using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private WeaponsHit _weaponsHit;
    [SerializeField] private BallController _ballController;
    private ChestController _chestController;

    private bool _onUpgrade = false;
    private bool _onMergeMachine = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EmptyBall"))
        {
            other.gameObject.GetComponent<Ball>().SetGoTarget(_ballController.LastObje());
            other.tag = "StackBall";
        }

        if (other.CompareTag("BaseRight"))
        {
            GameEventHandler.current.PlayerRightArea(true);
        }
        
        if (other.CompareTag("BaseLeft"))
        {
            GameEventHandler.current.PlayerLeftArea(true);
        }
        
        if (other.CompareTag("MergeMachine"))
        {
            if (!_onMergeMachine)
            {
                _onMergeMachine = true;
                _ballController.GoMerge();
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            if (!_onUpgrade)
            {
                _onUpgrade = true;
                _ballController.GoUpgrade();
                GameEventHandler.current.PlayerUpgradeArea(true);
            }
        }
    }

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
        
        if (other.CompareTag("BaseRight"))
        {
            GameEventHandler.current.PlayerRightArea(false);
        }
        
        if (other.CompareTag("BaseLeft"))
        {
            GameEventHandler.current.PlayerLeftArea(false);
        }

        if (other.CompareTag("MergeMachine"))
        {
            _onMergeMachine = false;
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            _onUpgrade = false;
            GameEventHandler.current.PlayerUpgradeArea(false);
        }
    }
    
    private void WeaponHit()
    {
        _chestController.Hit(_weaponsHit._DamageValue);
        GameEventHandler.current.PlayerHit(false);
    }
}
