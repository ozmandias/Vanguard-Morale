using UnityEngine;

public class EnemyGuardInfo : Info {
    RagdollManager enemyGuardRagdollManager;

    public override void Start() {
        base.Start();

        damage = 10;
        morality = 40;
        alignment = (Morality) morality;

        enemyGuardRagdollManager = GetComponent<RagdollManager>();
    }

    public override void ReduceHealth(int damageAmount) {
        base.ReduceHealth(damageAmount);

        if(isDead == true) {
            enemyGuardRagdollManager.EnableRagdoll();
        }
    }
}