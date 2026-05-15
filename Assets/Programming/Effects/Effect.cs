using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    public ParticleSystem particle;
    public EffectType effectType = EffectType.Play;
    public bool canCreateEffect = false;
    public GameObject owner;

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

    void OnParticleCollision(GameObject otherGameObject) {
        if(otherGameObject.CompareTag("Player") && owner != null) {
            Debug.Log("Hurt by Boss");
        }

        /*if(otherGameObject.CompareTag("Person") && owner != null) {
            CharacterInfo ownerInfo = GameHelpers.GetCharacterInfo(owner);
            otherGameObject.GetComponent<Person>().HurtByOther(ownerInfo);
        }*/

        if((otherGameObject.CompareTag("Citizen") || otherGameObject.CompareTag("Soldier") || otherGameObject.CompareTag("Leader")) && owner != null) {
            CharacterInfo ownerInfo = GameHelpers.GetCharacterInfo(owner);
            otherGameObject.GetComponent<Person>().HurtByOther(ownerInfo);
        }
    }
}