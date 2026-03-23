using UnityEngine;

[System.Serializable] public class PlayerInfo : Info {
    Player player;

    public /*PlayerInfo() : base()*/ void Init(GameObject owner)
    {
        base.Init(owner);
        strength = 50;
        agility = 50;
        intelligence = 50;
        damage = 50 + (int) Mathf.Round(strength / 10);
        magic = 20;
        morality = 60;
        alignment = (Morality)morality;

        player = owner.GetComponent<Player>();

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

    public Player GetPlayer() {
        return player;
    }
}