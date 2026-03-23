using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class KingdomModel {
    public string kingdomName;
    // something to say "strength", "technology" or "life"
    public string kingdomDescription;
    public string mapName;
    public Sprite kingdomMapSprite;
    public FactionModel factionModel;
    public QuestScriptableObject questScriptableObject;

    public KingdomModel() {}

    public KingdomModel(
        string kingdomName,
        string kingdomDescription,
        string mapName,
        Sprite kingdomMapSprite,
        FactionModel factionModel,
        QuestScriptableObject questScriptableObject
    ) {
        this.kingdomName = kingdomName;
        this.kingdomDescription = kingdomDescription;
        this.mapName = mapName;
        this.kingdomMapSprite = kingdomMapSprite;
        this.factionModel = factionModel;
        this.questScriptableObject = questScriptableObject;
    }
}