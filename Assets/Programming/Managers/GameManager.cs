using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[Header("Player Character")]
	public PlayerCharacter currentPlayer = PlayerCharacter.MasterKnight;
	public GameObject playerGameObject;
	public GameObject []playerCharacters;

	[Header("Character Lists")]
	public List<Friend> soldierList;
	public List<Person> personList;
	public List<Enemy> enemyList;

	[Header("Character Destinations")]
	public Transform soldierDestination;
	public Transform personDestination;
	public Transform enemyDestination;

	[Header("Game Manager Settings")]
	public bool isPaused = false;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(this.gameObject);
		}

		SetPlayer();
	}

	// Use this for initialization
	void Start () {
		HideCursor();
	}
	
	// Update is called once per frame
	void Update () {
		PauseOrResume();
	}

	void PauseOrResume() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(UIManager.instance.quitPanel.activeSelf == false) {
				ToggleCursor();
				if(isPaused == false) {
					PauseGame();
				} else {
					ResumeGame();
				}
			} else {
				UIManager.instance.HideQuitPanel();
			}
		}
	}
	public void PauseGame() {
		isPaused = true;
		UIManager.instance.ShowPausePanel();
		ShowCursor();
	}
	public void ResumeGame() {
		isPaused = false;
		UIManager.instance.HidePausePanel();
		HideCursor();
	}

	void ToggleCursor() {
		if(Cursor.visible) {
			HideCursor();
		} else {
			ShowCursor();
		}
	}
	void ShowCursor() {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	void HideCursor() {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void SetPlayer() {
		currentPlayer = CharacterSelectController.instance.characterDetails.character;

		GameObject.Find("MasterKnight").GetComponent<MasterKnight>().enabled = currentPlayer == PlayerCharacter.MasterKnight ? true : false;
		GameObject.Find("Hero").GetComponent<Hero>().enabled = currentPlayer == PlayerCharacter.Hero ? true : false;

		playerGameObject = currentPlayer == PlayerCharacter.MasterKnight ? playerCharacters[0] : playerCharacters[1] /*GameObject.Find(currentPlayer.ToString())*/;
	}
}