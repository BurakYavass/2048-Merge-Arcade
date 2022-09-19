using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;

    public void UpgradePanel(bool openClose)
    {
        if (openClose)
        {
            upgradePanel.SetActive(true);
            upgradePanel.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            upgradePanel.transform.DOScale(Vector3.zero, 0.5f)
                        .OnComplete((() => upgradePanel.SetActive(false)));
        }
        
    }
}
