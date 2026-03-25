using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public string playerChoiceCharacter;
    public GameObject playerGameObject;
    public GameObject []playerPrefabs;
    public PersonalData playerPersonalData;

    public static PlayerManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        GetPlayerChoice();
    }

    void Update() {

    }

    void GetPlayerChoice() {
        var playerChoice = GlobalData.characterDetails;
        if(string.IsNullOrEmpty(playerChoiceCharacter)) playerChoiceCharacter = playerChoice.character.ToString();
        GameObject playerChoicePrefab = Array.Find(playerPrefabs, (playerPrefab) => {
            return playerPrefab.name == playerChoiceCharacter; 
        });
        if(playerChoicePrefab != null) CreatePlayer(playerChoicePrefab);
    }

    void CreatePlayer(GameObject playerPrefab) {
        var createPoint = GameObject.Find("GameStartPoint");
        playerGameObject = Instantiate(playerPrefab, createPoint != null ? createPoint.transform.position : Vector3.zero, Quaternion.identity);
        playerPersonalData = playerGameObject.GetComponent<PersonalData>();

        // set player tag
        playerGameObject.tag = "Player";
        // add player component, player animator and other components
        // player - Vanguard/Player, AnimationManager, CombatManager, EffectManager, RagdollManager, QuestManager(optional)
        playerGameObject.AddComponent<Player>();
        playerGameObject.AddComponent<AnimationManager>();
        /*if Vanguard*/ playerGameObject.AddComponent<CombatManager>();
        playerGameObject.AddComponent<EffectManager>();
        playerGameObject.AddComponent<RagdollManager>();

        // person - Person, AnimationManager, CombatManager, EffectManager, RagdollManager, QuestManager(optional), AIChanger, StateMachineChanger, NavMeshAgent
    }
}