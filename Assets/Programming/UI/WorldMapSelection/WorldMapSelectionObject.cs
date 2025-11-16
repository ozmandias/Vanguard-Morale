using UnityEngine;

public class WorldMapSelectionObject : MonoBehaviour {
    SpriteRenderer mapRenderer;
    Color originalColor = new Color(1f, 1f, 1f, 1f);
    Color focusColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public string mapName;

    void Start() {
        mapRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        mapRenderer.color = focusColor;
    }

    void OnMouseUp() {
        mapRenderer.color = originalColor;
        if(SceneManager.instance) {
            SceneManager.instance.ChangeSceneByLoading(mapName);
        }
    }

    void OnMouseEnter() {
        WorldMapSelectionManager.instance.OnMouseEnterMap(mapName);
    }

    void OnMouseExit() {
        WorldMapSelectionManager.instance.OnMouseExitMap(mapName);
    }
}