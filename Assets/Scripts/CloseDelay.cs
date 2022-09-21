using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDelay : MonoBehaviour
{
   
    public void CloseObje()
    {
        StartCoroutine(DelayClose());
    }
    IEnumerator DelayClose()
    {

        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
