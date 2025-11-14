using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance;
    public string currentScene = "";
    public object sceneData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChangedEvent;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoadedEvent;

        currentScene = GetCurrentScene().name;
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }

    public void ChangeSceneByLoading(string sceneName) {
        ChangeScene("loading");
        currentScene = sceneName;
    }

    public void ChangeSceneByFading(string sceneName) {
        StartCoroutine(ChangeSceneByFadingCoroutine(sceneName));
    }

    public UnityEngine.SceneManagement.Scene GetCurrentScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public AsyncOperation ChangeSceneAsync(string sceneName) {
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    public void ActiveSceneChangedEvent(UnityEngine.SceneManagement.Scene currentScene, UnityEngine.SceneManagement.Scene nextScene) {
    }

    public void SceneLoadedEvent(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
        StartCoroutine(SceneLoadedByFadingCoroutine(scene));
    }

    void OnGUI()
    {
        if (SceneManager.instance.GetCurrentScene().name == "mainmenu") {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fontSize = 30;

            GlobalData.characterDetails = CharacterSelectController.instance.characterDetailsList[0];

            if (GUI.Button(new Rect(10, 10, 250, 100), "Test", guiStyle)) {
                ChangeSceneByLoading("test");
            }
            
            if (GUI.Button(new Rect(10, 120, 250, 100), "Lab", guiStyle)) {
                ChangeSceneByLoading("lab");
            }
        }
    }

    IEnumerator ChangeSceneByFadingCoroutine(string sceneName) {
        if(GetCurrentScene().name != "worldmapselection") {
            GameHelpers.GetUIChanger(currentScene).HideUIs();
        }
        if(FadeManager.instance) {
            FadeManager.instance.Fade("out");
            yield return new WaitForSeconds(FadeManager.instance.fadeTime);
        }
        ChangeScene(sceneName);
    }

    IEnumerator SceneLoadedByFadingCoroutine(UnityEngine.SceneManagement.Scene scene) {
        UIChanger sceneUIChanger = GameHelpers.GetUIChanger(currentScene);
        if(scene.name != "worldmapselection") {
            sceneUIChanger.OnUIChangerStartDelegate += sceneUIChanger.StopUIChanging;
        }
        if(FadeManager.instance) {
            if(FadeManager.instance.currentFade == "out") {
                FadeManager.instance.Fade("in");
                yield return new WaitForSeconds(FadeManager.instance.fadeTime);
            }
        }
        sceneUIChanger.hasUIChanges = true;
        if(sceneUIChanger.OnUIChangerStartDelegate != null) {
            sceneUIChanger.OnUIChangerStartDelegate -= sceneUIChanger.StopUIChanging;
        }
    }
}