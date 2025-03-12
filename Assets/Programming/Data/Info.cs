using UnityEngine;

public class Info : MonoBehaviour {
    int health;
    float morality = 0;
    [SerializeField] int MaxHealth = 100;

    void Start() {
        health = MaxHealth;
    }
}