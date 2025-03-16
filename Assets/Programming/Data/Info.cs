using UnityEngine;

public class Info : MonoBehaviour {
    public int health;
    public int MaxHealth = 100;
    public int damage;
    public int morality = 50;
    public Morality alignment = Morality.Neutral;
    public bool isDead = false;

    public virtual void Start() {
        health = MaxHealth;
    }

    public virtual void ReduceHealth(int damageAmount) {
        health = health - damageAmount;

        if(health <= 0) {
            isDead = true;
        }
    }
}