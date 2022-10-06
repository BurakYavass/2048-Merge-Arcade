using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject levelUnlockPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject joyStickPanel;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedLevel;
    [SerializeField] private TextMeshProUGUI armorLevel;
    [SerializeField] private TextMeshProUGUI damageLevel;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button armorButton;


    private void Update()
    {
        if (!gameManager.damageMax)
        {
            damageLevel.text = "Level - " + (gameManager._damageState).ToString();
            damageText.text = gameManager.ReturnDamageState().ToString();
        }
        else
        {
            damageLevel.text = "Max";
            damageText.text = "Max";
            damageButton.enabled = false;
        }

        if (!gameManager.armorMax)
        {
            armorLevel.text = "Level - " + (gameManager._armorState);
            armorText.text = gameManager.ReturnArmorState().ToString();
        }
        else
        {
            armorLevel.text = "Max";
            armorText.text = "Max";
            armorButton.enabled = false;
        }

        if (!gameManager.speedMax)
        {
            speedLevel.text = "Level - " + (gameManager._speedState).ToString();
            speedText.text = gameManager.ReturnSpeedState().ToString();
        }
        else
        {
            speedLevel.text = "Max";
            speedText.text = "Max";
            speedButton.enabled = false;
        }
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
