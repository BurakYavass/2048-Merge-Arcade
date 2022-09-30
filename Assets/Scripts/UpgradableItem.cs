using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradableArmor;
    [SerializeField] private GameObject[] upgradableWeapon;

    public void ArmorChanger(int armor)
    {
        if (armor > 0)
        {
            upgradableArmor[armor - 1].SetActive(false);
            upgradableArmor[armor].SetActive(true);
        }
        else
        {
            upgradableArmor[armor].SetActive(true);
        }
    }

    public void WeaponChanger(int weapon)
    {
        if (weapon > 0)
        {
            upgradableWeapon[weapon - 1].SetActive(false);
            upgradableWeapon[weapon].SetActive(true);
        }             
        else          
        {             
            upgradableWeapon[weapon].SetActive(true);
        }
    }
}
