using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ModelMapper {
    public static WarModel FactionModelToWarModel(FactionModel factionModel) {
        var warModel = new WarModel();
        
        /*GameObject []vanguards = new GameObject[factionModel.vanguardSpawnModels.Length];
        for(int i = 0; i < vanguards.Length; i = i + 1) {
            vanguards[i] = factionModel.vanguardSpawnModels[i].spawnObject;
        }
        warModel.vanguardHeroes = vanguards;*/
        warModel.vanguardHeroes = factionModel.vanguardSpawnModels;

        var meleeSoldierSpawnModels = Array.FindAll(factionModel.soldierSpawnModels, (spawnModel) => {
            var spawnModelCharacter = spawnModel.spawnObject.GetComponent<Character>();
            return spawnModelCharacter.combatType == CombatType.Melee;
        });
        /*GameObject []meleeSoldiers = meleeSoldierSpawnModels.Select((soldierSpawnDetails) => {
            return soldierSpawnDetails.spawnObject;
        }).ToArray();
        warModel.meleeSoldiers = meleeSoldiers;*/
        warModel.meleeSoldiers = meleeSoldierSpawnModels;

        var rangeSoliderSpawnModels = Array.FindAll(factionModel.soldierSpawnModels, (spawnModel) => {
            var spawnModelCharacter = spawnModel.spawnObject.GetComponent<Character>();
            return spawnModelCharacter.combatType == CombatType.Range;
        });
        /*GameObject []rangeSoldiers = rangeSoliderSpawnModels.Select((soldeirsSpawnDetails) => {
            return soldeirsSpawnDetails.spawnObject;
        }).ToArray();
        warModel.rangeSoldiers = rangeSoldiers;*/
        warModel.rangeSoldiers = rangeSoliderSpawnModels;

        warModel.faction = factionModel.faction;

        return warModel;
    }
}