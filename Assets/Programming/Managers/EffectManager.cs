using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public Effect attackEffect;
    public Effect counterEffect;
    Coroutine effectCoroutine;

    void Start() { }

    void Update() { }

    public void StartEffect(string particleName) {
        switch(particleName) {
            case "attack":
                effectCoroutine = StartCoroutine(SetEffectCoroutine(attackEffect, "on"));
                break;
            case "counter":
                effectCoroutine = StartCoroutine(SetEffectCoroutine(counterEffect, "on"));
                break;
            default:
                break;
        }
    }

    public void StopEffect(string effectName) {
        if (effectCoroutine != null) StopCoroutine(effectCoroutine);
        switch (effectName) {
            case "attack":
                break;
            case "counter":
                effectCoroutine = StartCoroutine(SetEffectCoroutine(counterEffect, "off"));
                break;
            default:
                break;
        }
    }

    public void DestroyEffect(GameObject destroyEffect) {
        StartCoroutine(DestroyEffectCoroutine(destroyEffect));
    }

    IEnumerator SetEffectCoroutine(Effect effect, string effectSwitch) {
        if (effectSwitch == "on") {
            if(effect == counterEffect) {
                yield return new WaitUntil(() => GetComponent<CombatManager>().counterAlert == true);
            }
            effect.Play();
        } else {
            effect.Clear();
            effect.Stop();
        }
        yield return null;
    }

    IEnumerator DestroyEffectCoroutine(GameObject destroyObject) {
        yield return new WaitForSeconds(1);
        Destroy(destroyObject);
    }
}