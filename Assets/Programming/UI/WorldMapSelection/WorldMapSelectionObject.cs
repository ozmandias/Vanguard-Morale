using UnityEngine;

public class WorldMapSelectionObject : MonoBehaviour {
    SpriteRenderer mapRenderer;
    public string mapName;
    public Vector2 []mapPoints;
    Color originalColor = new Color(1f, 1f, 1f, 1f);
    Color focusColor = new Color(0.5f, 0.5f, 0.5f, 1);

    void Start() {
        mapRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        mapRenderer.color = focusColor;
    }

    void OnMouseUp() {
        mapRenderer.color = originalColor;
        WorldMapSelectionManager.instance.SetMapData(this);
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