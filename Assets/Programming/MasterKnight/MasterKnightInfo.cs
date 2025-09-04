using UnityEngine;

[System.Serializable] public class MasterKnightInfo : Info {
    public /*MasterKnightInfo() : base()*/ void Init(GameObject owner)
    {
        base.Init(owner);
        damage = 50;
        morality = 60;
        alignment = (Morality)morality;

        if (PlayerProfileController.instance.OnHealthChanges != null)
        {
            PlayerProfileController.instance.OnHealthChanges(health);
        }

        if (PlayerProfileController.instance.OnDamageChanges != null)
        {
            PlayerProfileController.instance.OnDamageChanges(damage);
        }

        if (PlayerProfileController.instance.OnMoralityChanges != null)
        {
            PlayerProfileController.instance.OnMoralityChanges(alignment);
        }
    }

    public override void ReduceHealth(int damageAmount)
    {
        base.ReduceHealth(damageAmount);
        if(PlayerProfileController.instance.OnHealthChanges != null) {
            PlayerProfileController.instance.OnHealthChanges(health);
        }
    }
}