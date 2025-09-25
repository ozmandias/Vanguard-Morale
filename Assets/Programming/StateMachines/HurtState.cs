using UnityEngine;

public class HurtState : StateMachineBehaviour {
    Person mainPerson;
    public Info attackerInfo;
    float stateLength = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = true;

        if(animator.GetBool("ReduceHealth") == true) {
            animator.SetBool("ReduceHealth", false);
            mainPerson.GetInfo().ReduceHealth(attackerInfo /*animator.GetInteger("HurtAmount")*/);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(stateInfo.IsName("Hurt")) {
            stateLength = stateInfo.length;
        }

        if(mainPerson.hurtFrames > /*0.1f*/ stateLength && stateLength > 0) {
            animator.SetInteger("HurtAmount", 0);
            attackerInfo = null;
            mainPerson.isHurt = false;
            mainPerson.hurtFrames = 0;
            stateLength = 0;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }
}