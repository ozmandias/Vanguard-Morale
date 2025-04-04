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

    }

    public override void Work() {

    }

    public override void Follow() {
        
    }

    public override void Hurt(int hurtAmount) {
        // base.Hurt(hurtAmount);
    }

    public override void Dead() {
        // base.Dead();

        GameManager.instance.personList.Remove(this);
    }
}