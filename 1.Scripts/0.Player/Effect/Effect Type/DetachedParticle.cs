using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachedParticle : MonoBehaviour
{
    ParticleSystem Particle;
    PlayerDetachedEffect DetachedEffect;

    private void Awake()
    {
        TryGetComponent(out ParticleSystem Particle);
    }

    private void OnEnable()
    {
        TryGetComponent(out ParticleSystem Particle);
        TryGetComponent(out PlayerDetachedEffect DetachedEffect);
        Particle.Play();
        
        DetachedEffect.DeleteEffect(Particle.main.startLifetime.constant);
    }

    private void Return(){
        
    }
}
