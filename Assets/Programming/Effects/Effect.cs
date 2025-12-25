using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    public ParticleSystem particle;
    public EffectType effectType = EffectType.Play;
    public bool canManageEffect = false;
    public GameObject owner;

    void Start() {
        particle = GetComponent<ParticleSystem>();
        
        if(effectType == EffectType.Play) {
            owner = transform.parent.gameObject;
        }
    }

    void Update() {
        if(effectType == EffectType.Play) {
            if(owner.CompareTag("Person")) {
                if(particle.main.loop && particle.isPlaying && owner.GetComponent<Person>().GetInfo().isDead) {
                    particle.Clear();
                    particle.Stop();
                }
            }
        }
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

    public GameObject Create(Vector3 createPosition, Quaternion createRotation) {
        GameObject newEffect = GameObject.Instantiate(gameObject, createPosition, createRotation);
        newEffect.GetComponent<Effect>().Play();
        return newEffect;
    }

    public void SetOwner(GameObject _owner) {
        owner = _owner;
    }

    void OnParticleCollision(GameObject otherGameObject) {
        if(otherGameObject.CompareTag("Person") && owner != null) {
            Info ownerInfo = GameHelpers.GetCharacterInfo(owner);
            otherGameObject.GetComponent<Person>().HurtByOther(ownerInfo);
        }
    }
}