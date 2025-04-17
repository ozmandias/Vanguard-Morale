using UnityEngine;
using UnityEngine.AI;

public class PersonInfo : Info {
    public RagdollManager personRagdollManager;
    public PersonType personType = PersonType.Neutral;
    public bool stateMachineDead = false;

    public override void Start() {
        base.Start();

        damage = 10;
        morality = personType == PersonType.Neutral ? 50 : personType == PersonType.Friend ? 60 : 40;
        alignment = (Morality) morality;

        personRagdollManager = GetComponent<RagdollManager>();
    }

    public override void ReduceHealth(int damageAmount) {
        base.ReduceHealth(damageAmount);

        if(isDead == true) {
            switch (personType)
            {   case PersonType.Enemy:
                    GameManager.instance.playerGameObject.GetComponent<Info>().AddMorality(3);
                    break;
                case PersonType.Neutral:
                    break;
                case PersonType.Friend:
                    GameManager.instance.playerGameObject.GetComponent<Info>().ReduceMorality(6);
                    break;
                default:
                    break;
            }
        }
    }
}