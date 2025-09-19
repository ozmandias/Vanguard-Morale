using UnityEngine;

public class Boss : Person
{
    public delegate void SpecialAbilityDelegate();
    public SpecialAbilityDelegate OnSpecialAbility;

    public override void Start() {
        base.Start();

        OnSpecialAbility += SpecialAbility;
    }

    public override void Idle() {
        base.Idle();
    }

    public override void Move() {
        base.Move();
    }

    public override void Attack() {
        base.Attack();
    }

    public virtual void SpecialAbility() {}

    public override void FindTarget() {
        Debug.Log("FindTarget");
        target = GameManager.instance.playerGameObject;
        if(target) {
            Debug.Log("Boss found Player to attack!");
        }
    }
}