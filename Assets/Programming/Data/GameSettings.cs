using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;
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

        Application.targetFrameRate = 60;
        Physics.gravity = new Vector3(0, -9.81f * gravityIntensity, 0);
    }
}