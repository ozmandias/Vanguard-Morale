using UnityEngine;

public class GameCamera : MonoBehaviour {
    float mouseHorizontal;
    float mouseVertical;
    Vector3 followDirection;
    [SerializeField] float cameraSpeed = 4f;
    [SerializeField] float distance = -18f;
    [SerializeField] GameObject cameraFollowObject;

    void Start() {
        cameraFollowObject = GameObject.Find("CameraFollow");
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

        followDirection = cameraFollowObject.transform.position + (transform.forward * distance);

        CameraRotate();

        gameObject.transform.position = followDirection;
    }

    void CameraRotate() {
        gameObject.transform.rotation = Quaternion.Euler(mouseVertical, mouseHorizontal, 0);
    }
}