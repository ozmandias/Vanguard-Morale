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

    public static Info GetCharacterInfo(GameObject character) {
        Info characterInfo = null;
        if(character.CompareTag("Player")) {
            characterInfo = GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? character.GetComponent<MasterKnight>().GetInfo() as Info : character.GetComponent<Player>().GetInfo() as Info;
        } else if(character.CompareTag("Person") || character.CompareTag("Boss")) {
            characterInfo = character.GetComponent<Person>().GetInfo() as Info;
        }
        return characterInfo;
    }
}