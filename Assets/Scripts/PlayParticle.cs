using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustParticle;
    
    private void ParticlePlay()
    {
        dustParticle.Play();
    }
}
