using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager current;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject levelUnlockPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject joyStickPanel;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button armorButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private float delay;

    [SerializeField] private List<GameObject> armorLevelImage;
    [SerializeField] private List<GameObject> damageLevelImage;
    [SerializeField] private List<GameObject> speedLevelImage;
    
    [Header("Home Teleport")]
    [SerializeField] private GameObject homeTravelPanel;
    [SerializeField] private Button homeTravelButton;
    

    [Header("Revive Canvas")] 
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private GameObject reviveCanvas;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Image filledImage;


    private bool _once = false;
    public bool tutorial;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    private void Start()
    {
        PlayerPrefs.GetInt("uITutorial", 0);
        
        if (PlayerPrefs.GetInt("uITutorial") == 1)
        {
            tutorial = false;
        }
        else
        {
            tutorial = true;
        }
    }


    private void Update()
    {
        if (!gameManager.damageMax)
        {
            damageText.text = gameManager.ReturnDamageState().ToString();
            var damageState = gameManager._damageState;
            if (damageState == 0)
            {
                foreach (var image in damageLevelImage)
                {
                    image.SetActive(false);
                }
            }
            else if(damageState == 1)
            {
                var first3 = damageLevelImage.Take(3);
                foreach (var level1 in first3)
                {
                    level1.SetActive(true);
                }
            }
            else if(damageState == 2)
            {
                var first6 = damageLevelImage.Take(6);
                foreach (var level2 in first6)
                {
                    level2.SetActive(true);
                }
            }
            
        }
        else
        {
            for (int i = 0; i < damageLevelImage.Count; i++)
            {
                damageLevelImage[i].SetActive(true);
            }
            damageText.text = "Max";
            damageButton.enabled = false;
        }

        if (!gameManager.armorMax)
        {
            armorText.text = gameManager.ReturnArmorState().ToString();
            var armorState = gameManager._armorState;
            if (armorState == 0)
            {
                foreach (var image in armorLevelImage)
                {
                    image.SetActive(false);
                }
            }
            else if(armorState == 1)
            {
                var first3 = armorLevelImage.Take(3);
                foreach (var level1 in first3)
                {
                    level1.SetActive(true);
                }
            }
            else if(armorState == 2)
            {
                var first6 = armorLevelImage.Take(6);
                foreach (var level2 in first6)
                {
                    level2.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < armorLevelImage.Count; i++)
            {
                armorLevelImage[i].SetActive(true);
            }
            armorText.text = "Max";
            armorButton.enabled = false;
        }

        if (!gameManager.speedMax)
        {
            //speedLevel.text = "Level - " + (gameManager._speedState);
            speedText.text = gameManager.ReturnSpeedState().ToString();
            var speedState = gameManager._speedState;
            if (speedState == 0)
            {
                foreach (var image in speedLevelImage)
                {
                    image.SetActive(false);
                }
            }
            else if(speedState == 1)
            {
                var first3 = speedLevelImage.Take(3);
                foreach (var level1 in first3)
                {
                    level1.SetActive(true);
                }
            }
            else if(speedState == 2)
            {
                var first6 = speedLevelImage.Take(6);
                foreach (var level2 in first6)
                {
                    level2.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < speedLevelImage.Count; i++)
            {
                speedLevelImage[i].SetActive(true);
            }
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
            if (tutorial)
            {
                tutorial = false;
                PlayerPrefs.SetInt("uITutorial",1);
                armorButton.transform.DOPunchScale(new Vector3(0.1f,0.1f,0.1f), 0.5f).SetLoops(-1);
            }
            StartCoroutine(CloseButtonDelay());
        }
        else
        {
            StopCoroutine(CloseButtonDelay());
            upgradePanel.transform.DOScale(Vector3.zero, 0.5f)
                        .OnComplete((() =>
                        {
                            closeButton.SetActive(false);
                            upgradePanel.SetActive(false);
                            joyStickPanel.SetActive(true);
                        }));
        }
    }

    IEnumerator CloseButtonDelay()
    {
        yield return new WaitForSeconds(delay);
        closeButton.SetActive(true);
    }

    public void PlayerRevive(bool openClose,float delay)
    {
        if (openClose)
        {
            reviveCanvas.SetActive(true);
            counterText.text = delay.ToString();
            revivePanel.transform.DOScale(Vector3.one, 0.5f)
                                .OnComplete((() =>
                                {
                                    var text = delay;
                                    filledImage.DOFillAmount(delay, delay)
                                                    .OnUpdate((() => counterText.text = text.ToString("0")));;
                                    DOTween.To(x => text = x, text, 0, delay);

                                }));
        }
        else
        {
            
            revivePanel.transform.DOScale(Vector3.zero, 0.5f)
                .OnComplete((() =>
                {
                    filledImage.fillAmount = 0;
                    reviveCanvas.SetActive(false);
                }));
        }
    }


    public void PlayerHomeTravelButton(bool openClose)
    {
        if (openClose)
        {
            homeTravelPanel.SetActive(true);
            if (!_once)
            {
                _once = true;
                homeTravelPanel.transform.DOKill();
                homeTravelPanel.transform.DOMoveX(0, 1f).SetEase(Ease.OutBounce)
                                            .OnComplete((() => homeTravelButton.enabled = true));
            }
        }
        else
        {
            homeTravelButton.enabled = false;
            if (_once)
            {
                _once = false;
                homeTravelPanel.transform.DOKill();
                homeTravelPanel.transform.DOMoveX(-240f, .5f).SetEase(Ease.Flash)
                    .OnComplete((() => homeTravelPanel.SetActive(false)));
            }
            
        }
    }

    public void ArmorTutorial()
    {
        armorButton.transform.DOKill();
        TutorialControl.Instance.CompleteStage(TutorialStage.Upgrade);
    }

}
