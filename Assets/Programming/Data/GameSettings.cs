using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;
    public const float DEFAULT_GRAVITY = -9.81f;
    public float gravity = 0;
    public float gravityIntensity = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (gravity > 0)
        {
            gravity *= -1f;
        }
        else
        {
            gravity = DEFAULT_GRAVITY;
        }

        Application.targetFrameRate = 60;
        Physics.gravity = new Vector3(0, gravity * gravityIntensity, 0);
    }
}