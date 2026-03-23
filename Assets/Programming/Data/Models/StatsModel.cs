using UnityEngine;

[System.Serializable] public class StatsModel {
    public int strength;
    public int agility;
    public int intelligence;
    public int health;
    public int damage;
    public int magic;
    public int morality;

    public StatsModel() {}

    public StatsModel(
        int strength,
        int agility,
        int intelligence,
        int health,
        int damage,
        int magic,
        int morality
    ) {
        this.strength = strength;
        this.agility = agility;
        this.intelligence = intelligence;
        this.health = health;
        this.damage = damage;
        this.magic = magic;
        this.morality = morality;
    }
}