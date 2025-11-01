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

    public UnityEngine.SceneManagement.Scene GetCurrentScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public AsyncOperation ChangeSceneAsync(string sceneName) {
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    void OnGUI()
    {
        if (SceneManager.instance.GetCurrentScene().name == "mainmenu") {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fontSize = 30;

            sceneData = CharacterSelectController.instance.characterDetailsList[0];

            if (GUI.Button(new Rect(10, 10, 250, 100), "Test", guiStyle)) {
                ChangeSceneByLoading("test");
            }
            
            if (GUI.Button(new Rect(10, 120, 250, 100), "Lab", guiStyle)) {
                ChangeSceneByLoading("lab");
            }
        }
    }
}