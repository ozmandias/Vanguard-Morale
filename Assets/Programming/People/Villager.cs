using UnityEngine;

public class Villager : Person {
    public override void Start() {
        base.Start();

        GameManager.instance.personList.Add(this);
    }

    public override void Idle() {
        personAnimation.Play("Idle");
    }

    public override void Move() {
        personAnimation.Play("Move");
    }

    public override void Attack() {
        personAnimation.Play("Attack");
    }

    public override void Work() {

    }

    public override void Follow() {
        personAnimation.Play("Follow");
    }

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.personList.Remove(this);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.personList.Add(this);
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("EnemyAttackCollider")) {
            if(Time.time > nextHurtTime && personInfo.isDead == false){
                nextHurtTime = Time.time + hitRate;

                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Info>();
                Hurt(attackCharacterInfo.damage);
                if(attackingTarget == false) {
                    SetTarget(attackCharacterInfo.gameObject);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}