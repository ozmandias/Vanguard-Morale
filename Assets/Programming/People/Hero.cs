using UnityEngine;

public class Hero : Player {
    public override void Start() {
        base.Start();

        attackCollider = GameObject.Find("HeroAttackCollider").GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public override void Update() {
        base.Update();
    }
}