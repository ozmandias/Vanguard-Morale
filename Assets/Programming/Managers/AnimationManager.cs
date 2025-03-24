using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public Animator mainAnimator;

    void Start() {
        mainAnimator = GetComponent<Animator>();
    }

    public void Play(string animationName) {
        mainAnimator.Play(animationName);
    }
}