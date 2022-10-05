using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHit : MonoBehaviour
{
    public float _damageValue;
    public WeaponState weapon;

    private void Start()
    {
        
        //Weapons(WeaponState.Axe1);
    }

    private void Update()
    {
        _damageValue = GameManager.current.playerDamage;
    }

    private void Weapons(WeaponState weapons)
    {
        weapon = weapons;
        switch (weapons)
        {
            case WeaponState.Sword1:
                _damageValue = 5.01f;
                break;
            case WeaponState.Sword2:
                _damageValue = 10.01f;
                break;
            case WeaponState.Gurz:
                _damageValue = 15.01f;
                break;
            case WeaponState.Axe1:
                _damageValue = 40.01f;
                break;
            case WeaponState.Axe2:
                _damageValue = 50.01f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

    // IEnumerator HitDelay()
    // {
    //     yield return new WaitForSeconds(1);
    //     GetComponent<Collider>().enabled = true;
    //     //_HitDelay = false;
    // }

    public enum WeaponState
    {
        Sword1,
        Sword2,
        Gurz,
        Axe1,
        Axe2,
    }
}
