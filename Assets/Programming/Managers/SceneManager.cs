using UnityEngine;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance;

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

    public void ChangeScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}