using System.Collections;
using System.Collections.Generic;
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
    public GameObject raycastShooter;
    public GameObject []weapons; // use for both attackCollider and raycastShooter
    public bool attackNumberUpdate = false;
    public int attackNumber = 0;

    [Header("Person Settings")]
    public AnimationManager personAnimation;
    public CombatManager personCombat;
    public AIChanger personAI;
    public StateMachineChanger personState;
    public QuestManager personQuest;
    public RagdollManager personRagdoll;
    public NavMeshAgent personAgent;
    public PersonInfo personInfo = new PersonInfo();
    public EffectManager personEffect;
    public bool isHurt = false;
    public bool initDone = false;
    public float personDestroyWaitTime = 30f;
    
    [Header("Animation Settings")]
    public float attackFrames = 0;
    public float hurtFrames = 0;

    Coroutine PersonDestroyCoroutine;

    public virtual void Start()
    {
        var character = GetComponent<Character>();
        if(attackCollider == null && character.combatType == CombatType.Melee) attackCollider = character.personalData.attackColliderObject.GetComponent<Collider>();
        if(raycastShooter == null && character.combatType == CombatType.Range) raycastShooter = character.personalData.raycastShooterObject;
        if(weapons == null && character.personalData.weaponObjects.Length > 0) { // no need to assign weapons if person if fist fighter
            weapons = new GameObject[character.personalData.weaponObjects.Length];
            for(int i = 0; i < weapons.Length; i = i + 1) {
                weapons[i] = character.personalData.weaponObjects[i];
            }
        }

        personAnimation = GetComponent<AnimationManager>();
        personCombat = GetComponent<CombatManager>();
        personAI = GetComponent<AIChanger>();
        personState = GetComponent<StateMachineChanger>();
        personQuest = GetComponent<QuestManager>();
        personRagdoll = GetComponent<RagdollManager>();
        personAgent = GetComponent<NavMeshAgent>();
        personEffect = GetComponent<EffectManager>();

        personInfo.Init(gameObject);

        // when OnCirclingListUnregister.Invoke() is called, this will execute a function() for every Person instance in the scene.
        personCombat.OnCirclingListUnregister.AddListener((navMeshAgent) => CheckCirclingListAndRemove(navMeshAgent));

        initDone = true;
    }

    public virtual void Update()
    {
        if (personAI.aiType == AIType.StateMachine && personState.stateMachineDead == false)
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

            if (target == null && ShouldFindTarget() && personState.stateMachineTargeting == false && personInfo.isDead == false)
            {
                FindTarget();
            }
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
        if((GetInfo() as PersonInfo).combatType == CombatType.Melee) {
            
        } else if((GetInfo() as PersonInfo).combatType == CombatType.Range) {
            GetComponent<EffectManager>().attackEffect.canCreateEffect = true;
        }
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

    public virtual void HurtByOther(CharacterInfo attackerInfo) {
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

    public virtual void DecideToChangeTarget(CharacterInfo newTargetInfo) {
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

    public virtual bool ShouldFindTarget() {
        return true;
    }

    public void ChangeAttackCollider() {
        // check with animator and change attack collider on different animations
        attackCollider = weapons[(int) personAnimation.mainAnimator.GetFloat("AttackNumber")].GetComponent<Weapon>().itemCollider;
    }

    public void ChangeRaycastShooter() {
        // check with animator and change raycast shooter on different animations
        raycastShooter = weapons[(int) personAnimation.mainAnimator.GetFloat("AttackNumber")];
    }

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

                CharacterInfo attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
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

    // remove a NavMeshAgent from circlingList of every Person instance in the scene.
    public void CheckCirclingListAndRemove(NavMeshAgent navMeshAgent) {
        if(personCombat.CirclingListContains(navMeshAgent)) {
            personCombat.circlingList.Remove(navMeshAgent);
        }
    }

    public void StartDestroyCountdown() {
        PersonDestroyCoroutine = StartCoroutine(DestroyCoroutine());
    }

    public void CancelDestroyCountdown() {
        if(PersonDestroyCoroutine != null) StopCoroutine(DestroyCoroutine());
    }

    public IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(personDestroyWaitTime);
        Destroy(this.gameObject);
    }
}