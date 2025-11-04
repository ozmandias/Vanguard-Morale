using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour {
    public UIType uiType;
    public bool hasUIChanges = false;

    public delegate void UIChangerStartDelegate();
    public UIChangerStartDelegate OnUIChangerStartDelegate;

    void Start() {
        hasUIChanges = true;
        if(OnUIChangerStartDelegate != null) {
            OnUIChangerStartDelegate.Invoke();
        }
    }

    void Update() {
        if(hasUIChanges == true) {
            MakeUIChanges();
            hasUIChanges = false;
        }
    }

    void MakeUIChanges() {
        // uiBase means parent container for other UIs. Do not hide uiBases.
        GameObject uiBase = gameObject;
        for(int i = 0; i < uiBase.transform.childCount; i = i + 1) {
            UIObject uiObject = uiBase.transform.GetChild(i).GetComponent<UIObject>();
            uiObject.gameObject.SetActive(uiObject.autoDisplay);
        }
    }

    public void ChangeUIType(UIType _uiType) {
        uiType = _uiType;
        // hasUIChanges = true;
    }

    public void HideUIs() {
        GameObject uiBase = gameObject;
        foreach(Transform uiChild in uiBase.transform) {
            uiChild.gameObject.SetActive(false);
        }
    }

    public void StartUIChanging() {
        hasUIChanges = true;
    }

    public void StopUIChanging() {
        hasUIChanges = false;
        HideUIs();
    }
}