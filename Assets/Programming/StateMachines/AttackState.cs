using UnityEngine;

public class AttackState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    CombatManager targetCombat;
    float targetDistance;
    [SerializeField] float stateTime = 0;

    public float followDistance = 250f;
    public float nearDistance = 10f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.personAgent.isStopped = true;

        nearDistance = mainPerson.GetInfo().combatType == CombatType.Melee ? 10f : 50f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? (Info) mainPerson.target.GetComponent<MasterKnight>().GetInfo() : (Info) mainPerson.target.GetComponent<Player>().GetInfo() : (Info) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }

            if(canAttackTarget) {
                targetCombat = mainPerson.target.GetComponent<CombatManager>();

                Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
                targetDirection.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);

                if(animator.GetBool("Attacking") == true) {
                    mainPerson.isAttacking = true;
                    
                    if((mainPerson.GetInfo() as PersonInfo).combatType == CombatType.Melee) {
                        mainPerson.attackCollider.enabled = true;   
                    } else {
                        string raycastShooterName = "RaycastShooter" + animator.GetFloat("AttackNumber");
                        GameObject raycastShooter = GameHelpers.FindGameObjectInChildren(raycastShooterName, animator.transform.gameObject);
                        // LayerMask rangeLayerMask = layerMask.GetMask("");
                        
                        RaycastHit rangeRaycastHit;
                        if(Physics.Raycast(raycastShooter.transform.position /*animator.transform.position + Vector3.up * 8f*/, animator.transform.forward, out rangeRaycastHit, 60f /*, rangeLayerMask*/)) {
                            // Debug.DrawRay(raycastShooter.transform.position /*animator.transform.position + Vector3.up * 8f*/, animator.transform.TransformDirection(Vector3.forward) * 60f, Color.white);

                            if(rangeRaycastHit.collider.gameObject.CompareTag("Player")) {

                            } else if(rangeRaycastHit.collider.gameObject.CompareTag("Person")) {

                            }
                        }

                        if(mainPerson.personEffect.attackEffect.effectType == EffectType.Spawn && mainPerson.personEffect.attackEffect.canManageEffect) {
                            GameObject newAttackEffect = mainPerson.personEffect.attackEffect.Create(raycastShooter.transform.position, animator.transform.rotation);
                            if(newAttackEffect) {
                                newAttackEffect.GetComponent<Effect>().SetOwner(mainPerson.gameObject);
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

                    }

                    stateTime = 0;
                }

                if(mainPerson.isAttacking == true) {
                    stateTime += Time.deltaTime /*stateInfo.normalizedTime % 1*/;
                    targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                    if(targetDistance > nearDistance && targetInfo.isDead == false) {
                        mainPerson.atAttackDistance = false;
                    } else if(targetDistance < nearDistance && targetInfo.isDead == false) {
                        Debug.Log(mainPerson.gameObject.name + " is getting too close to target");
                    } else if(targetDistance > followDistance || targetInfo.isDead == true) {
                        if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                            targetCombat.circlingList.Remove(mainPerson.personAgent);
                        }
                        mainPerson.SetTarget(null);
                        mainPerson.atAttackDistance = false;
                        mainPerson.attackingTarget = false;
                        if(mainPerson.GetInfo().personType == PersonType.Boss) Debug.Log("attackingTarget is false by distance");
                    }
                }
            } else {
                mainPerson.attackingTarget = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("Attacking", false);
        mainPerson.attackNumberUpdate = false;
        mainPerson.isAttacking = false;
        if((mainPerson.GetInfo() as PersonInfo).combatType == CombatType.Melee)
            mainPerson.attackCollider.enabled = false;
        stateTime = 0;
    }
}