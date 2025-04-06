using UnityEngine;
using UnityEngine.AI;

public abstract class Person : MonoBehaviour {
    [Header("Move Settings")]
    public float speed = 20f;

    [Header("Attack Settings")]
    public bool isAttacking = false;
    public GameObject target;
    public bool attackingTarget = false;
    public Collider attackCollider;

    [Header("Person Settings")]
    public AnimationManager personAnimation;
    public NavMeshAgent personAgent;
    public PersonInfo personInfo;
    public Transform personDestination;
    public StateMachine personState = StateMachine.Idle;

    public virtual void Start() {
        personAnimation = GetComponent<AnimationManager>();
        personAgent = GetComponent<NavMeshAgent>();
        personInfo = GetComponent<PersonInfo>();
    }

    public virtual void Update() {
        if(personInfo.stateMachineDead == false) {
            switch(personState) {
                case StateMachine.Idle:
                    Idle();
                    break;
                case StateMachine.Move:
                    Move();
                    break;
                case StateMachine.Attack:
                    Attack();
                    break;
                case StateMachine.Work:
                    Work();
                    break;
                case StateMachine.Follow:
                    Follow();
                    break;
                case StateMachine.Dead:
                    Dead();
                    break;
                default:
                    break;
            }
            
            if(personInfo.personType != PersonType.Neutral) {
                // FindTarget();
            }
        }
        if(personInfo.isDead == true) {
            if(Input.GetKeyDown(KeyCode.R)) {
                Resurrect();
            }
        }
    }

    public abstract void Idle();
    public abstract void Move();
    public abstract void Attack();
    public abstract void Work();
    public abstract void Follow();

    /*bool collision = false;
    float collisionTimer = 0;*/
    float nextHurtTime = 0;
    float hitRate = 0.8f /*1f*/;
    void OnTriggerEnter(Collider otherCollider) {
        if(otherCollider.gameObject.CompareTag("MasterKnightAttackCollider") || otherCollider.gameObject.CompareTag("PlayerAttackCollider")) {
            // collisionTimer += Time.deltaTime;
            if(Time.time > nextHurtTime /*collision == false*/ && personInfo.isDead == false /*&& collisionTimer > 0.01f*/) {
                /*collision = true;
                collisionTimer = 0;*/
                nextHurtTime = Time.time + hitRate;

                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Info>();
                Hurt(attackCharacterInfo.damage);
            }
        }
    }
    void OnTriggerExit(Collider otherCollider) {
        // collision = false;
    }

    public void ChangeState(StateMachine _state) {
        personState = _state;
    }

    public virtual void SetTarget(GameObject _newTarget) {
        target = _newTarget;
    }

    public virtual void FindTarget() {}

    public virtual void Hurt(int hurtAmount) {
        personAnimation.SetParameter("HurtAmount", hurtAmount);
        personAnimation.Replay("Hurt");
    }

    public virtual void Dead() {
        personAnimation.Play("Dead");
    }

    public virtual void Resurrect() {
        personInfo.isDead = false;
        personInfo.stateMachineDead = false;
        personAgent.enabled = true;
        personInfo.AddHealth(personInfo.MaxHealth);
        personInfo.personRagdollManager.DisableRagdoll();
        Idle();
    }
}