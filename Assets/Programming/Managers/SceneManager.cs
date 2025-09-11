using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

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
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public UnityEngine.SceneManagement.Scene GetCurrentScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    void OnGUI()
    {
        if (SceneManager.instance.GetCurrentScene().name == "mainmenu") {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 30;

            if (GUI.Button(new Rect(10, 10, 250, 100), "Test", buttonStyle)) {
                UIManager.instance.ChangeUIType(UIType.Game);
                ChangeScene("test");
            }
            
            if (GUI.Button(new Rect(10, 120, 250, 100), "Lab", buttonStyle)) {
                UIManager.instance.ChangeUIType(UIType.Game);
                ChangeScene("lab");
            }
        }
    }
}