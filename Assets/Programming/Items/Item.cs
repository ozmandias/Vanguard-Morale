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
            ownerInfo = owner.name == PlayerCharacter.MasterKnight.ToString() ? (Info) owner.GetComponent<MasterKnight>().GetInfo() : (Info) owner.GetComponent<Player>().GetInfo();
        }
        else
        {
            ownerInfo = (Info) owner.GetComponent<Person>().GetInfo();
        }
        return ownerInfo;
    }
}