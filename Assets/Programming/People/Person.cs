using UnityEngine;
using UnityEngine.AI;

public abstract class Person : MonoBehaviour {
    public float speed = 20f;
    public AnimationManager personAnimation;
    public NavMeshAgent personAgent;
    public PersonInfo personInfo;
    public Transform personDestination;
    public GameObject personTarget;
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
        }
    }

    public abstract void Idle();
    public abstract void Move();
    public abstract void Attack();
    public abstract void Work();
    public abstract void Follow();

    bool collision = false;
    float collisionTimer = 0;
    void OnCollisionEnter(Collision otherCollision) {
        if(otherCollision.collider.gameObject.CompareTag("MasterKnightAttackCollider") || otherCollision.collider.gameObject.CompareTag("PlayerAttackCollider")) {
            collisionTimer += Time.deltaTime;
            if(collision == false && personInfo.isDead == false && collisionTimer > 0.01f) {
                collision = true;
                collisionTimer = 0;
                Info attackCharacterInfo = otherCollision.collider.gameObject.GetComponentInParent<Info>();
                personInfo.ReduceHealth(attackCharacterInfo.damage);
                Hurt();
            }
        }
    }
    void OnCollisionExit(Collision otherCollision) {
        collision = false;
    }

    public void ChangeState(StateMachine _state) {
        personState = _state;
    }

    public virtual void Hurt() {
        personAnimation.Play("Hurt");
    }

    public virtual void Dead() {
        personAnimation.Play("Dead");
    }
}