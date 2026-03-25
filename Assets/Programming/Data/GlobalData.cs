using UnityEditor;

public static class GlobalData {
    public static CharacterModel characterDetails =
    new CharacterModel(
        PlayerCharacter.Vanguard,
        "Van Ironmark",
        "Vanguard of Fortis",
        "Warstorm Knight",
        "",
        Faction.Fortis,
        null,
        null,
        new StatsModel(70, 50, 50, 100, 50, 30, 60)
    );

    public static KingdomModel kingdomDetails;

    public static QuestScriptableObject currentKingdomQuestScriptableObject;

    public static CharacterModel arenaVanguardPlayer;

    public static CharacterModel arenaVanguardPerson;

    public static GameMode gameMode;
}