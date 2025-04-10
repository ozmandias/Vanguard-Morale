using UnityEngine;

public class Friend : Person {
    public override void Start() {

    }

    public override void Idle() {

    }

    public override void Move() {

    }

    public override void Attack() {

    }

    public override void Work() {

    }

    public override void Follow() {

    }

    public override void FindTarget() {
        if(attackingTarget == false) {

        }
    }

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
    }

    public override void Dead() {
        base.Dead();
    }

    public override void Resurrect() {
        base.Resurrect();
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}