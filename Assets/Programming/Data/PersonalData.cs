using UnityEngine;

public class PersonalData : MonoBehaviour {
    public Rigidbody body;
    public Collider collider;
    public GameObject attackColliderObject;
    public GameObject cameraFollowObject;
    public GameObject groundCheckObject;
    public RuntimeAnimatorController playerRuntimeAnimator;
    public RuntimeAnimatorController personRuntimeAnimator;
    public GameObject weaponObject;
    public Transform weaponDefaultTransform;
    public Transform weaponParentTransform;
}