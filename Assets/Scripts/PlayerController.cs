using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform handle;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public GameObject[] closePart;

    public bool walking = false;
    private bool upgradeArea = false;
    private bool teleporting = false;

    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }

    private void Start()
    {
        GameEventHandler.current.OnPlayerUpgradeArea += OnPlayerUpgradeArea;
        DOTween.Init();
    }

    private void OnDestroy()
    {
        GameEventHandler.current.OnPlayerUpgradeArea -= OnPlayerUpgradeArea;
    }

    private void OnPlayerUpgradeArea(bool enterExit)
    {
        
        if (enterExit)
        {
            transform.DOMove(new Vector3(37f, 0.63f, 24.75f), 0.1f).OnUpdate((() =>
                {
                    upgradeArea = true;
                    walking = true;
                }))
                .OnComplete((() =>
                {
                    transform.DORotate(new Vector3(0, -66, 0), 0.1f);
                    upgradeArea = true;
                    walking = false;
                }));
            
        }
        else
        {
            walking = false;
            upgradeArea = false;
            // transform.DOMove(new Vector3(24.0f, 0.63f, 26.0f), 1.0f)
            //     .OnUpdate((() =>
            //     {
            //         upgradeArea = true;
            //     }))
            //     .OnComplete((() =>
            //     {
            //         upgradeArea = false;
            //         walking = false;
            //     }));
            
        }
        
    }

    public void OnPlayerTeleport(bool teleport)
    {
        teleporting = teleport;
    }

    void FixedUpdate()
    {
        if (joystick.isActiveAndEnabled && _virtualCamera != null && !upgradeArea && !teleporting)
        {
            Movement();
        }
        else
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }

    public void CameraChanger(int cam)
    {
        _virtualCamera = virtualCameras[0];
        // if (cam == 3)
        // {
        //     _virtualCamera = virtualCameras[cam];
        // }
        // else
        // {
        //     _virtualCamera = virtualCameras[cam];
        // }
    }

    private void Movement()
    {
        var speed = GameManager.current.playerSpeed;
        Vector3 inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        inputVector = Vector3.ClampMagnitude(inputVector, speed);
        if (inputVector != Vector3.zero)
        {
            var cameraTransform = _virtualCamera.transform;
            var forward = cameraTransform.forward;
            var cameraForwardHorizontal = new Vector3(forward.x, 0f, forward.z).normalized;

            var right = cameraTransform.right;
            var cameraRightHorizontal = new Vector3(right.x, 0f, right.z).normalized;

            var movementVector = inputVector.x * cameraRightHorizontal + inputVector.z * cameraForwardHorizontal;
            
            Vector3 temp = transform.position + transform.forward* (speed * Time.fixedDeltaTime);
            /* UnityEngine.AI.NavMeshHit hit;
            // bool isvalid = UnityEngine.AI.NavMesh.SamplePosition(temp, out hit, .3f, UnityEngine.AI.NavMesh.AllAreas);
            // if (isvalid)
            // {
            //     if ((transform.position - hit.position).magnitude >= 0.1f)
            //     {
            //         transform.position = temp;
            //         var lerp = Vector3.Lerp(transform.forward, movementVector, speed * Time.fixedDeltaTime);
            //         transform.rotation = Quaternion.LookRotation(lerp);
            //         walking = true;
            //     }
            // }
            // else
            // {
            //     walking = false;
             }*/
            transform.position = temp;
            var lerp = Vector3.Lerp(transform.forward, movementVector, (speed *1.1f)* Time.fixedDeltaTime);
            transform.rotation = Quaternion.LookRotation(lerp);
            walking = true;
        }
        else
        {
            walking = false;
        }
    }
}
