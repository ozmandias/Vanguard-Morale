using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[Header("Player Character")]
	public PlayerCharacter currentPlayer = PlayerCharacter.Vanguard; // for checking
	public GameObject playerGameObject; // for value
	public GameObject []playerCharacters;

	[Header("Character Lists")]
	public List<Friend> soldierList;
	public List<Person> personList;
	public List<Enemy> enemyList;
	public List<Boss> bossList;

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
	}

	// Use this for initialization
	void Start () {
		playerCharacters = GameObject.FindGameObjectsWithTag("Player");
		SetPlayer();
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

		if(Input.GetKeyDown(KeyCode.BackQuote)) {
			ToggleCursor();
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
		currentPlayer = GlobalData.characterDetails.character /*CharacterSelectController.instance.characterDetails.character*/;

		GameObject vanguardObject = null;
		GameObject heroObject = null;
		if (playerCharacters.Length > 0)
		{
			foreach (GameObject playerCharacter in playerCharacters)
			{
				if (playerCharacter.name == PlayerCharacter.Vanguard.ToString())
				{
					// playerCharacters[0]/*GameObject.Find("Vanguard")*/
					playerCharacter.GetComponent<Vanguard>().enabled = currentPlayer == PlayerCharacter.Vanguard ? true : false;
					vanguardObject = playerCharacter;
				}
				if (playerCharacter.name == PlayerCharacter.Hero.ToString())
				{
					// playerCharacters[1]/*GameObject.Find("Hero")*/
					playerCharacter.GetComponent<Hero>().enabled = currentPlayer == PlayerCharacter.Hero ? true : false;
					heroObject = playerCharacter;
				}
			}

			playerGameObject = currentPlayer == PlayerCharacter.Vanguard ? vanguardObject : heroObject /*GameObject.Find(currentPlayer.ToString())*/;
		}
	}
}