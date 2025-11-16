using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour {
    Camera characterSelectionCamera;
    float mouseHorizontal = -90f /*0f*/;
    public float characterSelectionRotateSpeed = 8f;
    public Button backButton;
    public Button selectButton;
    public bool hasCharacterSelection = false;
    public CharacterSerializable currentCharacterSelection;
    public GameObject currentCharacterObject;
    public Transform cameraOriginalTransform;
    public GameObject originalCinemachineObject;
    public GameObject selectionCinemachineObject;
    public Transform characterFocusTransform;
    public CharacterSelectionObject []characterSelectionObjects;
    CharacterSelectionObject previousCharacterSelectionObject;

    public delegate void CharacterSelectDelegate(string selectStatus);
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
        characterSelectionCamera = Camera.main;

        characterSelectionObjects = GetComponentsInChildren<CharacterSelectionObject>();
        if(characterSelectionObjects.Length > 0) {
            
        }

        selectButton.onClick.AddListener(ChangeToWorldMapSelection);
        backButton.onClick.AddListener(LeaveFromCharacterSelection);

        OnCharacterSelectDelegate += ChangeCinemachineCamera;
    }

    void Update() {
        RotateCharacter();
    }

    public void SelectCharacter(CharacterSelectionObject characterSelectionObject) {
        if(hasCharacterSelection == false) {
            hasCharacterSelection = true;
            characterSelectionObject.SetSelectionFrameColor("select");
            currentCharacterSelection = characterSelectionObject.characterSelection;
            currentCharacterObject = characterSelectionObject.characterObject;
            previousCharacterSelectionObject = characterSelectionObject;
            ShowCharacter();
            if(OnCharacterSelectDelegate != null) {
                OnCharacterSelectDelegate.Invoke("select");
            }
        } else {
            // switch selection to another
            if(currentCharacterSelection != null) {
                if(characterSelectionObject == previousCharacterSelectionObject) {
                    // selecting same character
                    hasCharacterSelection = false;
                    characterSelectionObject.SetSelectionFrameColor("original");
                    HideCharacter();
                    currentCharacterSelection = null;
                    currentCharacterObject = null;
                    previousCharacterSelectionObject = null;
                    if(OnCharacterSelectDelegate != null) {
                        OnCharacterSelectDelegate.Invoke("unselect");
                    }
                } else {
                    // selecting different character
                    hasCharacterSelection = true;
                    previousCharacterSelectionObject.SetSelectionFrameColor("original");
                    characterSelectionObject.SetSelectionFrameColor("select");
                    HideCharacter();
                    currentCharacterSelection = characterSelectionObject.characterSelection;
                    currentCharacterObject = characterSelectionObject.characterObject;
                    previousCharacterSelectionObject = characterSelectionObject;
                    ShowCharacter();
                    if(OnCharacterSelectDelegate != null) {
                        OnCharacterSelectDelegate.Invoke("select");
                    }
                }
            } else {
                hasCharacterSelection = false;
                characterSelectionObject.SetSelectionFrameColor("original");
                HideCharacter();
                currentCharacterSelection = null;
                currentCharacterObject = null;
                previousCharacterSelectionObject = null;
                if(OnCharacterSelectDelegate != null) {
                    OnCharacterSelectDelegate.Invoke("unselect");
                }
            }
        }
    }
    
    void RotateCharacter() {
        if(hasCharacterSelection && currentCharacterObject) {
            mouseHorizontal -= Input.GetAxis("Mouse X") * characterSelectionRotateSpeed * Time.deltaTime;
            currentCharacterObject.transform.rotation = Quaternion.Euler(0, mouseHorizontal, 0);
        }
    }

    void ChangeToWorldMapSelection() {
        GlobalData.characterDetails = currentCharacterSelection;
        if(SceneManager.instance) {
            SceneManager.instance.ChangeSceneByFading("worldmapselection");
        }
    }

    void LeaveFromCharacterSelection() {
        SceneManager.instance.ChangeSceneByFading("mainmenu");
    }

    void ChangeCinemachineCamera(string selectStatus) {
        if(selectStatus == "select") {
            originalCinemachineObject.SetActive(false);
            selectionCinemachineObject.SetActive(true);
        } else if(selectStatus == "unselect") {
            selectionCinemachineObject.SetActive(false);
            originalCinemachineObject.SetActive(true);
        }
    }
    
    void ShowCharacter() {
        if(currentCharacterObject) currentCharacterObject.SetActive(true);
        selectButton.gameObject.SetActive(true);
    }

    void HideCharacter() {
        if(currentCharacterObject) currentCharacterObject.SetActive(false);
        selectButton.gameObject.SetActive(false);
    }
}