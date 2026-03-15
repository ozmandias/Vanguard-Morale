using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarManager : MonoBehaviour {
    void Start() {
        GetFactions();
    }

    void Update() {
        CheckWar();
    }

    void GetFactions() {
        CreateSoldiers();
    }

    void CreateSoldiers() {
        StartCoroutine(CreateSoldiersCoroutine());
    }

    void CheckWar() {
        
    }

    IEnumerator CreateSoldiersCoroutine() {
        yield return null;
    }
}