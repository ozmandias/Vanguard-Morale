using UnityEngine;

public class Citizen : Person {
    public override void Start() {
        base.Start();

        /*GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);*/
        
        if(GameManager.instance.OnCharacterListRegister != null)
            GameManager.instance.OnCharacterListRegister.Invoke(personInfo.personType, this as Person);
        if(AIManager.instance.OnAIListRegister != null)
            AIManager.instance.OnAIListRegister.Invoke(personInfo.personType, personAgent);
    }

    public override void Update() {
        base.Update();
    }

    public override void Idle() {
        base.Idle();
    }

    public override void Move() {
        base.Move();
    }

    public override void Work() {
        base.Work();
    }

    public override void Follow() {
        base.Follow();
    }

    public override void Attack() {
        base.Attack();
    }

    public override void Hurt() {
        base.Hurt();
    }

    public override void Dead() {
        base.Dead();

        /*GameManager.instance.personList.Remove(this);
        AIManager.instance.personAIList.Remove(personAgent);*/

        if(GameManager.instance.OnCharacterListUnregister != null)
            GameManager.instance.OnCharacterListUnregister.Invoke(personInfo.personType, this as Person);
        if(AIManager.instance.OnAIListUnregister != null)
            AIManager.instance.OnAIListUnregister.Invoke(personInfo.personType, personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        /*GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);*/

        if(GameManager.instance.OnCharacterListRegister != null)
            GameManager.instance.OnCharacterListRegister.Invoke(personInfo.personType, this as Person);
        if(AIManager.instance.OnAIListRegister != null)
            AIManager.instance.OnAIListUnregister.Invoke(personInfo.personType, personAgent);
    }

    public override void FindTarget() {
        
    }

    public override bool ShouldFindTarget() {
        return false;
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("SoldierAttackCollider")) {
            CharacterInfo attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
            if(personInfo.isDead == false && attackCharacterInfo.isDead == false){
                personAnimation.SetParameter("HurtAmount", attackCharacterInfo.damage);
                personAnimation.SetParameter("ReduceHealth", true);
                personAnimation.mainAnimator.GetBehaviour<HurtState>().attackerInfo = attackCharacterInfo;
                hurtFrames = 0;
                isHurt = true;

                int changeTargetRandom = Random.Range(0, 10);
                if(personState.stateMachineTargeting == false || (personState.stateMachineTargeting && changeTargetRandom >= 5)) {
                    if(target) {
                        CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                        /*if(currentTargetCombat.CirclingListContains(personAgent)) {
                            currentTargetCombat.circlingList.Remove(personAgent);
                        }*/
                        if(currentTargetCombat.OnCirclingListUnregister != null)
                            currentTargetCombat.OnCirclingListUnregister.Invoke(personAgent);
                    }
                    SetTarget(attackCharacterInfo.owner);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}