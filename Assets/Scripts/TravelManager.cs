using System.Collections;
using DG.Tweening;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.UI;

public class TravelManager : MonoBehaviour
{
    public TravelType travelType;
    [SerializeField] private Transform yellowLevel;
    [SerializeField] private Transform greenLevel;
    [SerializeField] private Transform purpleLevel;
    [SerializeField] private Transform homeStage;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject[] openPart;
    [SerializeField] private GameObject firstTimePlay;
    [SerializeField] private BallController ballController;
    [SerializeField] private float travelDelay;
    [SerializeField] private UIManager uiManagser;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject homeTeleportCanvas;

    private Collider _collider;
    private Transform _travelPoint;

    public bool active;
    private bool _once = false;
    public bool homeButtonActiveNow;
    
    

    void Start()
    {
        PlayerPrefs.GetInt("yellow", 0);
        PlayerPrefs.GetInt("purple", 0);
        PlayerPrefs.GetInt("homeButton", 0);
        _collider = GetComponent<Collider>();
        
        if (travelType == TravelType.YellowStage)
        {
            _travelPoint = yellowLevel;
            if (PlayerPrefs.GetInt("yellow")==1)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }
        else if (travelType == TravelType.GreenStage)
        {
            _travelPoint = greenLevel;
            
        }
        else if (travelType == TravelType.PurpleStage)
        {
            _travelPoint = purpleLevel;
            if (PlayerPrefs.GetInt("purple") == 1)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }
        else if (travelType == TravelType.HomeStage)
        {
            _travelPoint = homeStage;
            if (PlayerPrefs.GetInt("homeButton")==1)
            {
                active = false;
                homeButtonActiveNow = true;
            }
            else
            {
                active = true;
                homeButtonActiveNow = false;
                
            }
        }

        if (active)
        {
            _collider.enabled = true;
            if (openPart.Length >0)
            {
                foreach (var particle in openPart)
                {
                    particle.SetActive(true);
                }
            }
            
        }
        else
        {
            if (openPart.Length > 0)
            {
                foreach (var particle in openPart)
                {
                    particle.SetActive(false);
                }
            }
            _collider.enabled = false;
        }
    }
    
    private void Update()
    {
        if (active)
        {
            if (!_once)
            {
                _once = true;
                _collider.enabled = true;
                foreach (var particle in openPart)
                {
                    particle.SetActive(true);
                }

                if (travelType == TravelType.YellowStage)
                {
                    PlayerPrefs.SetInt("yellow", 1);
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Gate 1");
                }
                else if (travelType == TravelType.PurpleStage)
                {
                    PlayerPrefs.SetInt("purple",1);
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Gate 2");
                }
                
            }
        }
        else
        {
            if (travelType == TravelType.HomeStage)
            {
                _collider.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            
            if (travelType == TravelType.HomeStage)
            {
                homeTeleportCanvas.SetActive(true);
                uiManagser.PlayerHomeTravelButton(true);
                homeButtonActiveNow = true;
                active = false;
                PlayerPrefs.SetInt("homeButton",1);
            }
            else
            {
                firstTimePlay.SetActive(true);
                playerController.OnPlayerTeleport(true);
                foreach (var closePart in playerController.closePart)
                {
                    closePart.SetActive(false);
                }
                foreach (var variCollider in playerController.colliders)
                {
                    variCollider.enabled = false;
                }
                StartCoroutine(TravelWaiter());
                fillImage.DOFillAmount(1, travelDelay).OnComplete((() => fillImage.fillAmount = 0));
                firstTimePlay.transform.DOLocalMoveY(0.43f,travelDelay);
            }
        }
       
    }

    public enum TravelType
    {
        YellowStage,
        GreenStage,
        PurpleStage,
        HomeStage,
    }
    
    public void PlayerHomeTravel()
    {
        playerController.OnPlayerTeleport(true);
        StartCoroutine(TravelWaiter());
        foreach (var closePart in playerController.closePart)
        {
            closePart.SetActive(false);
        }

        foreach (var variCollider in playerController.colliders)
        {
            variCollider.enabled = false;
        }
    }

    IEnumerator TravelWaiter()
    {
        if (travelType != TravelType.HomeStage)
        {
            GameEventHandler.current.PlayerLeftArea(false);
        }
        yield return new WaitForSeconds(travelDelay);
        var travelPos = _travelPoint.transform.position;
        ballController.GoTravelPoint();
        playerController.gameObject.transform.DOMove(new Vector3(travelPos.x,travelPos.y+.5f,travelPos.z), 1.5f)
            .OnComplete((() =>
            {
                if (firstTimePlay)
                    firstTimePlay.SetActive(false);
                playerController.transform.forward = Vector3.forward;
                playerController.OnPlayerTeleport(false);
                foreach (var variCollider in playerController.colliders)
                {
                    variCollider.enabled = true;
                }
                foreach (var closePart in playerController.closePart)
                {
                    closePart.SetActive(true);
                }
            }));
        
    }
}
