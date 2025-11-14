using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour {
    public Button backButton;
    public Button selectButton;
    public bool hasCharacterSelection = false;
    public CharacterSerializable currentCharacterSelection;
    public GameObject currentCharacterObject;
    CharacterSelectionObject previousCharacterSelectionObject;

    public delegate void CharacterSelectDelegate();
    public CharacterSelectDelegate OnCharacterSelectDelegate;
    
    public static CharacterSelectionManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    
    void Start() {
        selectButton.onClick.AddListener(ChangeToWorldMapSelection);
        backButton.onClick.AddListener(LeaveFromCharacterSelection);

        OnCharacterSelectDelegate += ShowCharacter;
    }

    public void SelectCharacter(CharacterSelectionObject characterSelectionObject) {
        if(hasCharacterSelection == false) {
            hasCharacterSelection = true;
            characterSelectionObject.SetSelectionFrameColor("select");
            currentCharacterSelection = characterSelectionObject.characterSelection;
            previousCharacterSelectionObject = characterSelectionObject;

            if(OnCharacterSelectDelegate != null) {
                OnCharacterSelectDelegate.Invoke();
            }
        } else {
            // switch selection to another
            if(currentCharacterSelection != null) {
                if(characterSelectionObject == previousCharacterSelectionObject) {
                    hasCharacterSelection = false;
                    characterSelectionObject.SetSelectionFrameColor("original");
                    currentCharacterSelection = null;
                    previousCharacterSelectionObject = null;

                    HideCharacter();
                } else {
                    hasCharacterSelection = true;
                    previousCharacterSelectionObject.SetSelectionFrameColor("original");
                    characterSelectionObject.SetSelectionFrameColor("select");
                    currentCharacterSelection = characterSelectionObject.characterSelection;
                    previousCharacterSelectionObject = characterSelectionObject;

                    if(OnCharacterSelectDelegate != null) {
                        OnCharacterSelectDelegate.Invoke();
                    }
                }
            } else {
                hasCharacterSelection = false;
                characterSelectionObject.SetSelectionFrameColor("original");
                currentCharacterSelection = null;
                previousCharacterSelectionObject = null;

                HideCharacter();
            }
        }
    }

    void ChangeToWorldMapSelection() {
        if(SceneManager.instance) {
            SceneManager.instance.ChangeSceneByFading("worldmapselection");
        }
    }

    void LeaveFromCharacterSelection() {
        SceneManager.instance.ChangeSceneByFading("mainmenu");
    }

    void ShowCharacter() {
        selectButton.gameObject.SetActive(true);
    }

    void HideCharacter() {
        selectButton.gameObject.SetActive(false);
    }
}