using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHit : MonoBehaviour
{
    [SerializeField] public float _DamageValue;
    public WeaponState weapon;
   
    //bool _HitDelay; 

    private void Start()
    {
        Weapons(WeaponState.Axe1);
    }

    private void Weapons(WeaponState weapons)
    {
        weapon = weapons;
        switch (weapons)
        {
            case WeaponState.Sword1:
                _DamageValue = 10;
                break;
            case WeaponState.Sword2:
                _DamageValue = 20;
                break;
            case WeaponState.Gurz:
                _DamageValue = 10;
                break;
            case WeaponState.Axe1:
                _DamageValue = 10;
                break;
            case WeaponState.Axe2:
                _DamageValue = 10;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
        //_HitDelay = false;
    }

    public enum WeaponState
    {
        Sword1,
        Sword2,
        Gurz,
        Axe1,
        Axe2,
    }
}
