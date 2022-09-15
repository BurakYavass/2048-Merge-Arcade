using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    float _Speed;
    [SerializeField]
    float _SpeedTemp;

    Rigidbody _Rb;
    Animator _Animator;

    [SerializeField]
    RuntimeAnimatorController _Run;
    [SerializeField]
    RuntimeAnimatorController _Idle;
    [SerializeField]
    RuntimeAnimatorController _MergeBallMove;
    [SerializeField]
    RuntimeAnimatorController _Hit;

    RuntimeAnimatorController _None;
    GameObject _BallController;
    GameObject _Cameras;
    bool _Stop=true;
    bool _Hitting;
    float _JoystickValue;
    bool _OnMergeMachine;
    bool _OnUpgrade;
     void Start()
    {
        _BallController = GameObject.FindGameObjectWithTag("BallController");
        _Cameras = GameObject.FindGameObjectWithTag("Cameras");
           _Rb = GetComponent<Rigidbody>();
        _Animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _SpeedTemp = _Speed;
    }


    private void Update()
    {
        
    }
    void FixedUpdate()
    {
      
        Vector3 direction = new Vector3(GetComponent<InputController>().GetJoystick().x, 0f, GetComponent<InputController>().GetJoystick().y) * _SpeedTemp * 4;

         direction = Vector3.ClampMagnitude(direction, _SpeedTemp);
        if (direction != Vector3.zero)
        {
            Vector3 temp = transform.position + direction * (Time.deltaTime);
            UnityEngine.AI.NavMeshHit hit;
            bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(temp, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            if (isvalid)
            {

                if ((transform.position - hit.position).magnitude >= 0.1f)
                {
                    _JoystickValue = (transform.position - hit.position).magnitude;


                    _Rb.MovePosition(temp);
                    _Rb.MoveRotation(Quaternion.LookRotation(-direction));



                    
                        _Animator.runtimeAnimatorController = _Run;
                    _Stop = false;
                }
                 
            }
            else
            {
                _Stop = true;
                _Animator.runtimeAnimatorController = _Idle;
           
            }

        }
        else
        {
            _Stop = true;
            if (_Hitting)
            {
                _Animator.runtimeAnimatorController = _Hit;
            }
            else
            {
                _Animator.runtimeAnimatorController = _Idle;

            }
         
        }

    }

    public bool GetStop()
    {
        return _Stop;
    }
    public bool GetHitting()
    {
        return _Hitting;
    }
    public float GetJoystickValue()
    {
        return _JoystickValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MergeMachine"))
        {
            if (!_OnMergeMachine)
            {
                _OnMergeMachine = true;
                _BallController.GetComponent<BallController>().GoMerge();
                // other.gameObject.transform.parent.GetComponent<Animator>().runtimeAnimatorController = _None;
                // other.gameObject.transform.parent.GetComponent<Animator>().runtimeAnimatorController = _MergeBallMove;

            }
        }

        if (other.CompareTag("UpgradeTrigger"))
        {
            if (!_OnUpgrade)
            {
                _OnUpgrade = true;
                _BallController.GetComponent<BallController>().GoUpgrade();
           
            }
        }
        
        if (other.CompareTag("HitArea"))
        {
             _SpeedTemp = 5;
            _Hitting = true;
           
        }

        if (other.CompareTag("EmptyBall"))
        {
            //  _BallController.GetComponent<BallController>().SetNewBall(other.gameObject);

            other.gameObject.GetComponent<Ball>().SetGoTarget(_BallController.GetComponent<BallController>().LastObje());
            other.tag = "StackBall";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MergeArea"))
        {
            _Cameras.GetComponent<CameraFollow>().SetMerge();
        }

        if (other.CompareTag("PortalArea"))
        {
            _Cameras.GetComponent<CameraFollow>().SetPortal();

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MergeMachine")  )
        {
            _OnMergeMachine = false;
        }

        if (other.CompareTag("HitArea"))
        {
            //_Rb.AddRelativeForce(-GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
            _SpeedTemp = _Speed;
            _Hitting = false;

        }
        if (other.CompareTag("MergeArea"))
        {
            _Cameras.GetComponent<CameraFollow>().SetOriginal();
        }
        if (other.CompareTag("PortalArea"))
        {
            _Cameras.GetComponent<CameraFollow>().SetOriginal();

        }
    }
}
