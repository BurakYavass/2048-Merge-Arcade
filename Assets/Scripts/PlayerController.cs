using System;
using Cinemachine;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private Rigidbody _rb;

    private float _joystickValue;

    public bool walking = false;
    private bool once = false;
    
    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }

    void FixedUpdate()
    {
        if (joystick.isActiveAndEnabled)
        {
            Movement();
        }
        else
        {
            walking = false;
            handle.anchoredPosition = Vector2.zero;
        }
    }

    private void Movement()
    {
        var speed = GameManager.current.playerSpeed;
        Vector3 inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical) * speed;
        
        inputVector = Vector3.ClampMagnitude(inputVector, speed);
        if (inputVector != Vector3.zero)
        {
            Vector3 temp = transform.position + inputVector * (Time.deltaTime);
            UnityEngine.AI.NavMeshHit hit;
            bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(temp, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            if (isvalid)
            {
                if ((transform.position - hit.position).magnitude >= 0.1f)
                {
                    _joystickValue = (transform.position - hit.position).magnitude;
                    _rb.MovePosition(temp);
                    _rb.MoveRotation(Quaternion.LookRotation(inputVector));
                    if (!once)
                    {
                        walking = true;
                        once = true;
                    }
                }
            }
            else
            {
                walking = false;
                once = false;
            }
        }
        else
        {
            walking = false;
            once = false;
        }
    }

    public float GetJoystickValue()
    {
        return _joystickValue;
    }
    
}
