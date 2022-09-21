using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] List<int> _SaveBall = new List<int>();
    [SerializeField] public List<GameObject> _Balls=new List<GameObject>();
    [SerializeField] private GameObject creatBall;
    [SerializeField] private GameObject playerRootPoint;
    [SerializeField] private PlayerController _Player;
    [SerializeField] private GameObject _MergeBallPos;
    [SerializeField] private GameObject _UpgradeBallPos;
    [SerializeField] private Animator animator;
    [SerializeField] float followSpeed ;
    private Vector3 _distance;
    bool _GoMerge;
    bool _GoUpgrade;
    float _TempSpeed;
    float _TempTimeMerge;
    float _DelayMerge;

    public float xStackSpeed;
    public float yStackSpeed;
    public float zStackSpeed;

    void Start()
    {
        _distance.z = _Balls[0].transform.position.z - playerRootPoint.transform.position.z;
        _distance.y = _Balls[0].transform.position.y - playerRootPoint.transform.position.y;
        _distance.x = _Balls[0].transform.position.x - playerRootPoint.transform.position.x;
        _TempSpeed = followSpeed;

        if (PlayerPrefs.GetInt("BallSaveCount") > 1)
        {
            for (int i = 0; i < PlayerPrefs.GetInt("BallSaveCount"); i++)
            {
                GameObject go = Instantiate(creatBall, _Balls[_Balls.Count-1].transform.position, Quaternion.Euler(0, 180, 0),gameObject.transform);
                go.GetComponent<Ball>().SetValue(PlayerPrefs.GetInt("BallSave" + (i + 1)));
                SetNewBall(go);
                go.tag = "StackBall";
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        PlayerPrefs.DeleteKey("BallSaveCount");
    }

    

    private void FixedUpdate()
    {
        if (_Player.walking)
        {
            _TempSpeed = followSpeed + (10  * _Player.GetJoystickValue());
            animator.SetBool("Scale", true);
        }
        else
        {
            animator.SetBool("Scale" , false);
            _TempSpeed = followSpeed;
        }
        
        if (!_GoMerge)
        {
            var firstBall = _Balls[0].GetComponent<Rigidbody>();
            var RootRb = playerRootPoint.GetComponent<Rigidbody>();
            firstBall.position = new Vector3(
                Mathf.Lerp(firstBall.position.x, RootRb.position.x, 1f),
                Mathf.Lerp(firstBall.position.y, RootRb.position.y, .2f),
                Mathf.Lerp(firstBall.position.z, RootRb.position.z, .5f)
            );
            firstBall.rotation = Quaternion.Lerp(firstBall.rotation, RootRb.rotation, .2f);

            for (int i = 1; i < _Balls.Count; i++)
            {
                var forwardBall = _Balls[i - 1].GetComponent<Rigidbody>();
                var currentBall = _Balls[i].GetComponent<Rigidbody>();
                
                currentBall.position = new Vector3(
                    //Mathf.Lerp(currentBall.position.x, forwardBall.position.x, (_TempSpeed * Time.deltaTime) / stackSpeed),
                    //Mathf.Lerp(currentBall.position.y, forwardBall.position.y, (_TempSpeed * Time.deltaTime) * ((_Balls.Count - i) * (.05f))),
                    //Mathf.Lerp(currentBall.position.z, forwardBall.position.z, (_TempSpeed * Time.deltaTime))
                    Mathf.Lerp(currentBall.position.x, forwardBall.position.x, (xStackSpeed * Time.deltaTime)),
                    Mathf.Lerp(currentBall.position.y, forwardBall.position.y, (yStackSpeed * Time.deltaTime)),
                    Mathf.Lerp(currentBall.position.z, forwardBall.position.z, (zStackSpeed * Time.deltaTime))
                );
                currentBall.position -= (forwardBall.transform.up * (currentBall.transform.localScale.z * 1.5f));
               
                currentBall.transform.localScale = Vector3.Lerp(currentBall.transform.localScale, forwardBall.transform.localScale, (_TempSpeed * Time.deltaTime));
                currentBall.rotation = Quaternion.Lerp(currentBall.rotation, forwardBall.rotation, (_TempSpeed * Time.deltaTime) / xStackSpeed);
            }
        }
    }
    
    public void GoMerge()
    {
        _GoMerge = true;
        _TempTimeMerge += Time.deltaTime;
        if (_Balls.Count> 1  )
        {
            if (_TempTimeMerge >= .05f)
            {
                _Balls[_Balls.Count-1].GetComponent<Ball>().SetGoMerge(_MergeBallPos, _DelayMerge);
                _DelayMerge += .5f;
                _Balls.RemoveAt(_Balls.Count-1);
                _TempTimeMerge = 0;
            }
        }
        else
        {
            // StartCoroutine(DelayMerge());
            _GoMerge = false;
            _DelayMerge = 0;
        }
    }

    public void GoUpgrade()
    {
        _TempTimeMerge += Time.deltaTime;
        if (_Balls.Count > 1)
        {
            if (_TempTimeMerge >= .05f)
            {

                _Balls[_Balls.Count - 1].GetComponent<Ball>().SetGoUpgrade(_UpgradeBallPos, _DelayMerge);
                _DelayMerge += .5f;
                _Balls.RemoveAt(_Balls.Count - 1);
                _TempTimeMerge = 0;
            }
        }
        else
        {
            // StartCoroutine(DelayMerge());
            _GoUpgrade = false;
            _DelayMerge = 0;
        }
    }
    public GameObject LastObje()
    {
        return _Balls[_Balls.Count - 1];
    }
    public void SetNewBall(GameObject ball)
    {
        _Balls.Add(ball) ;
    }
    

    // public void GoUpgrade()
    // {
    //     _GoUpgrade = true;
    // }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if (_Balls.Count>1)
            {
                for (int i = 1; i < _Balls.Count; i++)
                {
                    PlayerPrefs.SetInt("BallSave" + i, (int)(_Balls[i].GetComponent<Ball>().GetValue()));
                    PlayerPrefs.SetInt("BallSaveCount", _Balls.Count-1);
                    PlayerPrefs.Save();
                }
            }
        }
    }


    IEnumerator DelayMerge()
    {
        yield return new WaitForSeconds(5);
       
    }

}
