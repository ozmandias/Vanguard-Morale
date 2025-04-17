using UnityEngine;

public class Friend : Person {
    public override void Start() {
        base.Start();

        personDestination = GameManager.instance.soldierDestination /*GameObject.Find("SoldierDestination").transform*/;

        GameManager.instance.soldierList.Add(this);
        AIManager.instance.soldierAIList.Add(personAgent);
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

    public override void FindTarget() {
        float targetEnemyDistance = 0;
        GameObject targetEnemy = null;

        foreach(Enemy enemy in GameManager.instance.enemyList) {
            CombatManager enemyCombat = enemy.GetComponent<CombatManager>();
            if(enemy.personInfo.isDead == false && enemyCombat.IsCirclingListFull() == false) {
                targetEnemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
                if(targetEnemyDistance <= 300f) {
                    targetEnemy = enemy.gameObject;
                    break;
                }
            }
        }

        if(targetEnemyDistance != 0) {
            SetTarget(targetEnemy);
        }
    }

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.soldierList.Remove(this);
        AIManager.instance.soldierAIList.Remove(personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.soldierList.Add(this);
        AIManager.instance.soldierAIList.Add(personAgent);
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("EnemyAttackCollider")) {
            if(personInfo.isDead == false) {
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