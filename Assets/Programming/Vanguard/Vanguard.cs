using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanguard : MonoBehaviour {
	float horizontal;
	float vertical;
	Vector3 direction;
	float rotateAngle;
	[Header("Move Settings")]
	[SerializeField] float speed = 30f;
	[SerializeField] float jumpForce = 60f;
	[SerializeField] float gravity = 9.81f;
	[SerializeField] bool isJumping = false;
	[SerializeField] bool atGround = true;
	[SerializeField] float groundCheckDistance = 6f;

	[Header("Attack Settings")]
	[SerializeField] bool isAttacking = false;
	[SerializeField] int attackNumber = 0;
	[SerializeField] float attackTimer = 0;
	[SerializeField] float attackAnimationLength = 0;
	[SerializeField] Collider attackCollider;

	[Header("Vanguard Settings")]
	[SerializeField] Rigidbody vanguardBody;
	[SerializeField] Animator vanguardAnimator;
	[SerializeField] Camera vanguardCamera;
	[SerializeField] VanguardInfo vanguardInfo;
	[SerializeField] AnimationManager vanguardAnimation;
	[SerializeField] CombatManager vanguardCombat;

	// Use this for initialization
	void Start()
	{
		vanguardBody = gameObject.GetComponent<Rigidbody>();
		vanguardAnimator = gameObject.GetComponent<Animator>();
		vanguardCamera = Camera.main;

		attackCollider = GameObject.Find("VanguardAttackCollider").GetComponent<Collider>();
		attackCollider.enabled = false;

		vanguardAnimation = gameObject.GetComponent<AnimationManager>();
		vanguardCombat = gameObject.GetComponent<CombatManager>();

		vanguardInfo.Init(gameObject);
		/*vanguardBody.interpolation = RigidbodyInterpolation.Interpolate;
		vanguardBody.collisionDetectionMode = CollisionDetectionMode.Continuous;*/
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

		if(direction != Vector3.zero && vanguardCombat.managingMove == false) {
			Rotate();
			direction = transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);
			PlayAnimation("Run");
		} else {
			StopAnimation("Run");
		}

		if(isAttacking == true || vanguardCombat.managingMove) {
			direction = Vector3.zero;
			StopAnimation("Run");
		}

		if(Input.GetKeyDown(KeyCode.Space) && atGround == true && isJumping == false && isAttacking == false && vanguardCombat.managingMove == false) {
			isJumping = true;
			// direction.y += jumpForce * Time.deltaTime;
			vanguardBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
			PlayAnimation("Jump");
		}

		// direction.y -= gravity * Time.deltaTime;
		// vanguardBody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);

		gameObject.transform.position += direction;
	}

	void Rotate() {
		rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
		gameObject.transform.rotation = Quaternion.Euler(Vector3.up * (rotateAngle + vanguardCamera.transform.eulerAngles.y));
	}

	void Attack() {
		if(Input.GetKeyDown(KeyCode.Mouse0) && isJumping == false && vanguardCombat.managingAttack == false) {
			isAttacking = true;
			attackCollider.enabled = true;
			attackNumber = attackNumber + 1; /*Random.Range(1,3)*/
			attackNumber = attackNumber == 3 ? 1 : attackNumber;  /*Mathf.Clamp(attackNumber, 1, 2)*/
			PlayAnimation("Attack" + attackNumber);
			attackTimer = 0;
		}

		if(isAttacking) {
			attackTimer += Time.deltaTime;
			
			if(vanguardAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackNumber)) {
				attackAnimationLength = vanguardAnimator.GetCurrentAnimatorStateInfo(0).length;
			}

			if(attackTimer > attackAnimationLength && attackAnimationLength > 0) {
				isAttacking = false;
				attackCollider.enabled = false;
				attackNumber = 0;
				attackTimer = 0;
				attackAnimationLength = 0;
			}
		}
	}

	void PlayAnimation(string animationName) {
		if(animationName == "Run") {
			vanguardAnimator.SetBool("Run", true);
		} else if(animationName == "Jump") {
			vanguardAnimator.SetTrigger("Jump");
		} else if(animationName == "ReachGround") {
			vanguardAnimator.SetTrigger("ReachGround");
		} else if(animationName == "Attack1") {
			vanguardAnimator.SetTrigger("Attack1");
		} else if(animationName == "Attack2") {
			vanguardAnimator.SetTrigger("Attack2");
		}
	}

	void StopAnimation(string animationName) {
		if (animationName == "Run") {
			vanguardAnimator.SetBool("Run", false);
		} else if (animationName == "Attack1") {
			vanguardAnimator.ResetTrigger("Attack1");
		} else if (animationName == "Attack2") {
			vanguardAnimator.ResetTrigger("Attack2");
		}
	}

	void CheckGround() {
		StartCoroutine(CheckGroundCoroutine());
	}

	void OnTriggerEnter(Collider otherCollider) {
		if(otherCollider.gameObject.CompareTag("EnemyAttackCollider")) {
			
		}
	}

	public VanguardInfo GetInfo()
	{
		return vanguardInfo;
	}

	IEnumerator CheckGroundCoroutine() {
		RaycastHit vanguardRaycastHit;

		if(isJumping == true || atGround == false) {
			yield return new WaitForSeconds(0.1f);
		}

		Debug.DrawRay(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down * groundCheckDistance, Color.white);
		if(Physics.Raycast(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down, out vanguardRaycastHit, groundCheckDistance)) {
			if(vanguardRaycastHit.collider.gameObject.CompareTag("Runnable")) {
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