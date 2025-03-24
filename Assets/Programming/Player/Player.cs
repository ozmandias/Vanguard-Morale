using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour {
    float horizontal;
    float vertical;
    Vector3 direction;
    float rotateAngle;
    public float speed = 30f;
    public float jumpForce = 100f;
    public float gravity = 9.81f;
    public float forceLevel;
    public bool isJumping = false;
    public bool isGrounded = true;
    public float groundCheckDistance = 1f;
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

    public virtual void FixedUpdate() {
        CheckGround();
    }

    public void Move() {
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

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true && isJumping == false) {
            isJumping = true;
            playerBody.AddForce(Vector3.up * jumpForce * forceLevel);
            PlayAnimation("Jump");
        }

        playerBody.AddForce(Vector3.down * gravity * forceLevel);
        
        gameObject.transform.position += direction;
    }

    public void Rotate() {
        rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.Euler(Vector3.up * (rotateAngle + playerCamera.transform.eulerAngles.y));
    }

    public void Attack() {
        
    }

    void PlayAnimation(string animationName) {
        if(animationName == "Run") {
            playerAnimator.SetBool("Run", true);
        } else if(animationName == "Jump") {
            playerAnimator.SetTrigger("Jump");
        } else if(animationName == "ReachGround") {
            playerAnimator.SetTrigger("ReachGround");
        }
    }

    void StopAnimation(string animationName) {
        if(animationName == "Run") {
            playerAnimator.SetBool("Run", false);
        }
    }

    public void CheckGround() {
        StartCoroutine(CheckGroundCoroutine());
    }

    public IEnumerator CheckGroundCoroutine() {
        RaycastHit playerRaycastHit;

		if(isJumping == true || isGrounded == false) {
			yield return new WaitForSeconds(0.1f);
		}

		Debug.DrawRay(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down * groundCheckDistance, Color.white);
		if(Physics.Raycast(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down, out playerRaycastHit, groundCheckDistance)) {
			if(playerRaycastHit.collider.gameObject.CompareTag("Runnable")) {
				isGrounded = true;
				if(isJumping == true) {
					isJumping = false;
					PlayAnimation("ReachGround");
				}
			}
		} else {
			isGrounded = false;
		}
    }
}