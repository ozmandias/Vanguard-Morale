using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[Header("Player Character")]
	public PlayerCharacter currentPlayer = PlayerCharacter.Vanguard; // for checking
	public GameObject playerGameObject; // for value
	public GameObject []playerCharacters;

	[Header("Character Lists")]
	// switch with faction lists
	public List<Person> friendList;
	public List<Person> companionList;
	public List<Person> personList;
	public List<Person> enemyList;
	public List<Person> bossList;

	[Header("Character Destinations")]
	public Transform friendDestination;
	public Transform personDestination;
	public Transform enemyDestination;

	[Header("Game Manager Settings")]
	public bool isPaused = false;

	public PlayerReadyEvent OnPlayerReady = new PlayerReadyEvent();
	public WarReadyEvent OnWarReady = new WarReadyEvent();

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

		OnPlayerReady.AddListener((playerCharacter, playerGameObject) => PlayerReady(playerCharacter, playerGameObject));
		OnWarReady.AddListener((friendDestination, enemyDestination) => WarReady(friendDestination, enemyDestination));
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
		GameObject playerObject = null;
		
		if (playerCharacters.Length > 0)
		{
			foreach (GameObject playerCharacter in playerCharacters)
			{
				if (playerCharacter.GetComponent<Character>().playerCharacter == PlayerCharacter.Vanguard)
				{
					playerCharacter.GetComponent<Vanguard>().enabled = currentPlayer == PlayerCharacter.Vanguard ? true : false;
					vanguardObject = playerCharacter;
				} else {
					playerCharacter.GetComponent<Player>().enabled = currentPlayer == PlayerCharacter.Player ? true : false;
					playerObject = playerCharacter;
				}
			}

			playerGameObject = currentPlayer == PlayerCharacter.Vanguard ? vanguardObject : playerObject;
		}
	}

	public void PlayerReady(PlayerCharacter playerCharacter, GameObject playerGameObject) {
		// assign data from PlayerManager
		this.currentPlayer = playerCharacter;
		this.playerGameObject = playerGameObject;
	}

	public void WarReady(Transform friendDestination, Transform enemyDestination) {
		this.friendDestination = friendDestination;
		this.enemyDestination = enemyDestination;
	}
}