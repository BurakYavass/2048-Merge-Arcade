using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TriggerArea : MonoBehaviour
{
    public enum Area
    {
        Upgrade,
        Merge,
        BaseRight,
        BaseLeft,
    }

    public Area areaType;
    private PlayerController _playerController;
    [SerializeField] private Image filledImage;
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private BallController ballController;
    private PlayerBallCounter _playerBallCounter;
    private Coroutine _upgradeTriggerDelay;
    private Coroutine _mergeTriggerDelay;
    
    private bool _upgradeArea;
    private bool _mergeMachine;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            _playerBallCounter = _playerController.GetComponent<PlayerBallCounter>();
            if (areaType == Area.Upgrade)
            {
                if (!_upgradeArea)
                {
                    _upgradeTriggerDelay = StartCoroutine(UpgradeAreaDelay(true));
                }
            }
            else if(areaType == Area.Merge)
            {
                if (!_mergeMachine)
                {
                    _mergeMachine = true;
                    if (ballController.balls.Count > 1)
                    {
                        _mergeTriggerDelay = StartCoroutine(MergeAreaDelay(true));
                            
                    }
                }
            }
            else if (areaType == Area.BaseRight)
            {
                gameEventHandler.PlayerRightArea(true);
            }
            else if (areaType == Area.BaseLeft)
            {
                gameEventHandler.PlayerLeftArea(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (areaType == Area.Merge)
            {
                filledImage.fillAmount = 0;
                if (_mergeTriggerDelay != null)
                {
                    StopCoroutine(_mergeTriggerDelay);
                }
                filledImage.DOKill();
                _mergeMachine = false;
            }
            else if (areaType == Area.Upgrade)
            {
                filledImage.fillAmount = 0;
                _playerBallCounter.stackValue = 0;
                StopCoroutine(_upgradeTriggerDelay);
                filledImage.DOKill();
                _upgradeArea = false;
            }
            else if (areaType == Area.BaseRight)
            {
                gameEventHandler.PlayerRightArea(false);
            }
            else if (areaType == Area.BaseLeft)
            {
                gameEventHandler.PlayerLeftArea(false);
            }
        }
    }
    
    public void ExitUpgrade()
    {
        _playerBallCounter.stackValue = 0;
        _upgradeArea = false;
        StopCoroutine(_upgradeTriggerDelay);
        gameEventHandler.PlayerUpgradeArea(false);
    }
    
    IEnumerator UpgradeAreaDelay(bool inOut)
    {
        _upgradeArea = inOut;
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.7f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            ballController.GoUpgrade();
            gameEventHandler.PlayerUpgradeArea(true);
        }));
    }
    
    IEnumerator MergeAreaDelay(bool inOut)
    {
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            filledImage.fillAmount = 0;
            ballController.GoMerge();
        }));
    }
}
