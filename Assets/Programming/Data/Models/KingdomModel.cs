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
}