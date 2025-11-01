using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour {
    public static CharacterSelectController instance;

    public GameObject characterContainer;

    public CharacterSerializable characterDetails;
    public  List<CharacterSerializable> characterDetailsList;
    int currentLocation = 0;

    public Button nextButton;
    public Button previousButton;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        MakeCharacterChanges();
    }

    public void NextCharacter() {
        if(currentLocation < characterDetailsList.Count - 1) {
            currentLocation += 1;
        }
        MakeCharacterChanges();
    }

    public void  PreviousCharacter() {
        if(currentLocation > 0) {
            currentLocation -= 1;
        }
        MakeCharacterChanges();
    }

    public void MakeCharacterChanges() {
        characterDetails = characterDetailsList[currentLocation];
        characterContainer.transform.Find("Image").GetComponent<Image>().sprite = characterDetails.characterSprite;
        characterContainer.transform.Find("Text").GetComponent<Text>().text = characterDetails.characterName;

        if(currentLocation == characterDetailsList.Count - 1) {
            nextButton.interactable = false;
        } else if(currentLocation < characterDetailsList.Count - 1) {
            nextButton.interactable = true;
        }

        if(currentLocation == 0) {
            previousButton.interactable = false;
        } else if(currentLocation > 0) {
            previousButton.interactable = true;
        }
    }

    public void ConfirmCharacterSelect() {
        // PlayerProfileController.instance.SetProfilePicture(characterDetails.profileSprite);
        SceneManager.instance.sceneData = characterDetails;
        MainMenuManager.instance.PlayStory();
    }

    public void CancelCharacterSelect() {
        MainMenuManager.instance.HideCharacterSelectPanel();
    }
}