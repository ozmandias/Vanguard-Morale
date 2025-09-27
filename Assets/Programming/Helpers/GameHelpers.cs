using UnityEngine;

public static class GameHelpers {
    public static GameObject FindGameObjectInChildren(string search, GameObject parent) {
        if(parent.name == search) {
            Debug.Log("Found " + search);
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
}