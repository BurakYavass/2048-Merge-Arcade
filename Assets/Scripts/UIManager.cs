using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject joyStickPanel;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedLevel;
    [SerializeField] private TextMeshProUGUI armorLevel;
    [SerializeField] private TextMeshProUGUI damageLevel;
    

    
    public void UpgradePanel(bool openClose)
    {
        if (openClose)
        {
            upgradePanel.SetActive(true);
            joyStickPanel.SetActive(false);
            upgradePanel.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            joyStickPanel.SetActive(true);
            upgradePanel.transform.DOScale(Vector3.zero, 0.5f)
                        .OnComplete((() =>
                        {
                            upgradePanel.SetActive(false);
                            joyStickPanel.SetActive(true);
                        }));
        }
    }
}
