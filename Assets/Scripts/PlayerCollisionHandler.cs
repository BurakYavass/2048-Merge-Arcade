using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
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
    [SerializeField] public ChestController chestController;
    [SerializeField] public EnemyController enemyController;
    [SerializeField] private Image filledImage;
    public List<FollowerList> playerFollowPoints;

    private PlayerBallCounter _playerBallCounter;
    private Vector3 _lastposition;

    private Coroutine _triggerDelay;
    private Coroutine _mergeTriggerDelay;
    
    private bool _onMergeMachine = false;
    public bool _hit = false;
    private bool _upgradeArea = false;
    private bool _unlockArea = false;
    private int i = 0;
    [SerializeField] private Transform rayCastObject;
    

    private void OnEnable()
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
            else
            {
                animationHandler.CurrentPlayerHit(true);
            }
        }

        if (enemyController != null)
        {
            if (enemyController.enemyHealthValueCurrent < 1)
            {
                animationHandler.CurrentPlayerHit(false);
                enemyController = null;
            }
            else
            {
                animationHandler.CurrentPlayerHit(true);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enabled)
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
                        filledImage = other.GetComponent<TriggerArea>().filledImage;
                        _mergeTriggerDelay = StartCoroutine(MergeAreaDelay(true));

                        //GameEventHandler.current.PlayerMergeArea(true);
                    }
                }
            }
        
            if (other.CompareTag("UpgradeTrigger"))
            {
                if (!_upgradeArea)
                {
                    //_playerBallCounter.BallCountCheck();
                    filledImage = other.GetComponent<TriggerArea>().filledImage;
                    _triggerDelay = StartCoroutine(UpgradeAreaDelay(true));
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
    }

    private void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.CompareTag("Chest"))
        // {
        //     chestController= other.GetComponent<ChestController>();
        //     // RaycastHit hit;
        //     // var rayCastObje = rayCastObject.transform.position;
        //     // if (Physics.Raycast(rayCastObje,rayCastObject.transform.forward,out hit,10))
        //     // {
        //     //     if (hit.collider.CompareTag("Chest"))
        //     //     {
        //     //         animationHandler.CurrentPlayerHit(true);
        //     //     }
        //     // }
        //     animationHandler.CurrentPlayerHit(true);
        // }
        //
        // if (other.gameObject.CompareTag("Enemy"))
        // {
        //     enemyController= other.GetComponent<EnemyController>();
        //     // RaycastHit hit;
        //     // var rayCastObje = rayCastObject.transform.position;
        //     //
        //     // if (Physics.CapsuleCast(rayCastObje, rayCastObje+Vector3.up*2,0.7f,rayCastObject.transform.forward,out hit,10))
        //     // {
        //     //     if (hit.collider.CompareTag("Enemy"))
        //     //     {
        //     //         animationHandler.CurrentPlayerHit(true);
        //     //     }
        //     // }
        //     
        //     animationHandler.CurrentPlayerHit(true);
        //    
        // }
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
            //animationHandler.CurrentPlayerHit(false);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyController = null;
            //animationHandler.CurrentPlayerHit(false);
        }

        if (other.CompareTag("MergeMachine"))
        {
            if (filledImage != null)
                filledImage.fillAmount = 0;
            
            if (_mergeTriggerDelay != null)
            {
                StopCoroutine(_mergeTriggerDelay);
            }
            filledImage.DOKill();
            filledImage = null;
            _onMergeMachine = false;
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            if (filledImage != null)
                filledImage.fillAmount = 0;
            
            _playerBallCounter.stackValue = 0;
            if (_triggerDelay != null)
            {
                StopCoroutine(_triggerDelay);
            }
            filledImage.DOKill();
            filledImage = null;
            _upgradeArea = false;
        }
        
    }

    public void ExitUpgrade()
    {
        _playerBallCounter.stackValue = 0;
        _upgradeArea = false;
        if (_triggerDelay != null)
        {
            StopCoroutine(_triggerDelay);
        }
        gameEventHandler.PlayerUpgradeArea(false);
        playerController.CameraChanger(0);
    }
    
    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(.5f);
        _hit = false;
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

    IEnumerator UpgradeAreaDelay(bool inOut)
    {
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

    private void AnimationOver()
    {
        animationHandler.CurrentPlayerHit(false);
    }
    
}
