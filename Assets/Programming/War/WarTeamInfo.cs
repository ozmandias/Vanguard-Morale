using UnityEngine;

[System.Serializable] public class WarTeamInfo {
    public GameObject owner;
    public int warTeamId;
    public int health;
    int MaxHealth = 100;
    public bool isDestroyed = false;

    public void Init(GameObject owner) {
        this.owner = owner;

        health = MaxHealth;
    }

    public void SetWarTeamId(int id) {
        this.warTeamId = id;
    }
}