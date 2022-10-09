using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameEventHandler gameEventHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimationHandler animationHandler;
    [SerializeField] private WeaponsHit weaponsHit;
    [SerializeField] private BallController ballController;
    [SerializeField] private ChestController chestController;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Image filledImage;
    [SerializeField] private GameObject progressBar;
    public List<FollowerList> playerFollowPoints;

    private PlayerBallCounter _playerBallCounter;
    private Vector3 _lastposition;

    private Coroutine _triggerDelay;
    private Coroutine _mergeTriggerDelay;
    
    private bool _onMergeMachine = false;
    private bool _hit = false;
    private bool _upgradeArea = false;
    private bool _unlockArea = false;
    private int i = 0;

    private void Start()
    {
        _playerBallCounter = playerController.GetComponent<PlayerBallCounter>();
    }

    private void Update()
    {
        if (chestController != null)
        {
            if (chestController._ChestHealthValueCurrent < 1)
            {
                animationHandler.CurrentPlayerHit(false);
                chestController = null;
            }
        }

        if (progressBar.activeInHierarchy)
        {
            progressBar.transform.LookAt(Camera.main.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseRight"))
        {
            gameEventHandler.PlayerRightArea(true);
            playerController.CameraChanger(1);
        }
        else if (other.CompareTag("BaseLeft"))
        {
            gameEventHandler.PlayerLeftArea(true);
            playerController.CameraChanger(2);
        }
        else
        {
            playerController.CameraChanger(0);
        }

        if (other.CompareTag("MergeMachine"))
        {
            if (!_onMergeMachine)
            {
                _onMergeMachine = true;
                if (ballController.balls.Count > 1)
                {
                    _mergeTriggerDelay = StartCoroutine(MergeDelay(true));
                    
                    //GameEventHandler.current.PlayerMergeArea(true);
                }
            }
        }
        
        if (other.CompareTag("UpgradeTrigger"))
        {
            if (!_upgradeArea)
            {
                //_playerBallCounter.BallCountCheck();
                _triggerDelay = StartCoroutine(UpgradeDelay(true));
            }
        }
        
        if (other.CompareTag("EmptyBall"))
        {
            if (i>2)
            {
                i = 0;
            }
            if (i < 3)
            {
                other.tag = "StackBall";
                other.gameObject.GetComponent<Ball>().SetGoTarget(playerFollowPoints[i].ReturnLast().transform);
                playerFollowPoints[i].SaveBall(other.gameObject);
                i++;
                ballController.SetNewBall(other.gameObject);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            chestController= other.GetComponent<ChestController>();
            if (!_hit)
            {
                _hit = true;
                StartCoroutine(HitDelay());
            }
            animationHandler.CurrentPlayerHit(true);
        }
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyController= other.GetComponent<EnemyController>();
            if (!_hit)
            {
                _hit = true;
                StartCoroutine(HitDelay());
            }
            animationHandler.CurrentPlayerHit(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseRight"))
        {
            gameEventHandler.PlayerRightArea(false);
            playerController.CameraChanger(0);
        }
        else if (other.CompareTag("BaseLeft"))
        {
            gameEventHandler.PlayerLeftArea(false);
            playerController.CameraChanger(0);
        }
        else if (other.CompareTag("LevelWall"))
        {
            //gameEventHandler.PlayerLevelUnlockArea(false);
            _playerBallCounter.stackValue = 0;
            _unlockArea = false;
        }
        
        if (other.gameObject.CompareTag("Chest"))
        {
            chestController = null;
            animationHandler.CurrentPlayerHit(false);
            //GameEventHandler.current.PlayerHit(false);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyController = null;
            animationHandler.CurrentPlayerHit(false);
            //GameEventHandler.current.PlayerHit(false);
        }

        if (other.CompareTag("MergeMachine"))
        {
            if (_mergeTriggerDelay != null)
            {
                StopCoroutine(_mergeTriggerDelay);
            }
            filledImage.DOKill();
            filledImage.fillAmount = 0;
            progressBar.SetActive(false);
            _onMergeMachine = false;
            
            //GameEventHandler.current.BallMergeArea(false,null);
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            _playerBallCounter.stackValue = 0;
            filledImage.fillAmount = 0;
            if (_triggerDelay != null)
            {
                StopCoroutine(_triggerDelay);
            }
            filledImage.DOKill();
            progressBar.SetActive(false);
            _upgradeArea = false;
            //GameEventHandler.current.BallUpgradeArea(false,null);
            //gameEventHandler.PlayerUpgradeArea(false);
        }
        
    }

    public void ExitUpgrade()
    {
        _playerBallCounter.stackValue = 0;
        progressBar.SetActive(false);
        _upgradeArea = false;
        if (_triggerDelay != null)
        {
            StopCoroutine(_triggerDelay);
        }
        filledImage.fillAmount = 0;
        filledImage.DOKill();
        gameEventHandler.PlayerUpgradeArea(false);
        playerController.CameraChanger(0);
    }
    
    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(1f);
        _hit = false;
    }

    IEnumerator MergeDelay(bool inOut)
    {
        progressBar.SetActive(true);
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            filledImage.fillAmount = 0;
            progressBar.SetActive(false);
            ballController.GoMerge();
        }));
    }

    IEnumerator UpgradeDelay(bool inOut)
    {
        progressBar.SetActive(inOut);
        _upgradeArea = inOut;
        filledImage.fillAmount = 0;
        yield return new WaitForSeconds(0.7f);
        filledImage.DOFillAmount(1, 1f).OnComplete((() =>
        {
            ballController.GoUpgrade();
            playerController.CameraChanger(3);
            gameEventHandler.PlayerUpgradeArea(true);
        }));
    }

    private void WeaponHit()
    {
        if (chestController != null)
        {
            chestController.Hit(weaponsHit._damageValue);
            chestController.transform.parent.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce);
        }

        if (enemyController != null)
        {
            enemyController.GetHit(weaponsHit._damageValue);
            enemyController.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBounce);

        }
        //animationHandler.CurrentPlayerHit(false);
    }
    
}
