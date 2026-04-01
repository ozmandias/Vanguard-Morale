using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class KingdomModel {
    public string kingdomName;
    // something to say "strength", "technology" or "life"
    public string kingdomDescription;
    public string codeName;
    public Sprite kingdomMapSprite;
    public Faction faction;
    public ReputationModel []reputations;

    public KingdomModel() {}

    public KingdomModel(
        string kingdomName,
        string kingdomDescription,
        string codeName,
        Sprite kingdomMapSprite,
        Faction faction
    ) {
        this.kingdomName = kingdomName;
        this.kingdomDescription = kingdomDescription;
        this.codeName = codeName;
        this.kingdomMapSprite = kingdomMapSprite;
        this.faction = faction;
    }
}