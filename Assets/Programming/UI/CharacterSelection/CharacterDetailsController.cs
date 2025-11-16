using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailsController : MonoBehaviour {
    public Text characterNameText;
    public Text healthText;
    public Text damageText;

    public static CharacterDetailsController instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void SetCharacterDetails(CharacterSerializable characterDetails) {
        characterNameText.text = characterDetails.characterName;
        healthText.transform.GetChild(0).GetComponent<Text>().text = characterDetails.health.ToString();
        damageText.transform.GetChild(0).GetComponent<Text>().text = characterDetails.damage.ToString();
    }

    public void ClearCharacterDetails() {
        characterNameText.text = "";
        healthText.transform.GetChild(0).GetComponent<Text>().text = "";
        damageText.transform.GetChild(0).GetComponent<Text>().text = "";
    }
}