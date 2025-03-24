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
        if(personInfo.isDead == false) {
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
    void OnCollisionEnter(Collision otherCollision) {
        if(otherCollision.collider.gameObject.CompareTag("MasterKnightAttackCollider")) {
            if(collision == false && personInfo.isDead == false) {
                collision = true;
                Info attackCharacterInfo = otherCollision.collider.gameObject.GetComponentInParent<Info>();
                personInfo.ReduceHealth(attackCharacterInfo.damage);
            }
        }
    }
    void OnCollisionExit(Collision otherCollision) {
        collision = false;
    }

    public void ChangeState(StateMachine _state) {
        personState = _state;
    }

    public virtual void Dead() {
        ChangeState(StateMachine.Dead);
        personAgent.enabled = false;
        personInfo.personRagdollManager.EnableRagdoll();
    }
}