using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance;
    public string currentScene = "";
    // public object sceneData;
    public SceneScriptableObject sceneScriptableObject;
    SceneSerializable sceneDetails;

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
        sceneDetails = sceneScriptableObject.dataList.Find(scene => scene.sceneName.Contains(currentScene));
    }

    public void ChangeScene(string sceneName)
    {
        DOTween.Clear(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
        sceneDetails = sceneScriptableObject.dataList.Find(scene => scene.sceneName.Contains(currentScene));
    }

    public void ChangeSceneByLoading(string sceneName) {
        ChangeScene("loading");
        currentScene = sceneName;
        sceneDetails = sceneScriptableObject.dataList.Find(scene => scene.sceneName.Contains(currentScene));
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
            WorldScriptableObject worldScriptableObject = (WorldScriptableObject) AssetDatabase.LoadAssetAtPath("Assets/Data/World/VanguardWorldData.asset", typeof(WorldScriptableObject));
            GlobalData.kingdomDetails = worldScriptableObject.dataList[0];
            GlobalData.currentKingdomQuestScriptableObject = GlobalData.kingdomDetails.questScriptableObject;

            if (GUI.Button(new Rect(10, 10, 250, 100), "Test", guiStyle)) {
                ChangeSceneByLoading("test");
            }
            
            if (GUI.Button(new Rect(10, 120, 250, 100), "Lab", guiStyle)) {
                ChangeSceneByLoading("lab");
            }
        }
    }

    IEnumerator ChangeSceneByFadingCoroutine(string sceneName) {
        if(sceneDetails != null && sceneDetails.fadeUI) {
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
        if(sceneDetails != null && sceneDetails.fadeUI) {
            if(sceneUIChanger) sceneUIChanger.OnUIChangerStartDelegate += sceneUIChanger.StopUIChanging;
        }
        if(FadeManager.instance) {
            if(FadeManager.instance.currentFade == "out") {
                FadeManager.instance.Fade("in");
                yield return new WaitForSeconds(FadeManager.instance.fadeTime);
            }
        }
        if(sceneUIChanger) sceneUIChanger.hasUIChanges = true;
        if(sceneUIChanger && sceneUIChanger.OnUIChangerStartDelegate != null) {
            sceneUIChanger.OnUIChangerStartDelegate -= sceneUIChanger.StopUIChanging;
        }
    }
}