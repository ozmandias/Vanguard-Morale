using UnityEngine;
using UnityEngine.UI;

public class WorldMapSelectionManager : MonoBehaviour {
    public string mapName;
    public Image mapImage;
    public Button backButton;
    [SerializeField] WorldScriptableObject worldScriptableObject;
    Color originalColor = new Color(1f, 1f, 1f, 1f);
    Color fadeColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    public static WorldMapSelectionManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        backButton.onClick.AddListener(LeaveFromWorldMapSelection);
    }

    void Update() {
        
    }

    void SelectMap() {
        // Add Polygon Collider to WorldMapSelectionObject.
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D selectMapRaycastHit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if(selectMapRaycastHit.collider != null) {
                Debug.Log("hit: " + selectMapRaycastHit.collider.gameObject.name);
            }
        }
    }

    void LeaveFromWorldMapSelection() {
        if(SceneManager.instance) {
            SceneManager.instance.ChangeSceneByFading("characterselection");
        }
    }

    public void SetMapData(WorldMapSelectionObject mapSelectionObject) {
        KingdomSerializable kingdomDetails = worldScriptableObject.dataList.Find((kingdom)=>{
            return kingdom.mapName == mapSelectionObject.mapName;
        });
        if(kingdomDetails != null) {
            GlobalData.kingdomDetails = kingdomDetails;
            GlobalData.currentKingdomQuestScriptableObject = kingdomDetails.questScriptableObject;
        }
    }

    public void OnMouseEnterMap(string mapName) {
        mapImage.color = fadeColor;
    }

    public void OnMouseExitMap(string mapName) {
        mapImage.color = originalColor;
    }
}