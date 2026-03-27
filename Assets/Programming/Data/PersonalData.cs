using UnityEngine;

[System.Serializable]
public class PersonalData {
    public Rigidbody body;
    public Collider collider;
    public GameObject attackColliderObject;
    public GameObject cameraFollowObject;
    public GameObject groundCheckObject;
    public RuntimeAnimatorController playerRuntimeAnimator;
    public RuntimeAnimatorController personRuntimeAnimator;
    public GameObject weaponObject;
    public Vector3 weaponDefaultPosition;
    public Vector3 weaponDefaultEulerRotation;
    public Vector3 weaponDefaultScale;
    public Transform weaponParentTransform;
}