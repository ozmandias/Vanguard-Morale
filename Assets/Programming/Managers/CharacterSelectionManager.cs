using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterSelectionManager : MonoBehaviour {
    Camera characterSelectionCamera;
    float mouseHorizontal = -90f /*0f*/;
    [Header("Character Selection Manager Settings")]
    public float characterSelectionRotateSpeed = 8f;
    public Button backButton;
    public Button selectButton;
    public bool hasCharacterSelection = false;
    public CharacterSerializable currentCharacterSelection;
    public GameObject currentCharacterObject;
    public Transform cameraOriginalTransform;
    public Transform characterFocusTransform;
    public GameObject originalCinemachineObject;
    public GameObject selectionCinemachineObject;
    public Effect characterSelectionEffect;
    [Header("UI")]
    public GameObject characterDetailsUIObject;
    public GameObject characterSelectionGroupUIObject;
    [Header("Data")]
    public CharacterScriptableObject characterScriptableObject;
    public CharacterSelectionObject []characterSelectionObjects;
    public GameObject characterSelectionPrefab;
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
        foreach(CharacterSelectionObject characterSelectionObject in characterSelectionObjects) {
            if(characterSelectionObject.characterObject != null) {
                characterSelectionObject.characterObject.transform.DOScale(Vector3.zero, 0);
                characterSelectionObject.characterObject.SetActive(true);
            }
        }

        selectButton.onClick.AddListener(ChangeToWorldMapSelection);
        backButton.onClick.AddListener(LeaveFromCharacterSelection);

        OnCharacterSelectDelegate += ChangeCinemachineCamera;
    }

    void Update() {
        RotateCharacter();
    }

    void LoadCharacterData() {
        foreach(CharacterSerializable characterData in characterScriptableObject.dataList) {
            CharacterSelectionObject newCharacterSelectionObject = Instantiate(characterSelectionPrefab, characterSelectionGroupUIObject.transform).GetComponent<CharacterSelectionObject>();
            newCharacterSelectionObject.characterSelection = characterData;
        }
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
        characterSelectionEffect.Play();
        if(currentCharacterObject) {
            // currentCharacterObject.SetActive(true);
            // use DoTween to scale
            currentCharacterObject.transform.DOScale(
                Array.Find(characterSelectionObjects, (characterSelectionObject)=>{
                    return characterSelectionObject.characterObject == currentCharacterObject;
                })
                .characterOriginalScale,
                1f
            ).OnUpdate(()=>{
            }).OnComplete(()=>{
            });
        }
        selectButton.gameObject.SetActive(true);
        CharacterDetailsController.instance.SetCharacterDetails(currentCharacterSelection);
        characterDetailsUIObject.SetActive(true);
    }

    void HideCharacter() {
        characterSelectionEffect.Play() /*Stop()*/;
        if(currentCharacterObject) {
            // currentCharacterObject.SetActive(false);
            // use DoTween to scale
            currentCharacterObject.transform.DOKill();
            currentCharacterObject.transform.DOScale(
                Vector3.zero,
                hasCharacterSelection == false ? 1f : 0
            ).OnComplete(()=>{
            });
        }
        selectButton.gameObject.SetActive(false);
        CharacterDetailsController.instance.ClearCharacterDetails();
        characterDetailsUIObject.SetActive(false);
    }
}