using DG.Tweening;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private GameObject healParticle;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private ParticleSystem upgradeParticle;
    private PlayerController _playerController;
    private PlayerCollisionHandler _playerCollisionHandler;

    private bool _healArea;
    private bool _hit;
    
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _healArea = _playerController.healing;

        // if (_hit)
        // {
        //     hitParticle.Play();
        // }
        // else
        // {
        //     hitParticle.Stop();
        // }

        if (_healArea)
        {
            healParticle.SetActive(true);
        }
        else
        {
            healParticle.SetActive(false);
        }
        
    }

    public void PlayerUpgrade()
    {
        upgradeParticle.Play();
    }
}
