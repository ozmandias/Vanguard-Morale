using UnityEngine;

[System.Serializable] public class CharacterModel  {
    public PlayerCharacter character;
    public string characterName;
    public string vanguardTitle;
    public string characterTitle;
    public string characterDescription;
    public string codeName;
    public Faction faction;
    public ReputationModel []reputations;
    public Sprite characterSprite;
    public Sprite profileSprite;
    public StatsModel characterStats;

    public CharacterModel() {}

    public CharacterModel(
        PlayerCharacter character,
        string characterName,
        string vanguardTitle,
        string characterTitle,
        string characterDescription,
        string codeName,
        Faction faction,
        Sprite characterSprite,
        Sprite profileSprite,
        StatsModel characterStats
    ) {
        this.character = character;
        this.characterName = characterName;
        this.vanguardTitle = vanguardTitle;
        this.characterTitle = characterTitle;
        this.characterDescription = characterDescription;
        this.codeName = codeName;
        this.faction = faction;
        this.characterSprite = characterSprite;
        this.profileSprite = profileSprite;
        this.characterStats = characterStats;
    }
}