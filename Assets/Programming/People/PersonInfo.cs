using UnityEngine;
using UnityEngine.AI;

[System.Serializable] public class PersonInfo : Info
{
    public RagdollManager personRagdollManager;
    public PersonType personType = PersonType.Neutral;
    public AIType aiType = AIType.StateMachine;
    public bool stateMachineDead = false;

    public /*PersonInfo() : base()*/ void Init(GameObject owner)
    {
        base.Init(owner);
        damage = 10;
        morality = personType == PersonType.Neutral ? 50 : personType == PersonType.Friend ? 60 : 40;
        alignment = (Morality)morality;

        personRagdollManager = owner.GetComponent<RagdollManager>();
    }

    public override void ReduceHealth(int damageAmount)
    {
        base.ReduceHealth(damageAmount);

        if (isDead == true)
        {
            switch (personType)
            {
                case PersonType.Enemy:
                    (
                        GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight
                        ?
                        (Info) GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo()
                        :
                        (Info) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                    ).AddMorality(3);
                    break;
                case PersonType.Neutral:
                    break;
                case PersonType.Friend:
                    (
                        GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight
                        ?
                        (Info) GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo()
                        :
                        (Info) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                    ).ReduceMorality(6);
                    break;
                default:
                    break;
            }
        }
    }
    
    public void CombatReduceHealth(int damageAmount) {
        base.ReduceHealth(damageAmount);

        if (isDead == true && personType == PersonType.Enemy)
        {
            Person mainPerson = owner.GetComponent<Person>();
            (
                GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight
                ?
                (Info) GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo()
                :
                (Info) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
            ).AddMorality(3);
            aiType = AIType.StateMachine;
            mainPerson.ChangeState(StateMachine.Dead);
            stateMachineDead = true;
            personRagdollManager.EnableRagdoll();
            if (mainPerson.weapon)
            {
                mainPerson.weapon.transform.SetParent(null);
                mainPerson.weapon.AddComponent<Rigidbody>();
                mainPerson.weapon.AddComponent<BoxCollider>();
            }
            mainPerson.Dead();
        }
    }
}