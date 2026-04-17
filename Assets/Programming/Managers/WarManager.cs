using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarManager : MonoBehaviour {
    public WarScriptableObject warScriptableObject;
    // two war teams
    WarTeam warTeam0;
    WarTeam warTeam1;
    public Vector3 warTeam0CreatePoint;
    public Vector3 []warTeam0VanguardCreatePoints;
    public Vector3 []warTeam0SoldierCreatePoints;
    public Vector3 warTeam1CreatePoint;
    public Vector3 []warTeam1VanguardCreatePoints;
    public Vector3 []warTeam1SoldierCreatePoints;
    public float soldierCreationWaitTime = 60f;
    public bool fightingAtWar = true;

    public UnityEvent CreateWarTeamEvent = new UnityEvent();

    public static WarManager instance;
    
    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        DetermineWarTeams();
    }

    void Update() {
        CheckWar();
    }

    void DetermineWarTeams() {
        // get 2 factions at war based on reputation on each other
        // 1 faction is current player's faction and another faction is the one with bad reputation to current kingdom
        // get player faction
        var playerFaction = GlobalData.characterDetails.faction;
        var playerKingdomReputations = ReputationManager.instance.GetKingdomReputations(playerFaction);
        Debug.Log("WarManager - playerFaction: " + playerFaction);
        
        // get enemy faction
        var enemyFaction = FactionManager.instance.GetRandomFactionOfReputationDetailsList(playerKingdomReputations, "enemy");
        enemyFaction = Faction.Ignis;
        Debug.Log("WarManager - enemyFaction: " + enemyFaction);
        
        // create war teams for 2 factions
        CreateWarTeams(playerFaction, enemyFaction);
    }

    void CreateWarTeams(Faction warTeam0Faction, Faction warTeam1Faction) {
        // get WarData
        var warData0 = warScriptableObject.dataList.Find((warData) => {
            return warData.faction == warTeam0Faction;
        }); Debug.Log("warData0 - faction: " + warData0.faction);
        var warData1 = warScriptableObject.dataList.Find((warData) => {
            return warData.faction == warTeam1Faction;
        }); Debug.Log("warData1 - faction: " + warData1.faction);

        // create 2 war teams based on 2 faction pairs
        // use war team create points
        var warTeam0Object = new GameObject("WarTeam0");
        warTeam0Object.transform.position = warTeam0CreatePoint;
        warTeam0 = warTeam0Object.AddComponent<WarTeam>();
        warTeam0.warModel = warData0;
        warTeam0.GetInfo().SetWarTeamId(0);

        var warTeam1Object = new GameObject("WarTeam1");
        warTeam1Object.transform.position = warTeam1CreatePoint;
        warTeam1 = warTeam1Object.AddComponent<WarTeam>();
        warTeam1.warModel = warData1;
        warTeam1.GetInfo().SetWarTeamId(1);

        // spawn WarTeams from their WarData
        StartWar();

        if(CreateWarTeamEvent != null) CreateWarTeamEvent.Invoke();
    }

    void StartWar() {
        // create army base
        warTeam0.CreateBase();
        warTeam1.CreateBase();

        // create Vanguard heroes at army base
        warTeam0.CreateVanguards();
        warTeam1. CreateVanguards();
        
        StartCoroutine(StartWarCoroutine());
    }

    void CheckWar() {
        // if isDestroyed from any WarTeam's Info class is true, end war and declare winner and loser
        if(fightingAtWar == true) {
            if(warTeam0.GetInfo().isDestroyed || warTeam1.GetInfo().isDestroyed) {
                fightingAtWar = false;
            }
        }
    }

    IEnumerator StartWarCoroutine() {
        while(fightingAtWar) {
            // create soldiers at army base
            warTeam0.CreateSoldiers();
            warTeam1.CreateSoldiers();

            // register soldiers to factionSoldiers list
            
            yield return new WaitForSeconds(soldierCreationWaitTime);
        }
    }
}