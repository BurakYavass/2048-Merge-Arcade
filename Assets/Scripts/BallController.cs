using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] List<int> _SaveBall = new List<int>();


    [SerializeField]
    GameObject _CreatBall;  
    
    [SerializeField]
    GameObject _FirstObje;
   
    Vector3 _FirstObjePos;
    [SerializeField]
    Vector3 _Distance;
    [SerializeField]
    GameObject _Root;
    [SerializeField]
    List<GameObject> _Balls=new List<GameObject>();
    [SerializeField]
    RuntimeAnimatorController _BallSoft;
   
    [SerializeField]
    RuntimeAnimatorController _MachineVib;
    RuntimeAnimatorController _None;
    [SerializeField]
    bool _Delay;
    [SerializeField]
   float _FollowSpeed ;
    bool _GoMerge;
    bool _GoUpgrade;
    float _TempSpeed;
    [SerializeField] private PlayerController _Player;
    GameObject _MergeBallPos;
    GameObject _UpgradeBallPos;
    GameObject _MergeController;
    float _TempTimeMerge;
    float _DelayMerge;

   
    void Start()
    {
        _FirstObjePos = _FirstObje.transform.position;
        //_Player = GameObject.FindGameObjectWithTag("Player");
        _MergeBallPos = GameObject.FindGameObjectWithTag("MergeBallPos");
        _UpgradeBallPos = GameObject.FindGameObjectWithTag("UpgradeBallPos");
        _MergeController = GameObject.FindGameObjectWithTag("MergeController");
    

        _Distance.z = _Balls[0].transform.position.z - _Root.transform.position.z;
        _Distance.y = _Balls[0].transform.position.y - _Root.transform.position.y;
        _Distance.x = _Balls[0].transform.position.x - _Root.transform.position.x;
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
            //_TempSpeed = _FollowSpeed + (10  * _Player.GetComponent<PlayerControl>().GetJoystickValue());
            GetComponent<Animator>().runtimeAnimatorController = _BallSoft;
        }
        else
        {
            GetComponent<Animator>().runtimeAnimatorController = _None;
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
            //_Balls[0].GetComponent<Rigidbody>().position = Vector3.Lerp(_Balls[0].GetComponent<Rigidbody>().position, _Root.GetComponent<Rigidbody>().position, .2f);
            _Balls[0].GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Lerp(_Balls[0].GetComponent<Rigidbody>().position.x, _Root.transform.position.x, .5f),
            Mathf.Lerp(_Balls[0].GetComponent<Rigidbody>().position.y, _Root.transform.position.y, .2f),
            Mathf.Lerp(_Balls[0].GetComponent<Rigidbody>().position.z, _Root.transform.position.z, .2f)

            );
            //_Balls[0].transform.localScale = Vector3.Lerp(_Balls[0].transform.localScale, _Root.transform.lossyScale, _FollowSpeed * Time.deltaTime);
            _Balls[0].GetComponent<Rigidbody>().rotation = Quaternion.Lerp(_Balls[0].GetComponent<Rigidbody>().rotation, _Root.transform.rotation, .2f);
            for (int i = 1; i < _Balls.Count; i++)
            {
                //  _Balls[i].GetComponent<Rigidbody>().position = Vector3.Lerp(_Balls[i].GetComponent<Rigidbody>().position, _Balls[i-1].GetComponent<Rigidbody>().position  - (  _Balls[i - 1].GetComponent<Rigidbody>().transform.up * (_Balls[i].transform.localScale.z * 1.5f)), ( _TempSpeed * Time.deltaTime));

                _Balls[i].GetComponent<Rigidbody>().position = new Vector3(
                    Mathf.Lerp(_Balls[i].GetComponent<Rigidbody>().position.x, _Balls[i - 1].GetComponent<Rigidbody>().position.x, (_TempSpeed * Time.deltaTime)),
                    Mathf.Lerp(_Balls[i].GetComponent<Rigidbody>().position.y, _Balls[i - 1].GetComponent<Rigidbody>().position.y, (_TempSpeed * Time.deltaTime) * ((_Balls.Count - i) * (.05f))),
                    Mathf.Lerp(_Balls[i].GetComponent<Rigidbody>().position.z, _Balls[i - 1].GetComponent<Rigidbody>().position.z, (_TempSpeed * Time.deltaTime))

                    );
                _Balls[i].GetComponent<Rigidbody>().position -= (_Balls[i - 1].GetComponent<Rigidbody>().transform.up * (_Balls[i].transform.localScale.z * 1.5f));


                _Balls[i].transform.localScale = Vector3.Lerp(_Balls[i].transform.localScale, _Balls[i - 1].transform.localScale, (_TempSpeed * Time.deltaTime));
                _Balls[i].GetComponent<Rigidbody>().rotation = Quaternion.Lerp(_Balls[i].GetComponent<Rigidbody>().rotation, _Balls[i - 1].GetComponent<Rigidbody>().rotation, (_TempSpeed * Time.deltaTime));

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
