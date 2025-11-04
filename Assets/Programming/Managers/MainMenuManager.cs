using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public static MainMenuManager instance;

    [Header("MainMenu UI")]
    public GameObject mainMenuPanel;
    public GameObject characterSelectPanel;
    public GameObject fadePanel;
    public GameObject exitPanel;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    #region MainMenuUI
        public void PlayStory() {
            SceneManager.instance.ChangeSceneByFading("characterselection");
        }
        
        public void PlayArena() {
            SceneManager.instance.ChangeSceneByLoading("vs");
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
}