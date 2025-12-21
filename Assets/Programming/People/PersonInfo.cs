using UnityEngine;
using UnityEngine.AI;

[System.Serializable] public class PersonInfo : Info {
    public RagdollManager personRagdollManager;
    public PersonType personType = PersonType.Normal;
    public AIType aiType = AIType.StateMachine;
    public bool stateMachineDead = false;

    public /*PersonInfo() : base()*/ void Init(GameObject owner)
    {
        base.Init(owner);
        strength = 30;
        agility = 30;
        intelligence = 30;
        damage = 10 + (int) Mathf.Round(strength / 10);
        magic = 5;
        morality = personType == PersonType.Normal ? 50 : personType == PersonType.Friend ? 60 : 40;
        alignment = (Morality)morality;

        personRagdollManager = owner.GetComponent<RagdollManager>();
    }

    public void ReduceHealth(Info attackerInfo)
    {
        base.ReduceHealth(attackerInfo.damage);
        if (isDead == true) {
            if (attackerInfo is MasterKnightInfo || attackerInfo is PlayerInfo)
            {
                switch (personType)
                {
                    case PersonType.Enemy:
                        if(GameManager.instance.playerGameObject) (
                            GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight
                            ?
                            (Info) GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo()
                            :
                            (Info) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                        ).AddMorality(3);
                        break;
                    case PersonType.Normal:
                        break;
                    case PersonType.Friend:
                        if(GameManager.instance.playerGameObject) (
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
    }
    
    public void CombatReduceHealth(Info attackerInfo) {
        base.ReduceHealth(attackerInfo.damage);

        if(isDead == true) {
            if ((attackerInfo is MasterKnightInfo || attackerInfo is PlayerInfo) && personType == PersonType.Enemy)
            {
                Person mainPerson = owner.GetComponent<Person>();
                (
                    GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight
                    ?
                    (Info) GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo()
                    :
                    (Info) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                ).AddMorality(3);
            }
            MakePerson("dead");
        }
    }

    public void MakePerson(string makeStatus) {
        Person mainPerson = owner.GetComponent<Person>();
        if(makeStatus == "alive") {
            AddHealth(MaxHealth);
            mainPerson.personAgent.enabled = true;
            mainPerson.personCombat.enemyIsAttackable = true;
            stateMachineDead = false;
            personRagdollManager.DisableRagdoll();
            owner.GetComponent<StateMachineManager>().ChangeState(StateMachine.Idle);
        } else if(makeStatus == "dead") {
            mainPerson.personAgent.enabled = false;
            mainPerson.personCombat.enemyIsAttackable = false;
            stateMachineDead = true;
            personRagdollManager.EnableRagdoll();
            if (mainPerson.weapon)
            {
                mainPerson.weapon.transform.SetParent(null);
                mainPerson.weapon.AddComponent<Rigidbody>();
                mainPerson.weapon.AddComponent<BoxCollider>();
            }
            owner.GetComponent<StateMachineManager>().ChangeState(StateMachine.Dead);
        }
    }
}