using UnityEngine;
using UnityEngine.AI;

[System.Serializable] public class PersonInfo : CharacterInfo {
    public Person person;
    public PersonType personType = PersonType.Normal;

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

        person = owner.GetComponent<Person>();
    }

    public void ReduceHealth(CharacterInfo attackerInfo)
    {
        base.ReduceHealth(attackerInfo.damage);
        if (isDead == true) {
            if (attackerInfo is VanguardInfo || attackerInfo is PlayerInfo)
            {
                switch (personType)
                {
                    case PersonType.Enemy:
                        if(GameManager.instance.playerGameObject) (
                            GameManager.instance.currentPlayer == PlayerCharacter.Vanguard
                            ?
                            (CharacterInfo) GameManager.instance.playerGameObject.GetComponent<Vanguard>().GetInfo()
                            :
                            (CharacterInfo) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                        ).AddMorality(3);
                        break;
                    case PersonType.Normal:
                        break;
                    case PersonType.Friend:
                        if(GameManager.instance.playerGameObject) (
                            GameManager.instance.currentPlayer == PlayerCharacter.Vanguard
                            ?
                            (CharacterInfo) GameManager.instance.playerGameObject.GetComponent<Vanguard>().GetInfo()
                            :
                            (CharacterInfo) GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo()
                        ).ReduceMorality(6);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void MakeLife(string makeStatus) {
        if(makeStatus == "alive") {
            AddHealth(MaxHealth);
            person.personAgent.enabled = true;
            person.personCombat.enemyIsAttackable = true;
            person.personState.stateMachineDead = false;
            person.personRagdoll.DisableRagdoll();
            owner.GetComponent<StateMachineChanger>().ChangeState(StateMachine.Idle);
            owner.GetComponent<AIChanger>().aiChangerRunning = true;

            person.CancelDestroyCountdown();
        } else if(makeStatus == "dead") {
            person.personAgent.enabled = false;
            person.personCombat.enemyIsAttackable = false;
            person.personState.stateMachineDead = true;
            person.personRagdoll.EnableRagdoll();
            if (person.weapons != null && person.weapons.Length > 0)
            {
                foreach(var weapon in person.weapons) {
                    weapon.transform.SetParent(null);
                    weapon.AddComponent<Rigidbody>();
                    weapon.AddComponent<BoxCollider>();
                }
            }
            owner.GetComponent<StateMachineChanger>().ChangeState(StateMachine.Dead);
            owner.GetComponent<AIChanger>().aiChangerRunning = false;

            person.StartDestroyCountdown();
        }
    }
}