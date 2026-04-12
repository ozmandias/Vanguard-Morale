using UnityEngine;

[System.Serializable] public class ReputationModel {
    public Faction otherFaction;
    public Reputation reputation;

    public ReputationModel() {}

    public ReputationModel(
        Faction otherFaction,
        Reputation reputation
    ) {
        this.otherFaction = otherFaction;
        this.reputation = reputation;
    }
}