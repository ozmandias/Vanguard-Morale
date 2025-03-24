using UnityEngine;
using UnityEngine.AI;

public class Enemy : Person {
    public override void Start() {
        base.Start();
        personTarget = GameManager.instance.playerGameObject;
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

    public override void Dead() {
        base.Dead();
    }
}