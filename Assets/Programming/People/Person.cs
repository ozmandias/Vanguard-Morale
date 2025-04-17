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
    public CombatManager personCombat;
    public NavMeshAgent personAgent;
    public PersonInfo personInfo;
    public Transform personDestination;
    public StateMachine personState = StateMachine.Idle;

    public virtual void Start() {
        personAnimation = GetComponent<AnimationManager>();
        personCombat = GetComponent<CombatManager>();
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
                case StateMachine.Work:
                    Work();
                    break;
                case StateMachine.Follow:
                    Follow();
                    break;
                case StateMachine.Attack:
                    Attack();
                    break;
                case StateMachine.Wait:
                    Wait();
                    break;
                case StateMachine.Dead:
                    Dead();
                    break;
                default:
                    break;
            }
            
            if(personInfo.personType != PersonType.Neutral && attackingTarget == false) {
                FindTarget();
                /*SetTarget(GameManager.instance.personList[0].gameObject);
                AIManager.instance.AgentCircleTarget(personInfo.personType, personAgent, target.transform);*/
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
    public abstract void Work();
    public abstract void Follow();
    public abstract void Attack();
    public abstract void Wait();

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

    bool collision = false;
    public float nextHurtTime = 0;
    public float hitRate = 1f;
    public virtual void OnTriggerEnter(Collider otherCollider) {
        if(otherCollider.gameObject.CompareTag("MasterKnightAttackCollider") || otherCollider.gameObject.CompareTag("PlayerAttackCollider")) {
            if(/*collision == false && Time.time > nextHurtTime &&*/ personInfo.isDead == false) {
                /*collision = true;
                nextHurtTime = Time.time + hitRate;*/

                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Info>();
                Hurt(attackCharacterInfo.damage);
                int attackBackRandom = Random.Range(0, 10);
                if(personInfo.personType == PersonType.Neutral && attackingTarget == false && attackBackRandom >= 5) {
                    SetTarget(GameManager.instance.playerGameObject);
                }
            }
        }
    }
    public virtual void OnTriggerExit(Collider otherCollider) {
        // collision = false;
    }
}