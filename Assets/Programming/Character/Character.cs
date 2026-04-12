using UnityEngine;

public class Character : MonoBehaviour {
    public float speed = 30f;
    public float jumpForce = 60f;
    public float gravity = 9.81f;
    public PersonalData personalData;
    public PlayerCharacter playerCharacter;
    public PersonCharacter personCharacter;
    public Gender gender;
    public Morality morality = Morality.Neutral;
    public CombatType combatType;
    public Faction faction;
}