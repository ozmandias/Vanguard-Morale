using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReputationManager : MonoBehaviour { // for each Kingdom
    public List<ReputationModel> reputationList;

    public static ReputationManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        reputationList = GlobalData.kingdomDetails.reputations.ToList();
    }

    public Reputation GetPlayerReputation(Faction playerFaction) {
        var playerReputation = reputationList.Find((reputation) => {
            return reputation.otherFaction == playerFaction;
        });
        return playerReputation.reputation;
    }
}