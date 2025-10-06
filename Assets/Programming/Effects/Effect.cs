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

    public GameObject Create(Vector3 createPosition, Quaternion createRoatation) {
        GameObject newEffect = GameObject.Instantiate(gameObject, createPosition, createRoatation);
        newEffect.GetComponent<Effect>().Play();
        return newEffect;
    }

    public void SetOwner(GameObject _owner) {
        owner = _owner;
    }

    void OnParticleCollision(GameObject otherGameObject) {
        if(otherGameObject.CompareTag("Person") && owner != null) {
            Info ownerInfo = GameHelpers.GetCharacterInfo(owner);
            Debug.Log("ownerInfo-particle:" + ownerInfo);
            otherGameObject.GetComponent<Person>().MakePersonHurt(ownerInfo);
        }
    }
}