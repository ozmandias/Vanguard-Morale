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
		ToggleCursor();
	}

	void ToggleCursor() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(Cursor.visible) {
				HideCursor();
			} else {
				ShowCursor();
			}
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
		GameObject.Find("MasterKnight").GetComponent<MasterKnight>().enabled = currentPlayer == PlayerCharacter.MasterKnight ? true : false;
		GameObject.Find("Hero").GetComponent<Hero>().enabled = currentPlayer == PlayerCharacter.Hero ? true : false;

		playerGameObject = currentPlayer == PlayerCharacter.MasterKnight ? playerCharacters[0] : playerCharacters[1] /*GameObject.Find(currentPlayer.ToString())*/;
	}
}