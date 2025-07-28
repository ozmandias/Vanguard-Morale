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

    void OnGUI()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "mainmenu") {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Test"))
            {
                UIManager.instance.ChangeUIType(UIType.Game);
                ChangeScene("test");
            }
            
            if(GUI.Button(new Rect(10, 80, 150, 50), "Lab")) {
                UIManager.instance.ChangeUIType(UIType.Game);
                ChangeScene("lab");
            }
        }
    }
}