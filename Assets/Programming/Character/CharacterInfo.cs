using UnityEngine;

[System.Serializable] public class CharacterInfo {
    public GameObject owner;
    public int strength;
    public int agility;
    public int intelligence;
    public int health;
    public int MaxHealth = 100;
    public int damage;
    public int magic;
    public int morality = 50;
    public Morality alignment = Morality.Neutral;
    public Gender gender = Gender.Male;
    public CombatType combatType = CombatType.Melee;
    public bool isDead = false;

    public /*Info()*/ void Init(GameObject owner)
    {
        this.owner = owner;
        health = MaxHealth;
    }

    public virtual void AddHealth(int healthAmount) {
        health += healthAmount;

        if(health > MaxHealth) {
            health = MaxHealth;
        }
    }

    public virtual void ReduceHealth(int damageAmount) {
        health = health - damageAmount;

        if(health <= 0) {
            health = 0;
            isDead = true;
        }
    }

    public virtual void AddMorality(int moralityAmount) {
        morality = morality + moralityAmount;
    }

    public virtual void ReduceMorality(int moralityAmount) {
        morality = morality - moralityAmount;
    }
}