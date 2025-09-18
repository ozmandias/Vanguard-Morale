using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public ParticleSystem counterParticle;
    Coroutine particleCoroutine;

    void Start() { }

    void Update() { }

    public void StartParticle(string particleName) {
        switch(particleName) {
            case "counter":
                particleCoroutine = StartCoroutine(SetParticleCoroutine(counterParticle, "on"));
                break;
            default:
                break;
        }
    }

    public void StopParticle(string particleName) {
        if (particleCoroutine != null) StopCoroutine(particleCoroutine);
        switch (particleName) {
            case "counter":
                particleCoroutine = StartCoroutine(SetParticleCoroutine(counterParticle, "off"));
                break;
            default:
                break;
        }
    }

    IEnumerator SetParticleCoroutine(ParticleSystem particle, string particleSwitch) {
        if(particle == counterParticle) {
            yield return new WaitUntil(() => GetComponent<CombatManager>().counterAlert == true);
        }
        if (particleSwitch == "on") {
            particle.Play();
        } else {
            particle.Clear();
            particle.Stop();
        }
        yield return null;
    }
}