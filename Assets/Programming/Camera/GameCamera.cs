using UnityEngine;

public class GameCamera : MonoBehaviour {
    float mouseHorizontal;
    float mouseVertical;
    Vector3 followDirection;
    [SerializeField] float cameraSpeed = 4f;
    [SerializeField] float distance = 18f /*-18f*/;
    [SerializeField] float cameraLagSpeed = 10f;
    [SerializeField] GameObject cameraFollowObject;

    [Header("Check Camera Blocking Settings")]
    [SerializeField] LayerMask raycastLayers;
    [SerializeField] float raycastRadius = 0.5f;
    [SerializeField] float minimumDistanceFromObstacles = 0.8f /*-0.8f*/ /*-0.1f*/;
    [SerializeField] float smoothingFactor = 25f;
    [SerializeField] bool somethingBlocking = false;
    [SerializeField] float currentDistance;

    void Start() {
        cameraFollowObject = GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? GameObject.Find("Vanguard").transform.Find("CameraFollow").gameObject : GameObject.Find("Hero").transform.Find("CameraFollow").gameObject;
        if(PlayerManager.instance != null) cameraFollowObject = PlayerManager.instance.currentCharacter.personalData.cameraFollowObject;
        // distance = Vector3.Distance(cameraFollowObject.transform.position, gameObject.transform.position);

        raycastLayers = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));

        currentDistance = distance /*(cameraFollowObject.transform.position - transform.position).magnitude*/;
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

        CheckCameraBlocking();

        // followDirection = cameraFollowObject.transform.position - (gameObject.transform.forward * distance); // + (gameObject.transform.forward * distance)

        // CameraRotate();

        if(somethingBlocking == false) {
            followDirection = cameraFollowObject.transform.position - (transform.forward * distance);
        } else {
            followDirection = cameraFollowObject.transform.position - ((transform.forward * distance).normalized * currentDistance);
        }

        gameObject.transform.position = followDirection; // Vector3.Lerp(gameObject.transform.position, followDirection, Time.deltaTime * cameraLagSpeed) // No need to add camera lag.
    }

    void CameraRotate() {
        gameObject.transform.rotation = Quaternion.Euler(mouseVertical, mouseHorizontal, 0);
    }

    void CheckCameraBlocking() {
        // fix manually by reading forums without using AI
        // fix in the way of followDirection;
        Vector3 raycastDirection =  transform.position - cameraFollowObject.transform.position /* cameraFollowObject.transform.position - transform.position */;
        float raycastDistance = distance /* raycastDirection.magnitude + minimumDistanceFromObstacles */;
        float newDistance = raycastDirection.magnitude;

        RaycastHit hit;
        // reference AI's programming and needs to move camera to position of collider that was hit.
        // Physics.SphereCast(new Ray(transform.position, raycastDirection), raycastRadius, out hit, raycastDistance, raycastLayers)
        if(Physics.SphereCast(cameraFollowObject.transform.position, raycastRadius, raycastDirection, out hit, raycastDistance, raycastLayers, QueryTriggerInteraction.Ignore)) {
            somethingBlocking = true;
            // newDistance = Mathf.Max(0.5f, hit.distance - minimumDistanceFromObstacles); // returns largest maximum of two comparing values;
            newDistance = Mathf.Clamp(hit.distance, 0.5f, distance);
        } else {
            somethingBlocking = false;
        }
        
        currentDistance = Mathf.Lerp(currentDistance, newDistance, Time.deltaTime * smoothingFactor);
    }

    /*void CameraMove() {
        mouseHorizontal += Input.GetAxis("Mouse X") * cameraSpeed;
        mouseVertical -= Input.GetAxis("Mouse Y") * cameraSpeed;
        mouseVertical = Mathf.Clamp(mouseVertical, -15, 55);

        CameraRotate();

        // 1. Calculate the ideal (furthest) position
        Vector3 cameraDirection = cameraFollowObject.transform.position - (transform.forward * distance); // + (transform.forward * distance)

        // 2. Check for obstacles between the target and the desired camera position
        RaycastHit hit;
        Vector3 cameraCheckDirection = (cameraDirection - cameraFollowObject.transform.position).normalized;
        float maxDist = Mathf.Abs(distance); // returns absolute value; Example - Mathf.Abs(-10f) returns 10f

        // We use SphereCast instead of Raycast to give the camera "thickness" 
        // so you don't see through walls at edges.
        if (Physics.SphereCast(cameraFollowObject.transform.position, raycastRadius, cameraCheckDirection, out hit, maxDist, raycastLayers))
        {
            // If we hit something, set position to hit point (pulled back slightly by radius)
            somethingBlocking = true;
            float minDist = 0.5f;
            float currentDistance = Mathf.Clamp(hit.distance, minDist, maxDist);
            followDirection = cameraFollowObject.transform.position + (cameraCheckDirection * currentDistance);
        }
        else
        {
            // No obstacle, use the full distance
            somethingBlocking = false;
            followDirection = cameraDirection;
        }

        // 3. Apply position (Lerp adds a nice "softness" to the collision movement)
        transform.position = Vector3.Lerp(transform.position, followDirection, Time.deltaTime * cameraLagSpeed);
    }*/

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, raycastRadius);
    }
}