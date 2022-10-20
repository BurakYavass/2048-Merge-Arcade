using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WallValue : MonoBehaviour
{
    public int unlockRequire;
    public int unlockRequireCurrent;
    [SerializeField] private float playerWaitTime;
    [SerializeField] private float wallUnlockDelay;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Image wallValueTexture;
    [SerializeField] private TravelManager openGameObject;
    [SerializeField] private GameObject mainObject;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private Image filledImage;
    [SerializeField] private Image triggerFilled;
    private PlayerCollisionHandler _playerFollowerList;
    private PlayerBallCounter _playerBallCounter;
    private BallController _ballController;
    

    public int _totalValue;
    private bool _playerWallArea = false;
    private bool _once = false;
    private int _dicreaseValue;
    private int j;
    private Vector3 _scale;

    private void Start()
    {
        _ballController = GameObject.FindWithTag("BallController").GetComponent<BallController>();
        var image= images.Find((texture => texture.name == unlockRequire.ToString()));
        wallValueTexture.sprite = image;
        filledImage.sprite = image;
        unlockRequireCurrent = unlockRequire;
        _scale = transform.localScale;
    }

    public void UnlockCalculate(bool unlock)
    {
        _ballController.GoUnlock(transform,true);
    }

    private void Update()
    {
        if (_ballController.balls.Count == 0)
        {
            _ballController.GoUnlock(null,false);
        }
        if (unlockRequireCurrent == 0 && GameObject.FindGameObjectsWithTag("UnlockBall").Length<1)
        {
            _totalValue = Mathf.Clamp(_totalValue - unlockRequire, 0, 4096);
            TotalValue();
        }
        
    }

    void TotalValue()
    {
        if (_totalValue == 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            if (!_once)
            {
                _once = true;
                StartCoroutine(WallDelay());
            }
            return;
        }
        
        var sayi = 4096;
        while (_totalValue > 0)
        {

            if (sayi > _totalValue)
            {
                sayi /= 2;
                continue;
            }

            if (j > 2)
            {
                j = 0;
            }
            else
            {
                var follower = _playerFollowerList.playerFollowPoints[j];
                var last = follower.ReturnLast();
                var lastPosition = last.transform.position;
                GameObject go = Instantiate(ballPrefab, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3),
                    Quaternion.identity,
                    _ballController.transform);
                go.tag = "StackBall";
                var ball = go.GetComponent<Ball>();
                ball.SetValue(sayi);
                follower.SaveBall(ball.gameObject);
                ball.SetGoTarget(last.transform);
                ball.StartDelay();
                _totalValue -= sayi;
                _ballController.SetNewBall(go);
                j++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_playerWallArea)
            {
                _playerWallArea = true;
                _playerFollowerList = other.GetComponent<PlayerCollisionHandler>();
                _playerBallCounter = other.GetComponent<PlayerBallCounter>();
                triggerFilled.DOKill();
                triggerFilled.DOFillAmount(1, playerWaitTime).OnComplete((() =>
                {
                    UnlockCalculate(true);
                    triggerFilled.fillAmount = 0;
                }));
            }
        }
        
        if (other.CompareTag("UnlockBall"))
        {
            transform.DOKill();
            Destroy(other.gameObject);
            
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).SetEase(Ease.OutBounce)
                                                .OnComplete((() => transform.localScale = _scale));
            int tempvalue = other.GetComponent<Ball>().GetValue();
            _totalValue += tempvalue;
            unlockRequireCurrent = Mathf.Clamp(unlockRequireCurrent - tempvalue,0,unlockRequireCurrent);
            var bolum = (float)tempvalue / (float)unlockRequire;
            filledImage.fillAmount += bolum /1.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerFilled.DOKill();
            triggerFilled.fillAmount = 0;
            UnlockCalculate(false);
            _playerWallArea = false;
        }
    }

    IEnumerator WallDelay()
    {
        yield return new WaitForSeconds(wallUnlockDelay);
        transform.parent.transform.DOMoveY(-10f, 2f).SetEase(Ease.OutBounce).
                            OnComplete((() =>
                            {
                                gameObject.GetComponent<MeshRenderer>().enabled = false;
                                
                                openGameObject.active = true;
                                mainObject.GetComponent<CloseDelay>().CloseObje();
                            }));
    }
}
