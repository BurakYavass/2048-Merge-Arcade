using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] List<int> _SaveBall = new List<int>();


    [SerializeField] GameObject _CreatBall;  
    
    //[SerializeField] GameObject _FirstObje;
   
    //Vector3 _FirstObjePos;
    private Vector3 _distance;
    [SerializeField] GameObject _Root;
    [SerializeField] List<GameObject> _Balls=new List<GameObject>();
    [SerializeField] RuntimeAnimatorController _BallSoft;
    [SerializeField] private Animation _animation;
   
    [SerializeField] RuntimeAnimatorController _MachineVib;
    RuntimeAnimatorController _None;
    [SerializeField] float _FollowSpeed ;
    bool _GoMerge;
    bool _GoUpgrade;
    float _TempSpeed;
    [SerializeField] private PlayerController _Player;
    GameObject _MergeBallPos;
    GameObject _UpgradeBallPos;
    //GameObject _MergeController;
    float _TempTimeMerge;
    float _DelayMerge;

    public float xStackSpeed;

    void Start()
    {
        //_FirstObjePos = _FirstObje.transform.position;
        //_Player = GameObject.FindGameObjectWithTag("Player");
        _MergeBallPos = GameObject.FindGameObjectWithTag("MergeBallPos");
        _UpgradeBallPos = GameObject.FindGameObjectWithTag("UpgradeBallPos");
        //_MergeController = GameObject.FindGameObjectWithTag("MergeController");
    

        _distance.z = _Balls[0].transform.position.z - _Root.transform.position.z;
        _distance.y = _Balls[0].transform.position.y - _Root.transform.position.y;
        _distance.x = _Balls[0].transform.position.x - _Root.transform.position.x;
        _TempSpeed = _FollowSpeed;


        if (PlayerPrefs.GetInt("BallSaveCount") > 1)
        {
            for (int i = 0; i < PlayerPrefs.GetInt("BallSaveCount"); i++)
            {
                GameObject go = Instantiate(_CreatBall, _Balls[_Balls.Count-1].transform.position, Quaternion.Euler(0, 180, 0),gameObject.transform);
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
            _TempSpeed = _FollowSpeed + (10  * _Player.GetJoystickValue());
            _animation.Play("BallScale");
            //GetComponent<Animator>().runtimeAnimatorController = _BallSoft;
        }
        else
        {
            _animation.Stop("BallScale");
            //GetComponent<Animator>().runtimeAnimatorController = _None;
            _TempSpeed = _FollowSpeed;
        }
        if (_GoMerge)
        {
            _MergeBallPos.transform.parent.GetComponent<Animator>().runtimeAnimatorController = _MachineVib;
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
        else if (_GoUpgrade)
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
        else
        {
            var firstBall = _Balls[0].GetComponent<Rigidbody>();
            var RootRb = _Root.GetComponent<Rigidbody>();
            firstBall.position = new Vector3(
             Mathf.Lerp(firstBall.position.x, RootRb.position.x, 1f),
             Mathf.Lerp(firstBall.position.y, RootRb.position.y, .2f),
             Mathf.Lerp(firstBall.position.z, RootRb.position.z, .5f)
             );
             firstBall.rotation = Quaternion.Lerp(firstBall.rotation, RootRb.rotation, .2f);

             for (int i = 1; i < _Balls.Count; i++)
            {
                var downBall = _Balls[i - 1].GetComponent<Rigidbody>();
                var currentBall = _Balls[i].GetComponent<Rigidbody>();
                
                currentBall.position = new Vector3(
                    Mathf.Lerp(currentBall.position.x, downBall.position.x, (_TempSpeed / xStackSpeed * Time.deltaTime)),
                    Mathf.Lerp(currentBall.position.y, downBall.position.y, (_TempSpeed * Time.deltaTime) * ((_Balls.Count - i) * (.05f))),
                    Mathf.Lerp(currentBall.position.z, downBall.position.z, (_TempSpeed * Time.deltaTime))
                );
               currentBall.position -= (downBall.transform.up * (currentBall.transform.localScale.z * 1.5f));
               
               currentBall.transform.localScale = Vector3.Lerp(currentBall.transform.localScale, downBall.transform.localScale, (_TempSpeed * Time.deltaTime));
               currentBall.rotation = Quaternion.Lerp(currentBall.rotation, downBall.rotation, (_TempSpeed * Time.deltaTime));
            }
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
    public void GoMerge()
    {
        _GoMerge = true;
      
    }

    public void GoUpgrade()
    {
        _GoUpgrade = true;

    }

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
