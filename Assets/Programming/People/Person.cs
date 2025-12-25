using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {
    [Header("Move Settings")]
    public float speed = 20f;
    public Transform destination;

    [Header("Attack Settings")]
    public bool isAttacking = false;
    public GameObject target;
    public Collider attackCollider;
    public GameObject weapon;
    public bool attackNumberUpdate = false;
    public int attackNumber = 0;

    [Header("Person Settings")]
    public AnimationManager personAnimation;
    public CombatManager personCombat;
    public StateMachineManager personState;
    public QuestManager personQuest;
    public NavMeshAgent personAgent;
    public NavMeshHit personNavMeshHit;
    public PersonInfo personInfo;
    public EffectManager personEffect;
    public bool isHurt = false;

    [Header("Animation Settings")]
    public float attackFrames = 0;
    public float hurtFrames = 0;

    public virtual void Start()
    {
        personAnimation = GetComponent<AnimationManager>();
        personCombat = GetComponent<CombatManager>();
        personState = GetComponent<StateMachineManager>();
        personQuest = GetComponent<QuestManager>();
        personAgent = GetComponent<NavMeshAgent>();
        personEffect = GetComponent<EffectManager>();

        personInfo.Init(gameObject);
    }

    public virtual void Update()
    {
        if (personInfo.aiType == AIType.StateMachine && personInfo.stateMachineDead == false)
        {
            switch (personState.state)
            {
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
                case StateMachine.Hurt:
                    Hurt();
                    break;
                case StateMachine.Dead:
                    Dead();
                    break;
                default:
                    break;
            }

            if (personInfo.personType != PersonType.Normal && personState.stateMachineTargeting == false && personInfo.isDead == false)
            {
                FindTarget();
            }
        }

        if(personInfo.aiType == AIType.QuestAI && personInfo.stateMachineDead == false) {

        }

        if (personInfo.isDead == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Resurrect();
            }
        }
    }

    public virtual void Idle()
    {
        personAnimation.Play("Idle");
    }

    public virtual void Move()
    {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Move");
    }

    public virtual void Work()
    {
        personAnimation.Play("Work");
    }

    public virtual void Follow()
    {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Follow");
    }

    public virtual void Attack()
    {
        personAnimation.Play("Attack");
    }

    public virtual void Hurt()
    {
        hurtFrames += Time.deltaTime;
        if (hurtFrames < 1)
        {
            personAnimation.PlayByFrame("Hurt", hurtFrames);
        }
    }

    public virtual void Dead()
    {
        personAnimation.Play("Dead");
    }

    public virtual void Resurrect()
    {
        personInfo.isDead = false;
        personInfo.MakeLife("alive");
    }

    public virtual void HurtByOther(Info attackerInfo) {
        if(personInfo.isDead == false && Time.time > nextHurtTime) {
            nextHurtTime = Time.time + hitRate;
            personAnimation.SetParameter("HurtAmount", attackerInfo.damage);
            personAnimation.SetParameter("ReduceHealth", true);
            personAnimation.mainAnimator.GetBehaviour<HurtState>().attackerInfo = attackerInfo;
            hurtFrames = 0;
            isHurt = true;

            DecideToChangeTarget(attackerInfo);
        }
    }

    public virtual void DecideToChangeTarget(Info newTargetInfo) {
        int changeTargetRandom = Random.Range(0, 10);
        if(personState.stateMachineTargeting == false || (personState.stateMachineTargeting && changeTargetRandom >= 5)) {
            if(target) {
                CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                if(currentTargetCombat.CirclingListContains(personAgent)) {
                    currentTargetCombat.circlingList.Remove(personAgent);
                }
            }
            SetTarget(newTargetInfo.owner);
        }
    }

    public virtual void SetTarget(GameObject _newTarget)
    {
        target = _newTarget;
    }

    public virtual void FindTarget() { }

    bool collision = false;
    float nextHurtTime = 0;
    float hitRate = 1f;
    public virtual void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("VanguardAttackCollider") || otherCollider.gameObject.CompareTag("PlayerAttackCollider"))
        {
            if (/*collision == false && Time.time > nextHurtTime &&*/ personInfo.isDead == false)
            {
                /*collision = true;
                nextHurtTime = Time.time + hitRate;*/

                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
                personAnimation.SetParameter("HurtAmount", attackCharacterInfo.damage);
                personAnimation.SetParameter("ReduceHealth", true);
                personAnimation.mainAnimator.GetBehaviour<HurtState>().attackerInfo = attackCharacterInfo;
                hurtFrames = 0;
                isHurt = true;

                int changeTargetRandom = Random.Range(0, 10);
                if ((personState.stateMachineTargeting == false || (personState.stateMachineTargeting && changeTargetRandom >= 5)) && personInfo.personType != PersonType.Friend)
                {
                    if(target) {
                        CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                        if (currentTargetCombat.CirclingListContains(personAgent))
                        {
                            currentTargetCombat.circlingList.Remove(personAgent);
                        }
                    }
                    SetTarget(GameManager.instance.playerGameObject);
                }
            }
        }
    }
    public virtual void OnTriggerExit(Collider otherCollider)
    {
        // collision = false;
    }

    public PersonInfo GetInfo()
    {
        return personInfo;
    }
}