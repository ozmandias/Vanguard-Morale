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

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
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
                Hurt(attackCharacterInfo.damage);
                int attackBackRandom = Random.Range(0, 10);
                if(attackingTarget == false || attackBackRandom >= 5) {
                    SetTarget(attackCharacterInfo.gameObject);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}