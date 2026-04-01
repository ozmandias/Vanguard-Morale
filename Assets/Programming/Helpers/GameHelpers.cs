using UnityEngine;

public static class GameHelpers {
    public static GameObject FindGameObjectInChildren(string search, GameObject parent) {
        if(parent.name == search) {
            return parent;
        }

        for(int i = 0; i < parent.transform.childCount; i = i + 1) {
            GameObject searchObject = FindGameObjectInChildren(search, parent.transform.GetChild(i).gameObject);
            if(searchObject) {
                return searchObject;
            }
        }

        return null;
    }

    public static GameObject FindWithTagInChildren(string tag, GameObject parent) {
        if(parent.CompareTag(tag)) {
            return parent;
        }

        for(int i = 0; i < parent.transform.childCount; i = i + 1) {
            GameObject searchObject = FindWithTagInChildren(tag, parent.transform.GetChild(i).gameObject);
            if(searchObject) {
                return searchObject;
            }
        }

        return null;
    }

    public static CharacterInfo GetCharacterInfo(GameObject character) {
        CharacterInfo characterInfo = null;
        if(character.CompareTag("Player")) {
            characterInfo = GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? character.GetComponent<Vanguard>().GetInfo() as CharacterInfo : character.GetComponent<Player>().GetInfo() as CharacterInfo;
        } else if(character.CompareTag("Person") || character.CompareTag("Boss")) {
            characterInfo = character.GetComponent<Person>().GetInfo() as CharacterInfo;
        }
        return characterInfo;
    }

    public static CombatManager GetCharacterCombat(GameObject character) {
        CombatManager characterCombat = character.GetComponent<CombatManager>();
        return characterCombat;
    }

    public static UIChanger GetUIChanger(string sceneName) {
        UIChanger uiChanger = null;
        switch(sceneName) {
            case "mainmenu":
                if(MainMenuManager.instance) uiChanger = MainMenuManager.instance.GetComponent<UIChanger>();
                break;
            case "game":
            case "KingdomOfFortis":
            case "vs":
            case "test":
            case "lab":
                if(UIManager.instance) uiChanger = UIManager.instance.GetComponent<UIChanger>();
                break;
            case "characterselection":
                if(CharacterSelectionManager.instance) uiChanger = CharacterSelectionManager.instance.GetComponent<UIChanger>();
                break;
            case "worldmapselection":
                if(WorldMapSelectionManager.instance) uiChanger = WorldMapSelectionManager.instance.GetComponent<UIChanger>();
                break;
            default:
                break;
        }
        return uiChanger;
    }
}