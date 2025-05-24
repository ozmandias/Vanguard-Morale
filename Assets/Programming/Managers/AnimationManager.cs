using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public Animator mainAnimator;

    void Start() {
        mainAnimator = GetComponent<Animator>();
    }

    public void Play(string animationName) {
        mainAnimator.Play(animationName);
    }

    public void PlayByFrame(string animationName, float normalizedTime) {
        mainAnimator.Play("Base Layer." + animationName, 0, normalizedTime);
    }

    public void SetParameter(string parameterName, object value) {
        switch(parameterName) {
            case "Velocity":
                mainAnimator.SetFloat(parameterName, (float) value);
                break;
            case "Attacking":
                mainAnimator.SetBool(parameterName, (bool) value);
                break;
            case "ReduceHealth":
                mainAnimator.SetBool(parameterName, (bool) value);
                break;
            case "HurtAmount":
                mainAnimator.SetInteger(parameterName, (int) value);
                break;
            default:
                break;
        }
    }

    public void AttackEvent(string attackEventParameter) {
        if(attackEventParameter == "on") {
            SetParameter("Attacking", true);
        } else if(attackEventParameter == "off") {
            SetParameter("Attacking", false);
        }
    }
}