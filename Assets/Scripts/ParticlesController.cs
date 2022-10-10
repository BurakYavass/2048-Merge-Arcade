using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private GameObject healParticle;
    [SerializeField] private ParticleSystem hitParticle;
    private PlayerController _playerController;
    private PlayerCollisionHandler _playerCollisionHandler;

    private bool _healArea;
    private bool _hit;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _healArea = _playerController._healing;

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
}
