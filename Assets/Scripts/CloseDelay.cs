using System.Collections;
using UnityEngine;

public class CloseDelay : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f;
    public void CloseObje()
    {
        StartCoroutine(DelayClose());
    }
    IEnumerator DelayClose()
    {

        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
