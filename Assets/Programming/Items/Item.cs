using UnityEngine;

public class Item : MonoBehaviour {
    public Collider itemCollider;
    public GameObject owner;

    void Start()
    {
        itemCollider = GetComponentInChildren<Collider>();
        owner = transform.root.gameObject;
    }

    public Info GetOwnerInfo()
    {
        Info ownerInfo;
        if (owner.CompareTag("Player"))
        {
            // ownerInfo = owner.name == PlayerCharacter.Vanguard.ToString() ? (Info) owner.GetComponent<Vanguard>().GetInfo() : (Info) owner.GetComponent<Player>().GetInfo();
            ownerInfo = owner.GetComponent<Character>().playerCharacter == PlayerCharacter.Vanguard ? (Info) owner.GetComponent<Vanguard>().GetInfo() : (Info) owner.GetComponent<Player>().GetInfo();
        }
        else
        {
            ownerInfo = (Info) owner.GetComponent<Person>().GetInfo();
        }
        return ownerInfo;
    }
}