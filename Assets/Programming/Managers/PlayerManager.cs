using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public string playerCodeName;
    public GameObject playerGameObject;
    public GameObject []playerPrefabs;
    public Character currentCharacter;
    public Vector3 []playerCreatePoints; // add manually
    public bool autoCreatePlayer = false;

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
        if(string.IsNullOrEmpty(playerCodeName)) playerCodeName = playerChoice.codeName;
        GameObject playerChoicePrefab = Array.Find(playerPrefabs, (playerPrefab) => {
            return playerPrefab.name == playerCodeName; 
        });
        if(playerCreatePoints.Length == 0) {
            playerCreatePoints = new Vector3[1];
            playerCreatePoints[0] = Vector3.zero;
        }
        if(playerChoicePrefab != null && autoCreatePlayer) {
            CreatePlayer(playerChoicePrefab, playerCreatePoints[UnityEngine.Random.Range(0, playerCreatePoints.Length)]);
        }
    }

    void CreatePlayer(GameObject playerPrefab, Vector3 playerCreatePoint) {
        playerGameObject = Instantiate(playerPrefab, playerCreatePoint, Quaternion.identity);
        currentCharacter = playerGameObject.GetComponent<Character>();

        // set player tag
        // set morality alignment
        // add player component, player animator and other components
        // player - Vanguard/Player, AnimationManager, CombatManager, EffectManager, RagdollManager, QuestManager(optional)
        if(currentCharacter.playerCharacter == PlayerCharacter.Vanguard) {
            playerGameObject.tag = "Player";
            var vanguard = playerGameObject.AddComponent<Vanguard>();
            currentCharacter.personalData.attackColliderObject.tag = "VanguardAttackCollider";
            vanguard.GetInfo().alignment = currentCharacter.morality;
        } else {
            playerGameObject.tag = "Player";
            var player = playerGameObject.AddComponent<Player>();
            currentCharacter.personalData.attackColliderObject.tag = "PlayerAttackCollider";
            player.GetInfo().alignment = currentCharacter.morality;
        }
        playerGameObject.AddComponent<AnimationManager>();
        /*if Vanguard*/ playerGameObject.AddComponent<CombatManager>();
        playerGameObject.AddComponent<EffectManager>();
        playerGameObject.AddComponent<RagdollManager>();

        // GameManager.instance.InitPlayer(currentCharacter.playerCharacter, playerGameObject);
        if(GameManager.instance.OnPlayerReady != null) GameManager.instance.OnPlayerReady.Invoke(currentCharacter.playerCharacter, playerGameObject);
    }
}