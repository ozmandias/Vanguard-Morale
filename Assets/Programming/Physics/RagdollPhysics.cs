using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPhysics : MonoBehaviour {
    RagdollManager mainRagdollManager;

    void Start() {
        mainRagdollManager = GetComponentInParent<RagdollManager>();
    }

    void OnCollisionEnter(Collision otherCollision) {
        if(otherCollision.gameObject.CompareTag("Runnable") && mainRagdollManager.ragdollEnabled) {
            StartCoroutine(ClearRagdollCoroutine());
        }
    }

    IEnumerator ClearRagdollCoroutine() {
        yield return new WaitForSeconds(1f);
        mainRagdollManager.ClearRagdoll();
    }
}