using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    GameObject []playerPrefabs;

    void Start() {
        GetPlayerChoice();
    }

    void Update() {

    }

    void GetPlayerChoice() {
        var playerChoice = GlobalData.characterDetails;
        GameObject playerChoicePrefab = Array.Find(playerPrefabs, (playerPrefab) => {
            return playerPrefab.name == playerChoice.character.ToString();
        });
        CreatePlayer(playerChoicePrefab);
    }

    void CreatePlayer(GameObject playerPrefab) {
        var createPoint = GameObject.Find("GameStartPoint");
        GameObject player = Instantiate(playerPrefab, createPoint.transform.position, Quaternion.identity);
    }
}