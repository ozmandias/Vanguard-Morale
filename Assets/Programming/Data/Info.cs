using UnityEngine;

public class Info : MonoBehaviour {
    public int health;
    public int MaxHealth = 100;
    public int damage;
    public int morality = 50;
    public Morality alignment = Morality.Neutral;
    public Gender gender = Gender.Male;
    public bool isDead = false;

    public virtual void Start() {
        health = MaxHealth;
    }

    public virtual void AddHealth(int healthAmount) {
        health += healthAmount;

        if(health > MaxHealth) {
            health = MaxHealth;
        }
    }

    public virtual void ReduceHealth(int damageAmount) {
        Debug.Log("ReduceHealth");
        health = health - damageAmount;

        if(health <= 0) {
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