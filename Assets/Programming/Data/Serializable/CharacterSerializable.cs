using UnityEngine;

[System.Serializable] public class CharacterSerializable  {
    public PlayerCharacter character;
    public string characterName;
    public string characterDescription;
    public Sprite characterSprite;
    public Sprite profileSprite;
    public int health = 100;
    public int damage = 50;
}