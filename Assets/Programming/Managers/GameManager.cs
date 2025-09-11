using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[Header("Player Character")]
	public PlayerCharacter currentPlayer = PlayerCharacter.MasterKnight; // for checking
	public GameObject playerGameObject; // for value
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

		GameObject masterKnightObject = null;
		GameObject heroObject = null;
		if (playerCharacters.Length > 0)
		{
			foreach (GameObject playerCharacter in playerCharacters)
			{
				if (playerCharacter.name == PlayerCharacter.MasterKnight.ToString())
				{
					// playerCharacters[0]/*GameObject.Find("MasterKnight")*/
					playerCharacter.GetComponent<MasterKnight>().enabled = currentPlayer == PlayerCharacter.MasterKnight ? true : false;
					masterKnightObject = playerCharacter;
				}
				if (playerCharacter.name == PlayerCharacter.Hero.ToString())
				{
					// playerCharacters[1]/*GameObject.Find("Hero")*/
					playerCharacter.GetComponent<Hero>().enabled = currentPlayer == PlayerCharacter.Hero ? true : false;
					heroObject = playerCharacter;
				}
			}

			playerGameObject = currentPlayer == PlayerCharacter.MasterKnight ? masterKnightObject : heroObject /*GameObject.Find(currentPlayer.ToString())*/;
		}
	}
}