using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour {
    public Image introImage;
    public float interpolateTime = 3f;
    float timePassed = 0;
    Color hiddenColor;
    Color displayColor;

    void Start() {
        hiddenColor = introImage.color;
        displayColor = new Color(introImage.color.r, introImage.color.b, introImage.color.g, 1f);
    }

    void Update() {
        Introduction();
    }

    public void Introduction() {
        if(timePassed < interpolateTime) {
            timePassed += Time.fixedDeltaTime;
            introImage.color = Color.LerpUnclamped(hiddenColor, displayColor, timePassed / interpolateTime); 
        } else {
            StartCoroutine(IntroductionCoroutine());
        }
    }

    IEnumerator IntroductionCoroutine() {
        yield return new WaitForSeconds(2f);
        SceneManager.instance.ChangeScene("mainmenu");
    }
}