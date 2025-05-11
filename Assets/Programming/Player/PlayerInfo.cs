using UnityEngine;

public class PlayerInfo : Info {
    public override void Start() {
        base.Start();
        damage = 50;
        morality = 60;
        alignment = (Morality) morality;
    }

    public override void Update() {
        PlayerProfileController.instance.SetHealth(health);
    }

    public override void ReduceHealth(int damageAmount) {
        base.ReduceHealth(damageAmount);
    }
}