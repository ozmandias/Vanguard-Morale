using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {
    public Image fadeImage;
    public float fadeTime = 1f;
    public string currentFade = "in";

    public static FadeManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Fade(string fadeParameter) {
        if(fadeParameter == "in") {
            StartCoroutine(FadeIn());
        } else if(fadeParameter == "out") {
            StartCoroutine(FadeOut());
        }
    }

    // appear from black
    IEnumerator FadeIn() {
        currentFade = "in";
        for(float i = fadeTime; i > 0; i = i - Time.deltaTime) {
            Color fadeImageColor = fadeImage.color;
            fadeImageColor.a = Mathf.Clamp01(i / fadeTime);
            fadeImage.color = fadeImageColor;
            yield return null;
        }
    }

    // disappear out of sight
    IEnumerator FadeOut() {
        currentFade = "out";
        for(float i = 0; i < fadeTime; i = i + Time.deltaTime) {
            Color fadeImageColor = fadeImage.color;
            fadeImageColor.a = Mathf.Clamp01(i / fadeTime);
            fadeImage.color = fadeImageColor;
            yield return null;
        }
    }
}