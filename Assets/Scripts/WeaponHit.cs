using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    [SerializeField]
    float _DamageValue;

    GameObject _Player;
    bool _HitDelay;
    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            other.gameObject.GetComponent<ChestController>().Hit(_DamageValue);
           // GetComponent<Collider>().enabled = false;
          //  StartCoroutine(HitDelay());
            _HitDelay = true;
        }
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
        _HitDelay = false;
    }
}
