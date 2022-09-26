using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BallController : MonoBehaviour
{
    [SerializeField] public List<GameObject> balls=new List<GameObject>();
    [SerializeField] private GameObject creatBall;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private GameObject mergeBallPos;
    [SerializeField] private GameObject upgradeBallPos;
    [SerializeField] private Animator animator;
    private Vector3 _distance;
    private bool _goMerge;
    private bool _goUpgrade;
    private bool _waiter = false;
    private float _tempSpeed;
    private float _tempTimeMerge;
    private float _delayMerge;
    private int j = 0;

    void Start()
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
    
    private void FixedUpdate()
    {
        if (_goUpgrade)
        {
            _tempTimeMerge += Time.deltaTime;
            if (balls.Count > 1)
            {
                if (_tempTimeMerge >= .05f)
                {

                    balls[balls.Count - 1].GetComponent<Ball>().SetGoUpgrade(upgradeBallPos, _delayMerge);
                    _delayMerge += .5f;
                    balls.RemoveAt(balls.Count - 1);
                    _tempTimeMerge = 0;
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
                        StartCoroutine(DelayMerge());
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
    
    IEnumerator DelayMerge()
    {
        yield return new WaitForSeconds(.2f);
        _waiter = false;
    }

}
