using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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
}