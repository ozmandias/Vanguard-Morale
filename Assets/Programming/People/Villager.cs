using UnityEngine;

public class Villager : Person {
    public override void Start() {
        base.Start();

        GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);
    }

    public override void Idle() {
        personAnimation.Play("Idle");
    }

    public override void Move() {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Move");
    }

    public override void Work() {
        personAnimation.Play("Work");
    }

    public override void Follow() {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Follow");
    }

    public override void Attack() {
        personAnimation.Play("Attack");
    }

    public override void Wait() {
        personAnimation.Play("Wait");
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
            if(personInfo.isDead == false){
                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Info>();
                personAnimation.SetParameter("HurtAmount", attackCharacterInfo.damage);
                personAnimation.SetParameter("ReduceHealth", true);
                if(isHurt == true) {
                    hurtFrames = 0;
                }
                isHurt = true;

                int attackBackRandom = Random.Range(0, 10);
                if(attackingTarget == false || (attackingTarget && attackBackRandom >= 5)) {
                    if(attackingTarget && attackBackRandom >= 5) {
                        CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                        if(currentTargetCombat.CirclingListContains(personAgent)) {
                            currentTargetCombat.circlingList.Remove(personAgent);
                        }
                    }
                    SetTarget(attackCharacterInfo.gameObject);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}