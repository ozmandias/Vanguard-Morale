using UnityEngine;

public class GameCamera : MonoBehaviour {
    float mouseHorizontal;
    float mouseVertical;
    Vector3 followDirection;
    [SerializeField] float cameraSpeed = 4f;
    [SerializeField] float distance = -18f;
    [SerializeField] float cameraLagSpeed = 10f;
    [SerializeField] GameObject cameraFollowObject;

    void Start() {
        cameraFollowObject = GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? GameObject.Find("MasterKnight").transform.Find("CameraFollow").gameObject : GameObject.Find("Hero").transform.Find("CameraFollow").gameObject;
    }

    void Update() {
        // CameraMove();
    }

    void LateUpdate() {
        CameraMove();
    }

    void CameraMove() {
        mouseHorizontal += Input.GetAxis("Mouse X") * cameraSpeed;
        mouseVertical -= Input.GetAxis("Mouse Y") * cameraSpeed;
        mouseVertical = Mathf.Clamp(mouseVertical, -15, 55);

        CameraRotate();

        followDirection = cameraFollowObject.transform.position + (transform.forward * distance);

        // CameraRotate();

        gameObject.transform.position = /*Vector3.Lerp(gameObject.transform.position, followDirection, Time.deltaTime * cameraLagSpeed)*/ followDirection; // No need to add camera lag.
    }

    void CameraRotate() {
        gameObject.transform.rotation = Quaternion.Euler(mouseVertical, mouseHorizontal, 0);
    }
}