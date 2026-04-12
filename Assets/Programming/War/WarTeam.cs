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
            Instantiate(warModel.mainBase, transform.position, Quaternion.identity);
        }
    }

    public void CreateVanguards() {
        var vanguardCreatePoint = warTeamInfo.warTeamId == 0 ? WarManager.instance.warTeam0VanguardCreatePoint : WarManager.instance.warTeam1VanguardCreatePoint;
        foreach(var vanguardHero in warModel.vanguardHeroes) {
            Instantiate(vanguardHero, vanguardCreatePoint, Quaternion.identity);
        }
    }

    public void CreateSoldiers() {
        var soldierCreatePoints = warTeamInfo.warTeamId == 0 ? WarManager.instance.warTeam0SoldierCreatePoints : WarManager.instance.warTeam1SoldierCreatePoints;
        foreach(var meleeSoldier in warModel.meleeSoldiers) {
            PersonManager.instance.CreatePerson(meleeSoldier, soldierCreatePoints[Random.Range(0, soldierCreatePoints.Length)]);
        }
        /*foreach(var rangeSoldier in warModel.rangeSoldiers) {
            PersonManager.instance.CreatePerson(rangeSoldier, soldierCreatePoints[Random.Range(0, soldierCreatePoints.Length)]);
        }*/
    }

    public WarTeamInfo GetInfo() {
        return warTeamInfo;
    }
}