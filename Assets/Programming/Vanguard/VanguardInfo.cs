using UnityEngine;

[System.Serializable] public class VanguardInfo : CharacterInfo {
    Vanguard vanguard;
    public PlayerType playerType = PlayerType.Single;

    public /*VanguardInfo() : base()*/ void Init(GameObject owner)
    {
        base.Init(owner);
        strength = 70;
        agility = 50;
        intelligence = 50;
        damage = 50 + (int) Mathf.Round(strength / 10);
        magic = 30;
        morality = 60;
        alignment = (Morality)morality;

        vanguard = owner.GetComponent<Vanguard>();

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

    public Vanguard GetVanguard() {
        return vanguard;
    }
}