using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarTeam : MonoBehaviour {
    public WarModel warModel;
    public WarTeamInfo warTeamInfo = new WarTeamInfo();

    void Start() {
        warTeamInfo.Init(gameObject);

        if(warTeamInfo.warTeamId == 0) {
            GameManager.instance.friendDestination = transform;
        } else if(warTeamInfo.warTeamId == 1) {
            GameManager.instance.enemyDestination = transform;
        }
    }

    void Update() {
        
    }

    public void CreateBase() {
        if(warModel.mainBase != null) {
            for(int i = 0; i < warModel.mainBase.spawnCount; i = i + 1) {
                Instantiate(warModel.mainBase.spawnObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void CreateVanguards() {
        var vanguardCreatePoints = warTeamInfo.warTeamId == 0 ? WarManager.instance.warTeam0VanguardCreatePoints : WarManager.instance.warTeam1VanguardCreatePoints;
        foreach(var vanguardHero in warModel.vanguardHeroes) {
            for(int i = 0; i < vanguardHero.spawnCount; i = i + 1) {
                Instantiate(vanguardHero.spawnObject, vanguardCreatePoints[Random.Range(0, vanguardCreatePoints.Length)], Quaternion.identity);
            }
        }
    }

    public void CreateSoldiers() {
        var soldierCreatePoints = warTeamInfo.warTeamId == 0 ? WarManager.instance.warTeam0SoldierCreatePoints : WarManager.instance.warTeam1SoldierCreatePoints;
        foreach(var meleeSoldier in warModel.meleeSoldiers) {
            for(int i = 0; i < meleeSoldier.spawnCount; i = i + 1) {
                PersonManager.instance.CreatePerson(meleeSoldier.spawnObject, soldierCreatePoints[Random.Range(0, soldierCreatePoints.Length)]);
            }
        }
        /*foreach(var rangeSoldier in warModel.rangeSoldiers) {
            for(int i = 0; i < rangeSoldier.spawnCount; i = i + 1) {
                PersonManager.instance.CreatePerson(rangeSoldier.spawnObject, soldierCreatePoints[Random.Range(0, soldierCreatePoints.Length)]);
            }
        }*/
    }

    public WarTeamInfo GetInfo() {
        return warTeamInfo;
    }
}