using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public GameObject mainMenuUI;
    public GameObject gameUI;
    public List<GameObject> uiList;

    public UIType uiType; 
    GameObject currentUI;

    public bool hasUIChanges = false;

    [Header("MainMenu  UI")]
    public GameObject mainMenuPanel;
    public GameObject characterSelectPanel;
    public GameObject fadePanel;
    public GameObject exitPanel;
    
    [Header("Game UI")]
    public GameObject pausePanel;
    public GameObject quitPanel;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        DontDestroyOnLoad(this.gameObject);
        foreach(Transform child in gameObject.transform) {
            uiList.Add(child.gameObject);
        }
        hasUIChanges = true;
    }

    void Update() {
        if(hasUIChanges == true) {
            MakeUIChanges();
            hasUIChanges = false;
        }
    }

    void MakeUIChanges() {
        switch(uiType) {
            case UIType.MainMenu:
                currentUI = mainMenuUI;
                break;
            case UIType.Game:
                currentUI = gameUI;
                break;
            default:
                currentUI = null;
                break;
        }

        foreach(GameObject uiBase in uiList) {
            if(uiBase != currentUI) {
                for(int i = 0; i < uiBase.transform.childCount; i = i + 1) {
                    uiBase.transform.GetChild(i).gameObject.SetActive(false);
                }
            } else {
                for(int i = 0; i < uiBase.transform.childCount; i = i + 1) {
                    UIObject uiObject = uiBase.transform.GetChild(i).GetComponent<UIObject>();
                    uiObject.gameObject.SetActive(uiObject.autoDisplay);
                }
            }
        }
    }

    public void ChangeUIType(UIType _uiType) {
        uiType = _uiType;
        hasUIChanges = true;
    }

    #region MainMenuUI
        public void PlayStory() {
            ChangeUIType(UIType.Game);
            SceneManager.instance.ChangeScene("game");
        }
        
        public void PlayArena() {
            ChangeUIType(UIType.Game);
            SceneManager.instance.ChangeScene("vs");
        }


        public void ShowMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
    }
        public void HideMainMenuPanel() {
            mainMenuPanel.SetActive(false);
        }


        public void ShowCharacterSelectPanel() {
            mainMenuPanel.SetActive(false);
            characterSelectPanel.SetActive(true);
        }
        public void HideCharacterSelectPanel() {
            mainMenuPanel.SetActive(true);
            characterSelectPanel.SetActive(false);
        }


        public void ShowExitPanel() {
            fadePanel.SetActive(true);
            exitPanel.SetActive(true);
        }
        public void HideExitPanel() {
            fadePanel.SetActive(false);
            exitPanel.SetActive(false);
        }


        public void ExitGame() {
            Application.Quit();
        }
    #endregion

    #region GameUI
        public void ResumeGameplay() {
            GameManager.instance.ResumeGame();
        }


        public void ShowPausePanel() {
            pausePanel.SetActive(true);
        }
        public void HidePausePanel() {
            pausePanel.SetActive(false);
        }


        public void ShowQuitPanel() {
            pausePanel.SetActive(false);
            quitPanel.SetActive(true);
        }
        public void HideQuitPanel() {
            pausePanel.SetActive(true);
            quitPanel.SetActive(false);
        }


        public void QuitGameplay() {
            ChangeUIType(UIType.MainMenu);
            SceneManager.instance.ChangeScene("mainmenu");
        }
    #endregion
}