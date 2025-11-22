using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailsController : MonoBehaviour {
    public Text characterIdentityText;
    public Text characterNameText;
    public Text characterDescriptionText;
    public Text strengthText;
    public Text agilityText;
    public Text intelligenceText;
    public Text healthText;
    public Text damageText;
    public Text magicText;

    public static CharacterDetailsController instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        characterNameText = characterNameText.transform.GetChild(0).GetComponent<Text>();
        characterDescriptionText = characterDescriptionText.transform.GetChild(0).GetComponent<Text>();
        strengthText = strengthText.transform.GetChild(0).GetComponent<Text>();
        agilityText = agilityText.transform.GetChild(0).GetComponent<Text>();
        intelligenceText = intelligenceText.transform.GetChild(0).GetComponent<Text>();
        healthText = healthText.transform.GetChild(0).GetComponent<Text>();
        damageText = damageText.transform.GetChild(0).GetComponent<Text>();
        magicText = magicText.transform.GetChild(0).GetComponent<Text>();
    }

    public void SetCharacterDetails(CharacterSerializable characterDetails) {
        characterIdentityText.text = characterDetails.characterIdentity;
        characterNameText.text = characterDetails.characterName;
        characterDescriptionText.text = characterDetails.characterDescription;
        strengthText.text = characterDetails.characterStats.strength.ToString();
        agilityText.text = characterDetails.characterStats.agility.ToString();
        intelligenceText.text = characterDetails.characterStats.intelligence.ToString();
        healthText.text = characterDetails.characterStats.health.ToString();
        damageText.text = characterDetails.characterStats.damage.ToString();
        magicText.text = characterDetails.characterStats.magic.ToString();
    }

    public void ClearCharacterDetails() {
        characterIdentityText.text = "";
        characterNameText.text = "";
        characterDescriptionText.text = "";
        strengthText.text = "";
        agilityText.text = "";
        intelligenceText.text = "";
        healthText.text = "";
        damageText.text = "";
        magicText.text = "";
    }
}