using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionObject : MonoBehaviour {
    public Button characterSelectButton;
    public Image characterSelectFrame;
    public CharacterSerializable characterSelection;
    public GameObject characterObject;
    Quaternion characterOriginalRotation;
    Color selectColor = new Color(0.6f, 0.4f, 0.8f, 1f);
    Color originalColor = new Color(1f, 1f, 1f, 1f);

    void Start() {
        characterSelectButton = GetComponent<Button>();
        characterSelectFrame = transform.GetChild(transform.childCount - 1).GetComponent<Image>();

        if(characterObject) {
            characterOriginalRotation = characterObject.transform.rotation;
        }

        if(CharacterSelectionManager.instance) {
            characterSelectButton.onClick.AddListener(() => CharacterSelectionManager.instance.SelectCharacter(this));
        }
    }

    public void SetSelectionFrameColor(string colorType) {
        if(colorType == "original") {
            characterSelectFrame.color = originalColor;
        } else if(colorType == "select") {
            characterSelectFrame.color = selectColor;
        }
    }
}