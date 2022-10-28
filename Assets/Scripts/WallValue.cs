using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WallValue : MonoBehaviour
{
    public int unlockRequire;
    public int unlockRequireCurrent;
    [SerializeField] private float playerWaitTime;
    [SerializeField] private float wallUnlockDelay;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Image wallValueTexture;
    [SerializeField] private TravelManager openGameObject;
    [SerializeField] private GameObject mainObject;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private Image filledImage;
    [SerializeField] private Image triggerFilled;
    private PlayerCollisionHandler _playerFollowerList;
    private BallController _ballController;
    

    public int _totalValue;
    private bool _playerWallArea = false;
    private bool _once = false;
    private int _dicreaseValue;
    private int j;
    private Vector3 _scale;
    private bool _warningAnim;
    [SerializeField] private Image notEnoughImage;

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
        _ballController.GoUnlock(transform,unlockRequire);
        var value = unlockRequire;

        var sort = _ballController.balls.OrderBy(y => y.GetValue());
        var ball = sort.FirstOrDefault((x => x.GetValue() >= value));
        if (ball)
        {
            ball.GetComponent<Ball>().SetGoUnlock(transform);
            _ballController.balls.Remove(ball);
        }
        else
        {
            if (!_warningAnim)
            {
                _warningAnim = true;
                notEnoughImage.enabled = true;
                notEnoughImage.DOColor(Color.white, .2f)
                    .OnComplete((() => notEnoughImage.DOColor(Color.red, 0.2f)));
            }
        }
    }

    private void Update()
    {
        if (unlockRequireCurrent == 0 && GameObject.FindGameObjectsWithTag("UnlockBall").Length<1)
        {
            _totalValue = Mathf.Clamp(_totalValue - unlockRequire, 0, 4096);
            gameObject.GetComponent<Collider>().enabled = false;
            TotalValue();
            
        }
    }

    void TotalValue()
    {
        if (_totalValue == 0)
        {
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
                var go = Instantiate(ballPrefab, new Vector3(lastPosition.x, lastPosition.y, lastPosition.z - 3),
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

        if (_totalValue == 0)
        {
            if (!_once)
            {
                _once = true;
                StartCoroutine(WallDelay());
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
                triggerFilled.DOKill();
                triggerFilled.DOFillAmount(1, playerWaitTime).OnComplete((() =>
                {
                    UnlockCalculate(true);
                    triggerFilled.fillAmount = 0;
                }));
                notEnoughImage.DOKill();
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
            //UnlockCalculate(false);
            _playerWallArea = false;
            notEnoughImage.DOKill();
            notEnoughImage.enabled = false;
            _warningAnim = false;
        }
    }

    IEnumerator WallDelay()
    {
        yield return new WaitForSeconds(wallUnlockDelay);
        transform.parent.transform.DOMoveY(-10f, 2f).SetEase(Ease.OutBounce)
                            .OnComplete((() =>
                            {
                                gameObject.GetComponent<MeshRenderer>().enabled = false;
                                openGameObject.active = true;
                                mainObject.GetComponent<CloseDelay>().CloseObje();
                            }));
    }
}
