using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

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
    }
}