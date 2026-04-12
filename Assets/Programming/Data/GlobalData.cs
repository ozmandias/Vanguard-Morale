using UnityEditor;

public static class GlobalData {
    public static CharacterModel characterDetails =
    new CharacterModel(
        PlayerCharacter.Vanguard,
        "Van Ironmark",
        "Vanguard of Fortis",
        "Warstorm Knight",
        "Description",
        "Vanguard - Warstorm Knight",
        Faction.Fortis,
        null,
        null,
        new StatsModel(70, 50, 50, 100, 50, 30, 60)
    );

    public static KingdomModel kingdomDetails =
    new KingdomModel(
        "Kingdom of Fortis",
        "Description",
        "KingdomOfFortis",
        null,
        Faction.Fortis,
        new ReputationModel[1] {
            new ReputationModel(Faction.Ignis, Reputation.Hostile)
        }
    );

    public static CharacterModel arenaVanguardPlayer;

    public static CharacterModel arenaVanguardPerson;

    public static GameMode gameMode;
}