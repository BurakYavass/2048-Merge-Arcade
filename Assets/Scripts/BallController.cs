using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public List<GameObject> balls=new List<GameObject>();
    [SerializeField] private GameObject creatBall;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private GameObject mergeBallPos;
    [SerializeField] private GameObject upgradeBallPos;
    [SerializeField] private UpgradeArea _upgradeArea;
    private Transform _unlockWallPos;
    private Vector3 _distance;
    private bool _goMerge;
    private bool _goUpgrade;
    private bool _goUnlock;
    private bool _goFree;
    private bool _goTravelPoint;
    private bool _waiter = false;
    private float _tempSpeed;
    private float _tempTimeMerge;
    private float _delayMerge;
    private int j = 0;

    void Start()
    {
        ReloadGame();
    }

    private void ReloadGame()
    {
        if (PlayerPrefs.GetInt("BallSaveCount") > 0)
        {
            for (int i = 0; i < PlayerPrefs.GetInt("BallSaveCount"); i++)
            {
                if (j >2)
                {
                    j = 0;
                }
                if (j <= 2)
                {
                    var follower = _playerFollowerList.playerFollowPoints[j];
                    var last = follower.ReturnLast();
                    var lastPosition = last.transform.position;
                    GameObject go = Instantiate(creatBall, new Vector3(lastPosition.x,lastPosition.y,lastPosition.z-3), Quaternion.identity,gameObject.transform);
                    go.tag = "StackBall";
                    var ball = go.GetComponent<Ball>();
                    ball.SetValue(PlayerPrefs.GetInt("BallSave" + (i)));
                    follower.SaveBall(ball.gameObject);
                    ball.SetGoTarget(last.transform);
                    ball.StartDelay();
                    SetNewBall(go);
                    j++;
                }
            }
        }
        PlayerPrefs.DeleteKey("BallSaveCount");
    }
    
    private void Update()
    {
        balls.RemoveAll((obje => obje == null));
        if (_goUpgrade)
        {
            if (balls.Count > 0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    // if (!_waiter)
                    // {
                    //     _waiter = true;
                    //     StartCoroutine(Delay());
                    //     balls[i].GetComponent<Ball>().SetGoUpgrade(upgradeBallPos);
                    //     _delayMerge += .5f;
                    //     balls.RemoveAt(i);
                    //     _tempTimeMerge = 0;
                    // }

                    var ballValue = balls[i].GetComponent<Ball>()._BallValue;
                    switch (ballValue)
                    {
                        case 2:
                            _upgradeArea._totalValue += 2;
                            break;
                        case 4:
                            _upgradeArea._totalValue += 4;
                            break;
                        case 8:
                            _upgradeArea._totalValue += 8;
                            break;
                        case 16:
                            _upgradeArea._totalValue += 16;
                            break;
                        case 32:
                            _upgradeArea._totalValue += 32;
                            break;
                        case 64:
                            _upgradeArea._totalValue += 64;
                            break;
                        case 128:
                            _upgradeArea._totalValue += 128;
                            break;
                        case 256:
                            _upgradeArea._totalValue += 256;
                            break;
                        case 512:
                            _upgradeArea._totalValue += 512;
                            break;
                        case 1024:
                            _upgradeArea._totalValue += 1024;

                            break;
                        case 2048:
                            _upgradeArea._totalValue += 2048;

                            break;
                        case 4096:
                            _upgradeArea._totalValue += 4096;
                            break;
                    }
                }

                foreach (var ball in balls)
                {
                    Destroy(ball.gameObject);
                }
                balls.Clear();
            }
            else
            {
                _goUpgrade = false;
                _delayMerge = 0;
            }
        } 
        
        if (_goMerge)
        {
            _tempTimeMerge += Time.deltaTime;
            if (balls.Count> 0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    if (!_waiter)
                    {
                        _waiter = true;
                        StartCoroutine(Delay());
                        var first = balls.FirstOrDefault();
                        if (first) 
                            first.GetComponent<Ball>().SetGoMerge(mergeBallPos,_delayMerge);
                        _delayMerge += .5f;
                        balls.Remove(first);
                        _tempTimeMerge = 0;
                    }
                }
            }
            else
            {
                _goMerge = false;
                _delayMerge = 0;
            }
        }

        if (_goUnlock)
        {
            if (balls.Count > 0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    if (!_waiter)
                    {
                        _waiter = true;
                        StartCoroutine(Delay());
                            var first = balls.FirstOrDefault();
                            if (first) 
                                first.GetComponent<Ball>().SetGoUnlock(_unlockWallPos);
                            balls.Remove(first);
                    }
                }
            }
            else
            {
                _goUnlock = false;
            }
        }

        if (_goTravelPoint)
        {
            if (balls.Count > 0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    //_playerFollowerList.playerFollowPoints[i].clearList = true;
                    balls[i].GetComponent<Ball>().SetGoTravel();
                    balls[i].SetActive(false);
                    // balls.RemoveAt(i);
                    PlayerPrefs.SetInt("BallSave" + i, (int)(balls[i].GetComponent<Ball>().GetValue()));
                    PlayerPrefs.SetInt("BallSaveCount", balls.Count);
                    PlayerPrefs.Save();
                    Destroy(balls[i].gameObject);
                    balls.RemoveAll((obje => obje == null));
                }
            }
            else
            {
                StartCoroutine(Reload());
                _goTravelPoint = false;
            }
        }

        if (_goFree)
        {
            if (balls.Count > 0)
            {
                for (int i = 0; i <balls.Count; i++)
                {
                    balls[i].GetComponent<Ball>().SetGoFree();
                    balls.RemoveAt(i);
                }
            }
            else
            {
                _goFree = false;
            }
        }
    }

    public void GoUnlock(Transform target , bool go)
    {
        _unlockWallPos = target;
        _goUnlock = go;
    }
    public GameObject LastObje()
    {
        return balls[balls.Count - 1];
    }
    public void SetNewBall(GameObject ball)
    {
        balls.Add(ball);
    }
    public void GoMerge()
    {
        _goMerge = true;
    }

    public void GoUpgrade()
    {
        _goUpgrade = true;
    }

    public void GoTravelPoint()
    {
        _goTravelPoint = true;
    }

    public void GoFree()
    {
        _goFree = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if (balls.Count>0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    PlayerPrefs.SetInt("BallSave" + i, (int)(balls[i].GetComponent<Ball>().GetValue()));
                    PlayerPrefs.SetInt("BallSaveCount", balls.Count);
                    PlayerPrefs.Save();
                }
            }
        }
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.1f);
        _waiter = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        ReloadGame();
    }

}
