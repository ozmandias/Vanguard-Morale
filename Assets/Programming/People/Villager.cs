using UnityEngine;

public class Villager : Person {
    public override void Start() {
        base.Start();

        GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);
    }

    public override void Idle() {
        base.Idle();
    }

    public override void Move() {
        base.Move();
    }

    public override void Work() {
        base.Work();
    }

    public override void Follow() {
        base.Follow();
    }

    public override void Attack() {
        base.Attack();
    }

    public override void Hurt() {
        base.Hurt();
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.personList.Remove(this);
        AIManager.instance.personAIList.Remove(personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("EnemyAttackCollider")) {
            Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
            if(personInfo.isDead == false && attackCharacterInfo.isDead == false){
                personAnimation.SetParameter("HurtAmount", attackCharacterInfo.damage);
                personAnimation.SetParameter("ReduceHealth", true);
                personAnimation.mainAnimator.GetBehaviour<HurtState>().attackerInfo = attackCharacterInfo;
                if(isHurt == true) {
                    hurtFrames = 0;
                }
                isHurt = true;

                int changeTargetRandom = Random.Range(0, 10);
                if(attackingTarget == false || (attackingTarget && changeTargetRandom >= 5)) {
                    if(target) {
                        CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                        if(currentTargetCombat.CirclingListContains(personAgent)) {
                            currentTargetCombat.circlingList.Remove(personAgent);
                        }
                    }
                    SetTarget(attackCharacterInfo.owner);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}