using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterKnight : MonoBehaviour {
	float horizontal;
	float vertical;
	Vector3 direction;
	float rotateAngle;
	[Header("Move Settings")]
	[SerializeField] float speed = 30f;
	[SerializeField] float jumpForce = 100f;
	[SerializeField] float gravity = 9.81f;
	[SerializeField] float forceLevel;
	[SerializeField] bool isJumping = false;
	[SerializeField] bool atGround = true;
	[SerializeField] float groundCheckDistance = 1f;

	[Header("Attack Settings")]
	[SerializeField] bool isAttacking = false;
	[SerializeField] int attackNumber = 0;
	[SerializeField] float attackTimer = 0;
	[SerializeField] float attackAnimationLength = 0;

	[Header("Master Knight Settings")]
	[SerializeField] Rigidbody masterKnightBody;
	[SerializeField] Animator masterKnightAnimator;
	[SerializeField] Camera masterKnightCamera;

	// Use this for initialization
	void Start () {
		masterKnightBody = GetComponent<Rigidbody>();
		masterKnightAnimator = GetComponent<Animator>();
		masterKnightCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Attack();
	}

	void FixedUpdate() {
		CheckGround();
	}

	void Move() {
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

		if(isAttacking == true) {
			direction = Vector3.zero;
			StopAnimation("Run");
		}

		if(Input.GetKeyDown(KeyCode.Space) && atGround == true && isJumping == false) {
			// direction.y += jumpForce * Time.deltaTime;
			isJumping = true;
			masterKnightBody.AddForce(Vector3.up * jumpForce * forceLevel);
			PlayAnimation("Jump");
		}

		// direction.y -= gravity * Time.deltaTime;
		masterKnightBody.AddForce(Vector3.down * gravity * forceLevel);
		gameObject.transform.position += direction;
	}

	void Rotate() {
		rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
		gameObject.transform.rotation = Quaternion.Euler(Vector3.up * (rotateAngle + masterKnightCamera.transform.eulerAngles.y));
	}

	void Attack() {
		if(Input.GetKeyDown(KeyCode.Mouse0)) {
			isAttacking = true;
			attackNumber = attackNumber + 1; /*Random.Range(1,3)*/
			attackNumber = Mathf.Clamp(attackNumber, 1, 2);
			PlayAnimation("Attack" + attackNumber);
			attackTimer = 0;
		}

		if(isAttacking) {
			attackTimer += Time.deltaTime;
			
			if(masterKnightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackNumber)) {
				attackAnimationLength = masterKnightAnimator.GetCurrentAnimatorStateInfo(0).length;
			}

			if(attackTimer > attackAnimationLength && attackAnimationLength > 0) {
				isAttacking = false;
				attackNumber = 0;
				attackTimer = 0;
				attackAnimationLength = 0;
			}
		}
	}

	void PlayAnimation(string animationName) {
		if(animationName == "Run") {
			masterKnightAnimator.SetBool("Run", true);
		} else if(animationName == "Jump") {
			masterKnightAnimator.SetTrigger("Jump");
		} else if(animationName == "ReachGround") {
			masterKnightAnimator.SetTrigger("ReachGround");
		} else if(animationName == "Attack1") {
			masterKnightAnimator.SetTrigger("Attack1");
		} else if(animationName == "Attack2") {
			masterKnightAnimator.SetTrigger("Attack2");
		}
	}

	void StopAnimation(string animationName) {
		if(animationName == "Run") {
			masterKnightAnimator.SetBool("Run", false);
		}
	}

	void CheckGround() {
		StartCoroutine(CheckGroundCoroutine());
	}

	void OnCollisionEnter(Collision otherCollision) {
		// if(otherCollision.collider.gameObject.CompareTag("")) {
		// }
	}

	IEnumerator CheckGroundCoroutine() {
		RaycastHit masterKnightRaycastHit;

		if(isJumping == true || atGround == false) {
			yield return new WaitForSeconds(0.1f);
		}

		Debug.DrawRay(GameObject.Find("GroundCheck").transform.position, Vector3.down * groundCheckDistance, Color.white);
		if(Physics.Raycast(GameObject.Find("GroundCheck").transform.position, Vector3.down, out masterKnightRaycastHit, groundCheckDistance)) {
			if(masterKnightRaycastHit.collider.gameObject.CompareTag("Runnable")) {
				atGround = true;
				if(isJumping == true) {
					isJumping = false;
					PlayAnimation("ReachGround");
				}
			}
		} else {
			atGround = false;
		}
	}
}
