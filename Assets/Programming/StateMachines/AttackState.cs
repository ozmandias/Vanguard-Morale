using UnityEngine;

public class AttackState : StateMachineBehaviour {
    Person mainPerson;
    CharacterInfo targetInfo;
    CombatManager targetCombat;
    float targetDistance;
    [SerializeField] float attackStateTime = 0;

    public float followDistance = 250f;
    public float attackDistance = 10f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.personAgent.isStopped = true;

        attackDistance = mainPerson.GetInfo().combatType == CombatType.Melee ? 10f : 50f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? (CharacterInfo) mainPerson.target.GetComponent<Vanguard>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Player>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).person.personAI.aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }

            targetCombat = mainPerson.target.GetComponent<CombatManager>();
            if(canAttackTarget) {
                Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
                targetDirection.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);

                if(animator.GetBool("Attacking") == true) {
                    mainPerson.isAttacking = true;
                    
                    if((mainPerson.GetInfo() as PersonInfo).combatType == CombatType.Melee) {
                        mainPerson.attackCollider.enabled = true;
                        // Play Effect
                    } else {
                        string raycastShooterName = "RaycastShooter" + animator.GetFloat("AttackNumber");
                        GameObject raycastShooter = GameHelpers.FindGameObjectInChildren(raycastShooterName, animator.transform.gameObject);
                        
                        RaycastHit rangeRaycastHit;
                        // LayerMask rangeLayerMask = layerMask.GetMask("");
                        if(Physics.Raycast(raycastShooter.transform.position /*animator.transform.position + Vector3.up * 8f*/, animator.transform.forward, out rangeRaycastHit, 60f /*, rangeLayerMask*/)) {
                            // Debug.DrawRay(raycastShooter.transform.position /*animator.transform.position + Vector3.up * 8f*/, animator.transform.TransformDirection(Vector3.forward) * 60f, Color.white);
                            if(rangeRaycastHit.collider.gameObject.CompareTag("Player")) {
                            } else if(rangeRaycastHit.collider.gameObject.CompareTag("Person")) {
                            }
                        }

                        if(mainPerson.personEffect.attackEffect.effectType == EffectType.Spawn && mainPerson.personEffect.attackEffect.canManageEffect) {
                            GameObject newAttackEffect = mainPerson.personEffect.CreateEffect("attack", raycastShooter.transform.position, Quaternion.LookRotation(mainPerson.target.transform.position - animator.transform.position, Vector3.up) /*animator.transform.rotation*/);
                            if(newAttackEffect) {
                                mainPerson.personEffect.DestroyEffect(newAttackEffect);
                            }
                            mainPerson.personEffect.attackEffect.canManageEffect = false;
                        }
                    }
                } else {
                    mainPerson.isAttacking = false;

                    if((mainPerson.GetInfo() as PersonInfo).combatType == CombatType.Melee) {
                        mainPerson.attackCollider.enabled = false;
                    } else {
                        // do range attack
                    }

                    attackStateTime = 0;
                }

                if(mainPerson.isAttacking == true) {
                    attackStateTime += Time.deltaTime /*stateInfo.normalizedTime % 1*/;
                }

                targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                if(targetDistance > attackDistance && targetDistance <= followDistance && targetInfo.isDead == false) {
                    mainPerson.personState.stateMachineAttacking = false;
                }
                else if(targetDistance > followDistance || targetInfo.isDead == true) {
                    if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        targetCombat.circlingList.Remove(mainPerson.personAgent);
                    }
                    mainPerson.SetTarget(null);
                    mainPerson.personState.stateMachineAttacking = false;
                    mainPerson.personState.stateMachineTargeting = false;
                }
            } else {
                if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                    targetCombat.circlingList.Remove(mainPerson.personAgent);
                }
                mainPerson.SetTarget(null);
                mainPerson.personState.stateMachineAttacking = false;
                mainPerson.personState.stateMachineTargeting = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("Attacking", false);
        mainPerson.attackNumberUpdate = false;
        mainPerson.isAttacking = false;
        if((mainPerson.GetInfo() as PersonInfo).combatType == CombatType.Melee)
            mainPerson.attackCollider.enabled = false;
        attackStateTime = 0;
    }
}