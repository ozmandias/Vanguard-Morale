using UnityEngine;

public class HurtState : StateMachineBehaviour {
    Person mainPerson;
    float stateTimer = 0;
    float stateLength = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        mainPerson.ChangeState(StateMachine.Hurt);

        mainPerson.personInfo.ReduceHealth(animator.GetInteger("HurtAmount"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        stateTimer += Time.deltaTime;
        
        if(stateInfo.IsName("Hurt")) {
            stateLength = stateInfo.length;
        }

        if(stateTimer > stateLength && stateLength > 0) {
            animator.SetInteger("HurtAmount", 0);
            stateTimer = 0;
            stateLength = 0;

            if(mainPerson.personInfo.isDead == false) {
                mainPerson.ChangeState(StateMachine.Attack);
            } else {
                mainPerson.ChangeState(StateMachine.Dead);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }
}