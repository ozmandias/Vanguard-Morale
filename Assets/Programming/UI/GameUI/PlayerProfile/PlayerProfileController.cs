using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileController : MonoBehaviour {
    public static PlayerProfileController instance;

    public GameObject playerContainer;
    public Slider healthBar;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void SetProfilePicture(Sprite profileSprite) {
        playerContainer.transform.Find("Image").GetComponent<Image>().sprite = profileSprite;
    }

    public void SetHealth(int healthAmount) {
        healthBar.value = healthAmount;
    }
}