using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    
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
            SceneManager.instance.ChangeSceneByLoading("mainmenu");
        }
    #endregion
}