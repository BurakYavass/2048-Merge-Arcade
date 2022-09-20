using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

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

    public void CameraChanger(int cam)
    {
        _virtualCamera = null;
        _virtualCamera = virtualCameras[cam];
    }

    private void Movement()
    {
        var speed = GameManager.current.playerSpeed;
        Vector3 inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical) * speed;
        
        inputVector = Vector3.ClampMagnitude(inputVector, speed);
        if (inputVector != Vector3.zero)
        {
            var cameraTransform = _virtualCamera.transform;
            var forward = cameraTransform.forward;
            var cameraForwardHorizontal = new Vector3(forward.x, 0f, forward.z).normalized;

            var right = cameraTransform.right;
            var cameraRightHorizontal = new Vector3(right.x, 0f, right.z).normalized;

            var movementVector = inputVector.x * cameraRightHorizontal + inputVector.z * cameraForwardHorizontal;
            
            Vector3 temp = transform.position + movementVector * (Time.deltaTime);
            UnityEngine.AI.NavMeshHit hit;
            bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(temp, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            if (isvalid)
            {
                if ((transform.position - hit.position).magnitude >= 0.1f)
                {
                    _joystickValue = (transform.position - hit.position).magnitude;
                    _rb.MovePosition(temp);
                    _rb.MoveRotation(Quaternion.LookRotation(movementVector));
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
