using UnityEngine;

public class RagdollManager : MonoBehaviour {
    float ragDollTimer = 0;
    public bool ragdollEnabled = false;
    [SerializeField] Animator mainAnimator;
    [SerializeField] Rigidbody mainBody;
    [SerializeField] Collider mainCollider;
    [SerializeField] GameObject boneBase;
    [SerializeField] Rigidbody[] ragdollBodies;
    [SerializeField] Collider[] ragdollColliders;

    void Start() {
        ragdollBodies = boneBase.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = boneBase.GetComponentsInChildren<Collider>();
        boneBase.AddComponent<RagdollPhysics>();

        DisableRagdoll();
    }

    void Update() {
        
    }

    public void EnableRagdoll() {
        ragdollEnabled = true;

        mainAnimator.enabled = false;
        mainBody.isKinematic = true;
        mainCollider.enabled = false;

        foreach (var body in ragdollBodies)
        {
            body.isKinematic = false;
        }

        foreach (var collider in ragdollColliders)
        {
            collider.enabled = true;
        }
    }

    public void DisableRagdoll() {
        ragdollEnabled = false;

        mainAnimator.enabled = true;
        mainBody.isKinematic = false;
        mainCollider.enabled = true;

        foreach (var body in ragdollBodies)
        {
            body.isKinematic = true;
        }

        foreach (var collider in ragdollColliders) {
            collider.enabled = false;
        }
    }

    public void ClearRagdoll() {
        ragdollEnabled = false;

        foreach (var body in ragdollBodies)
        {
            body.isKinematic = true;
        }

        foreach (var collider in ragdollColliders) {
            collider.enabled = false;
        }
    }

    void EnableRagdollOnTimer() {
        if(ragDollTimer != -1f) {
            ragDollTimer += Time.deltaTime;
            if(ragDollTimer > 3f) {
                EnableRagdoll();
                ragDollTimer = -1f;
            }
        }
    }
}