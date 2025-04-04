using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public Animator mainAnimator;

    void Start() {
        mainAnimator = GetComponent<Animator>();
    }

    public void Play(string animationName) {
        mainAnimator.Play(animationName);
    }

    public void Replay(string animationName) {
        mainAnimator.Play("Base Layer." + animationName, 0, 0);
    }

    public void SetParameter(string parameterName, object value) {
        switch(parameterName) {
            case "HurtAmount":
                mainAnimator.SetInteger(parameterName, (int) value);
                break;
            default:
                break;
        }
    }
}