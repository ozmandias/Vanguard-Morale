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
            case "HurtAmount":
                mainAnimator.SetInteger(parameterName, (int) value);
                break;
            case "Velocity":
                mainAnimator.SetFloat(parameterName, (float) value);
                break;
            case "ReduceHealth":
                mainAnimator.SetBool(parameterName, (bool) value);
                break;
            default:
                break;
        }
    }
}