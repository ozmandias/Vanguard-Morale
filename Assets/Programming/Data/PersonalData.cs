using UnityEngine;

[System.Serializable]
public class PersonalData {
    public Rigidbody body;
    public Collider collider;
    public GameObject attackColliderObject;
    public GameObject raycastShooterObject;
    public GameObject cameraFollowObject;
    public GameObject groundCheckObject;
    public RuntimeAnimatorController playerRuntimeAnimator;
    public RuntimeAnimatorController personRuntimeAnimator;
    public GameObject []weaponObjects; // use for both attackCollider and raycastShooter
    public Vector3 weaponDefaultPosition;
    public Vector3 weaponDefaultEulerRotation;
    public Vector3 weaponDefaultScale;
    public Transform weaponParentTransform;
}