using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    GameObject[] _Colors;
    [SerializeField]
    float _BallValue;

    [SerializeField]
    TextMeshPro _BallText;
    [SerializeField]
    RuntimeAnimatorController _Merge;
    float _Distance;
    public GameObject _TempObje;
  public  bool _Go;
  public  bool _GoMerge;
  public  bool _GoUpgrade;
    bool _OnStackpos;
    GameObject _BallController;
 
    Material  _Material;
    float _DelayMerge;
    void Start()
    {
      
        _Material  = GetComponent<MeshRenderer>().material;
        _BallController = GameObject.FindGameObjectWithTag("BallController");
        BallTextChange();
        StartCoroutine(DelayKinematic());
       
    }
  void Update()
    {
        if (_Go)
        {
            transform.position = Vector3.Lerp(transform.position,_TempObje.transform.position,.1f*(_Distance- Vector3.Distance(transform.position, _TempObje.transform.position)));
            if (Vector3.Distance(transform.position, _TempObje.transform.position)<_Distance*.2f)
            {
          
                _Go = false;
                _OnStackpos = true;
                _BallController.GetComponent<BallController>().SetNewBall(gameObject);
            }
        }
        else if (_GoMerge)
        {
            transform.position = Vector3.Lerp(transform.position, _TempObje.transform.position, .03f   );
            transform.localScale = Vector3.Lerp(transform.localScale, _TempObje.transform.localScale, .03f   );
            if (Vector3.Distance(transform.position, _TempObje.transform.position) < _Distance * .05f)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Collider>().enabled = false;
                _GoMerge = false;
                StartCoroutine(DelayMergeTime());
             }
        }
        else if (_GoUpgrade)
        {
            Vector3 rndm = new Vector3(Random.Range(-2, 2), Random.Range(2, 4), 0);
            if (Vector3.Distance(transform.position, _TempObje.transform.position) < _Distance * .05f)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Collider>().enabled = false;
                _GoUpgrade = false;
               


            }
            else if (Vector3.Distance(transform.position, _TempObje.transform.position+ rndm) < _Distance * .8f)
            {
                transform.position = Vector3.Lerp(transform.position, _TempObje.transform.position+ rndm, .01f);
                transform.localScale = Vector3.Lerp(transform.localScale, _TempObje.transform.localScale, .01f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _TempObje.transform.position, .03f);
                transform.localScale = Vector3.Lerp(transform.localScale, _TempObje.transform.localScale, .03f);
            }
        }
        
    }
    public void SetValue(float ballvalue)
    {
        _BallValue = ballvalue;
        BallTextChange();
    }
    void BallTextChange()
    {
        _BallText.text = _BallValue.ToString();

        for (int i = 0; i < _Colors.Length; i++)
        {
            _Colors[i].SetActive(false);
        }
        if (_BallValue==2)
        {
            _Colors[0].SetActive(true);
        }
        else if (_BallValue == 4)
        {
            _Colors[1].SetActive(true);
        }
        else if (_BallValue == 8)
        {
            _Colors[2].SetActive(true);
        }
        else if (_BallValue == 16)
        {
            _Colors[3].SetActive(true);
        }
        else if (_BallValue == 32)
        {
            _Colors[4].SetActive(true);
        }
        else if (_BallValue == 64)
        {
            _Colors[5].SetActive(true);
        }
        else if (_BallValue == 128)
        {
            _Colors[6].SetActive(true);
        }
        else if (_BallValue == 256)
        {
            _Colors[7].SetActive(true);
        }
        else if (_BallValue == 512)
        {
            _Colors[8].SetActive(true);
        }
        else if (_BallValue == 1024)
        {
            _Colors[9].SetActive(true);
        }
        else if (_BallValue == 2048)
        {
            _Colors[10].SetActive(true);
        }
        else if (_BallValue == 4096)
        {
            _Colors[11].SetActive(true);
        } 
        
       
    }
    public void SetGoUpgrade(GameObject target, float delay)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        _TempObje = target;
        _Distance = Vector3.Distance(transform.position, _TempObje.transform.position);
        GetComponent<Collider>().isTrigger = true;
        _GoUpgrade = true;
    }
    public void SetGoTarget(GameObject target)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        _TempObje = target;
        _Distance = Vector3.Distance(transform.position,_TempObje.transform.position);
        GetComponent<Collider>().isTrigger = true;
        _Go = true;
    }
    public void SetGoMerge(GameObject target,float delay)
    {
        _DelayMerge = delay;
        gameObject.transform.parent = target.transform.parent;
        GetComponent<Rigidbody>().isKinematic = true;
        _TempObje = target;
        _Distance = Vector3.Distance(transform.position, _TempObje.transform.position);
        GetComponent<Collider>().isTrigger = true;
        _GoMerge = true;
    }
    public bool GetStackPos()
    {
        return _OnStackpos;
    }
    public float GetValue()
    {
        return _BallValue;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name=="BallPool")
        {
            GetComponent<Rigidbody>().isKinematic = false;
           
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BallPool")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }

        IEnumerator DelayKinematic()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().isKinematic = true;
    }
    IEnumerator DelayMergeTime( )
    {
        GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(_DelayMerge);
        GetComponent<Animator>().runtimeAnimatorController = _Merge;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;

    }
}
