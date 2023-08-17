using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    public List<ParticleSystem> ParticleList;

    public static PlayerParticleManager ParticleManager;

    void Awake()
    {
        if (ParticleManager == null)
        {
            ParticleManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (ParticleSystem particle in ParticleList)
        {
            particle.Stop();
        }
    }

    public void PlayParticle(string name)
    {
        ParticleSystem particle = ParticleList.Find(x => x.name == name);
        particle.Play();
    }

    public void PlayParticle(string name, Vector3 rotation)
    {
        ParticleSystem particle = ParticleList.Find(x => x.name == name);
        var Shape = particle.shape;
        Shape.rotation = rotation;
        particle.Play();
    }

    public void StopParticle(string name)
    {
        ParticleSystem particle = ParticleList.Find(x => x.name == name);
        particle.Stop();
    }

    public void PlayParticleWithTime(string name, float time)
    {
        ParticleSystem particle = ParticleList.Find(x => x.name == name);
        particle.Stop();
        particle.Play();
        StartCoroutine(SetTimer(particle, time));
    }

    IEnumerator SetTimer(ParticleSystem particle, float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        yield return wfs;
        particle.Stop();
    }

}
