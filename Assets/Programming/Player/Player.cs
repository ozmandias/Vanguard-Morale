using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Person {
    float horizontal;
    float vertical;
    Vector3 direction;
    float rotateAngle;
    public float speed = 30f;
    public float jumpForce = 100f;
    public float gravity = 9.81f;
    public float forceLevel;
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Camera playerCamera;
    [SerializeField] Info playerInfo;

    public virtual void Start() {
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerCamera = Camera.main;

        playerInfo = GetComponent<Info>();
    }

    public virtual void Update() {
        Move();
        Attack();
    }

    public override void Move() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        direction = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        if(direction != Vector3.zero) {
            Rotate();
            direction = transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);
            PlayAnimation("Run");
        } else {
            StopAnimation("Run");
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            playerBody.AddForce(Vector3.up * jumpForce * forceLevel);
        }

        playerBody.AddForce(Vector3.down * gravity * forceLevel);
        
        gameObject.transform.position += direction;
    }

    public override void Rotate() {
        rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.Euler(Vector3.up * (rotateAngle + playerCamera.transform.eulerAngles.y));
    }

    public override void Attack() {
        
    }

    void PlayAnimation(string animationName) {
        if(animationName == "Run") {
            playerAnimator.SetBool("Run", true);
        }
    }

    void StopAnimation(string animationName) {
        if(animationName == "Run") {
            playerAnimator.SetBool("Run", false);
        }
    }
}