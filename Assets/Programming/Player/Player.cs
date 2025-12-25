using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    float horizontal;
    float vertical;
    Vector3 direction;
    float rotateAngle;
    [Header("Move Settings")]
    public float speed = 30f;
    public float jumpForce = 60f;
    public float gravity = 9.81f;
    public bool isJumping = false;
    public bool isGrounded = true;
    public float groundCheckDistance = 6f;

    [Header("Attack Settings")]
    public bool isAttacking = false;
    public int attackNumber = 0;
    [SerializeField] float attackTimer = 0;
    [SerializeField] float attackAnimationLength = 0;
    public Collider attackCollider;
    
    [Header("Player Settings")]
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Camera playerCamera;
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] AnimationManager playerAnimation;
    [SerializeField] CombatManager playerCombat;

    public virtual void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerCamera = Camera.main;

        playerAnimation = GetComponent<AnimationManager>();
        playerCombat = GetComponent<CombatManager>();

        playerInfo.Init(gameObject);
        /*playerBody.interpolation = RigidbodyInterpolation.Interpolate;
        playerBody.collisionDetectionMode = CollisionDetectionMode.Continuous;*/
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

        if(isAttacking == true || playerCombat.managingMove) {
            direction = Vector3.zero;
            StopAnimation("Run");
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true && isJumping == false && isAttacking == false) {
            isJumping = true;
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            PlayAnimation("Jump");
        }

        // direction.y -= gravity * Time.deltaTime;
        // playerBody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);

        gameObject.transform.position += direction;
    }

    public void Rotate() {
        rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.Euler(Vector3.up * (rotateAngle + playerCamera.transform.eulerAngles.y));
    }

    public void Attack() {
        if(Input.GetKeyDown(KeyCode.Mouse0) && isJumping == false && playerCombat.managingAttack == false) {
            isAttacking = true;
            attackCollider.enabled = true;
            attackNumber += 1;
            attackNumber = attackNumber == 3 ? 1 : attackNumber; /*Mathf.Clamp(attackNumber, 1, 2)*/
            PlayAnimation("Attack" + attackNumber);
            attackTimer = 0;
        }

        if(isAttacking == true) {
            attackTimer += Time.deltaTime;

            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackNumber)) {
                attackAnimationLength = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
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
            playerAnimator.SetBool("Run", true);
        } else if(animationName == "Jump") {
            playerAnimator.SetTrigger("Jump");
        } else if(animationName == "ReachGround") {
            playerAnimator.SetTrigger("ReachGround");
        } else if(animationName == "Attack1") {
            playerAnimator.SetTrigger("Attack1");
        } else if(animationName == "Attack2") {
            playerAnimator.SetTrigger("Attack2");
        }
    }

    void StopAnimation(string animationName) {
        if (animationName == "Run") {
            playerAnimator.SetBool("Run", false);
        } else if (animationName == "Attack1") {
            playerAnimator.ResetTrigger("Attack1");
        } else if (animationName == "Attack2") {
            playerAnimator.ResetTrigger("Attack2");
        }
    }

    public void CheckGround() {
        StartCoroutine(CheckGroundCoroutine());
    }

    public PlayerInfo GetInfo()
    {
        return playerInfo;
    }

    public IEnumerator CheckGroundCoroutine()
    {
        RaycastHit playerRaycastHit;

        if (isJumping == true || isGrounded == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Debug.DrawRay(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down * groundCheckDistance, Color.white);
        if (Physics.Raycast(gameObject.transform.Find("GroundCheck").gameObject.transform.position, Vector3.down, out playerRaycastHit, groundCheckDistance))
        {
            if (playerRaycastHit.collider.gameObject.CompareTag("Runnable"))
            {
                isGrounded = true;
                if (isJumping == true)
                {
                    isJumping = false;
                    PlayAnimation("ReachGround");
                }
            }
        }
        else
        {
            isGrounded = false;
        }
    }
}