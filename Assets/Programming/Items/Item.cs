using UnityEngine;

public class Item : MonoBehaviour {
    public Collider itemCollider;
    public GameObject owner;

    void Start()
    {
        itemCollider = GetComponentInChildren<Collider>();
        owner = transform.root.gameObject;
    }

    public CharacterInfo GetOwnerInfo()
    {
        CharacterInfo ownerInfo;
        if (owner.CompareTag("Player"))
        {
            // ownerInfo = owner.name == PlayerCharacter.Vanguard.ToString() ? (CharacterInfo) owner.GetComponent<Vanguard>().GetInfo() : (CharacterInfo) owner.GetComponent<Player>().GetInfo();
            ownerInfo = owner.GetComponent<Character>().playerCharacter == PlayerCharacter.Vanguard ? (CharacterInfo) owner.GetComponent<Vanguard>().GetInfo() : (CharacterInfo) owner.GetComponent<Player>().GetInfo();
        }
        else
        {
            ownerInfo = (CharacterInfo) owner.GetComponent<Person>().GetInfo();
        }
        return ownerInfo;
    }
}