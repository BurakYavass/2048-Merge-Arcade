using System;
using Cinemachine;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController current;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private PlayerAnimationHandler _playerAnimationHandler;
    private CharacterController _characterController;

    [SerializeField]private float turnSpeed;

    public bool walking = false;
    private bool once = false;
    
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
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
        var inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        if (joystick.Horizontal != 0f || joystick.Vertical != 0f)
        {
            if (!once)
            {
                walking = true;
                once = true;
            }
        }
        else
        {
            walking = false;
            once = false;
        }

        var cameraTransform = virtualCamera.transform;
        var forward = cameraTransform.forward;
        var cameraForwardHorizontal =
            new Vector3(forward.x, 0f, forward.z).normalized;

        var right = cameraTransform.right;
        var cameraRightHorizontal =
            new Vector3(right.x, 0f, right.z).normalized;

        var movementVector = inputVector.x * cameraRightHorizontal + inputVector.z * cameraForwardHorizontal;

        var speed = GameManager.current.playerSpeed;
      
        _characterController.Move(movementVector.normalized * (speed * Time.deltaTime));

        if (inputVector.magnitude > 0)
        {
            var newRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            
            transform.rotation = newRotation;
        }
    }
    
    //var rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.fixedDeltaTime);
    
    //var dot = Mathf.Clamp(Vector3.Dot(transform.forward, inputVector),0,1);

    //_currentMoveMultiplier = Mathf.Lerp(_currentMoveMultiplier, dot, _acceleration * Time.fixedDeltaTime);
        
    //var position = transform.position + transform.forward.normalized * (speed * dot * Time.fixedDeltaTime);

    //transform.position += movementVector.normalized * (speed * Time.deltaTime);
    
}
