using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public List<GameObject> balls=new List<GameObject>();
    [SerializeField] private GameObject creatBall;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private GameObject mergeBallPos;
    [SerializeField] private GameObject upgradeBallPos;
    private Transform _unlockWallPos;
    private Vector3 _distance;
    private bool _goMerge;
    private bool _goUpgrade;
    private bool _goUnlock;
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

    public void ReloadGame()
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
                    if (!_waiter)
                    {
                        _waiter = true;
                        StartCoroutine(Delay());
                        balls[i].GetComponent<Ball>().SetGoUpgrade(upgradeBallPos);
                        _delayMerge += .5f;
                        balls.RemoveAt(i);
                        _tempTimeMerge = 0;
                    }
                }
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
                        balls[i].GetComponent<Ball>().SetGoMerge(mergeBallPos,_delayMerge);
                        _delayMerge += .5f;
                        balls.RemoveAt(i);
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
                        balls[i].GetComponent<Ball>().SetGoUnlock(_unlockWallPos);
                        balls.RemoveAt(i);
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
        yield return new WaitForSeconds(.2f);
        _waiter = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        ReloadGame();
    }

}
