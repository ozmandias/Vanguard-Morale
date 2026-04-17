using UnityEngine;

[System.Serializable] public class WarModel {
    public SpawnModel mainBase;
    public SpawnModel []vanguardHeroes;
    public SpawnModel []meleeSoldiers;
    public SpawnModel []rangeSoldiers;
    public Faction faction;

    public WarModel() {}

    public WarModel(
        SpawnModel mainBase,
        SpawnModel []vanguardHeroes,
        SpawnModel []meleeSoldiers,
        SpawnModel []rangeSoldiers,
        Faction faction
    ) {
        this.mainBase = mainBase;
        this.vanguardHeroes = vanguardHeroes;
        this.meleeSoldiers = meleeSoldiers;
        this.rangeSoldiers = rangeSoldiers;
        this.faction = faction;
    }
}