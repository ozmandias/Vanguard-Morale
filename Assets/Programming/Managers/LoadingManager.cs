using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour {
    public Text loadingText;
    public float loadSpeed = 2f;
    AsyncOperation loadOperation;
    float loadingProgress = 0f;
    float loadRate = 0f;

    void Start() {
        StartCoroutine(Loading());
    }

    void Update() {
        
    }

    IEnumerator Loading() {
        loadRate = 0.01f * loadSpeed;
        loadOperation = SceneManager.instance.ChangeSceneAsync(SceneManager.instance.currentScene);
        loadOperation.allowSceneActivation = false;
        while(loadingProgress <= 0.99f && loadOperation.isDone == false) {
            yield return new WaitForSeconds(loadRate /*0.01f*/);
            loadingProgress += loadRate /*0.01f*/;
            loadingText.text = "Loading: " + Mathf.Round(loadingProgress /*loadOperation.progress*/ * 100) + "%";
        }
        if(loadingProgress >= 0.99f && loadOperation.progress >= 0.9f) {
            loadOperation.allowSceneActivation = true;
        }
    }
}