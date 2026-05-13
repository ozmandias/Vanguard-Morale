using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonManager : MonoBehaviour {
    public Vector3 []personCreatePoints; // add manually

    public static PersonManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {

    }

    void Update() {

    }

    public GameObject CreatePerson(GameObject personPrefab, Vector3 personCreatePoint) {
        GameObject personObject = Instantiate(personPrefab, personCreatePoint, Quaternion.identity);
        Character character = personObject.GetComponent<Character>();

        // set person tag
        // set morality alignment and person type (determine reputation with player)
        // determine Citizen, Soldier, Leader or Vendor to add to person
        // person - Person, AnimationManager, CombatManager, EffectManager, RagdollManager, QuestManager(optional), AIChanger, StateMachineChanger, NavMeshAgent
        switch(character.personCharacter) {
            case PersonCharacter.Citizen:
                personObject.tag = "Citizen";
                var citizen = personObject.AddComponent<Citizen>();
                character.personalData.attackColliderObject.tag = "CitizenAttackCollider";
                citizen.GetInfo().alignment = character.morality;
                AssignPersonType(personObject);
                break;
            case PersonCharacter.Soldier:
                personObject.tag = "Soldier";
                var soldier = personObject.AddComponent<Soldier>();
                character.personalData.attackColliderObject.tag = "SoldierAttackCollider";
                soldier.GetInfo().alignment = character.morality;
                AssignPersonType(personObject);
                break;
            case PersonCharacter.Leader:
                personObject.tag = "Leader";
                var leader = personObject.AddComponent<Leader>();
                // character.personalData.attackColliderObject.tag = "LeaderAttackCollider";
                leader.GetInfo().alignment = character.morality;
                AssignPersonType(personObject);
                break;
            case PersonCharacter.Vendor:
                var vendor = personObject.AddComponent<Vendor>();
                break;
            default:
                break;
        }
        personObject.AddComponent<AnimationManager>();
        personObject.AddComponent<CombatManager>();
        personObject.AddComponent<EffectManager>();
        personObject.AddComponent<RagdollManager>();
        personObject.AddComponent<QuestManager>();
        personObject.AddComponent<AIChanger>();
        personObject.AddComponent<StateMachineChanger>();
        personObject.GetComponent<NavMeshAgent>().enabled = true;

        return personObject;
    }

    void AssignPersonType(GameObject personObject) {
        switch(ReputationManager.instance.GetTwoFactionsReputation(
            personObject.GetComponent<Character>().faction,
            GlobalData.characterDetails.faction
        )) {
            case Reputation.Friendly:
                personObject.GetComponent<Person>().GetInfo().personType = PersonType.Friend;
                break;
            case Reputation.Neutral:
                personObject.GetComponent<Person>().GetInfo().personType = PersonType.Normal;
                break;
            case Reputation.Hostile:
                personObject.GetComponent<Person>().GetInfo().personType = PersonType.Enemy;
                break;
            default:
                break;
        }
    }
}