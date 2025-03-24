using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public PlayerCharacter currentPlayer = PlayerCharacter.MasterKnight;
	public GameObject playerGameObject;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		HideCursor();
		SetPlayer();
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

		playerGameObject = GameObject.Find(currentPlayer.ToString());
	}
}