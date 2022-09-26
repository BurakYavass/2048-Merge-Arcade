using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BallController : MonoBehaviour
{

    [SerializeField] List<int> saveBall = new List<int>();
    [SerializeField] public List<GameObject> balls=new List<GameObject>();
    [SerializeField] private GameObject creatBall;
    //[SerializeField] private GameObject playerRootPoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerCollisionHandler _playerFollowerList;
    [SerializeField] private GameObject mergeBallPos;
    [SerializeField] private GameObject upgradeBallPos;
    [SerializeField] private Animator animator;
    [SerializeField] float followSpeed ;
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
                    //ball.ballRb.isKinematic = true;
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
                // StartCoroutine(DelayMerge());
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
                //StartCoroutine(DelayMerge());
                _goMerge = false;
                _delayMerge = 0;
            }
        }
        else
        {
            //     var firstBall = balls[0].GetComponent<Rigidbody>();
        //     var RootRb = playerController.GetComponent<Rigidbody>();
        //     firstBall.position = new Vector3(
        //         Mathf.Lerp(firstBall.position.x, RootRb.position.x, 1f),
        //         Mathf.Lerp(firstBall.position.y, RootRb.position.y, .2f),
        //         Mathf.Lerp(firstBall.position.z, RootRb.position.z, .5f)
        //     );
        //     firstBall.rotation = Quaternion.Lerp(firstBall.rotation, RootRb.rotation, .2f);
        //
        //     for (int i = 1; i < balls.Count; i++)
        //     {
        //         var forwardBall = balls[i - 1].GetComponent<Rigidbody>();
        //         var currentBall = balls[i].GetComponent<Rigidbody>();
        //         
        //         currentBall.position = new Vector3(
        //             Mathf.Lerp(currentBall.position.x, forwardBall.position.x, (xStackSpeed * Time.deltaTime)),
        //             Mathf.Lerp(currentBall.position.y, forwardBall.position.y, (yStackSpeed * Time.deltaTime)),
        //             Mathf.Lerp(currentBall.position.z, forwardBall.position.z, (zStackSpeed * Time.deltaTime))
        //         );
        //         currentBall.position -= (forwardBall.transform.up * (currentBall.transform.localScale.z * 1.5f));
        //        
        //         currentBall.transform.localScale = Vector3.Lerp(currentBall.transform.localScale, forwardBall.transform.localScale, (_tempSpeed * Time.deltaTime));
        //         currentBall.rotation = Quaternion.Lerp(currentBall.rotation, forwardBall.rotation, (_tempSpeed * Time.deltaTime) / xStackSpeed);
        //     }
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
