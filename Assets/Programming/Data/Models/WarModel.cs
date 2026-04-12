using UnityEngine;

[System.Serializable] public class WarModel {
    public GameObject mainBase;
    public GameObject []vanguardHeroes;
    public GameObject []meleeSoldiers;
    public GameObject []rangeSoldiers;
    public Faction faction;

    public WarModel() {}

    public WarModel(
        GameObject mainBase,
        GameObject []vanguardHeroes,
        GameObject []meleeSoldiers,
        GameObject []rangeSoldiers,
        Faction faction
    ) {
        this.mainBase = mainBase;
        this.vanguardHeroes = vanguardHeroes;
        this.meleeSoldiers = meleeSoldiers;
        this.rangeSoldiers = rangeSoldiers;
        this.faction = faction;
    }
}