using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour {
    public Button backButton;

    public static CharacterSelectionManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    
    void Start() {
        backButton.onClick.AddListener(LeaveCharacterSelection);
    }

    void LeaveCharacterSelection() {
        SceneManager.instance.ChangeSceneByFading("mainmenu");
    }
}