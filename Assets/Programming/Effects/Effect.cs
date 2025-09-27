using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    public ParticleSystem particle;
    public EffectType effectType = EffectType.Play;

    void Start() {
        particle = GetComponent<ParticleSystem>();
    }

    void Update() {

    }

    public void Play() {
        particle.Play();
    }

    public void Clear() {
        particle.Clear();
    }

    public void Stop() {
        particle.Stop();
    }

    public GameObject Create(Transform createTransform) {
        GameObject newEffect = GameObject.Instantiate(gameObject, createTransform.position, createTransform.rotation);
        newEffect.GetComponent<Effect>().Play();
        return newEffect;
    }
}