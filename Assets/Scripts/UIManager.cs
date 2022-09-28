using System;
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


    private void Update()
    {
        speedText.text = GameManager.current.ReturnSpeedState().ToString();
        armorText.text = GameManager.current.ReturnArmorState().ToString();
        damageText.text = GameManager.current.ReturnDamageState().ToString();
        speedLevel.text = (GameManager.current._speedState+1).ToString();
        armorLevel.text = (GameManager.current._armorState+1).ToString();
        damageLevel.text = (GameManager.current._damageState+1).ToString();
    }

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
                        .OnComplete((() =>
                        {
                            upgradePanel.SetActive(false);
                            joyStickPanel.SetActive(true);
                        }));
        }
    }
}
