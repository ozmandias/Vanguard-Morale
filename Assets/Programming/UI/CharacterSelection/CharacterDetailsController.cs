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
    public Image strengthProgress;
    public Image agilityProgress;
    public Image intelligenceProgress;
    public Image healthProgress;
    public Image damageProgress;
    public Image magicProgress;

    public static CharacterDetailsController instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void SetCharacterDetails(CharacterSerializable characterDetails) {
        characterIdentityText.text = characterDetails.characterIdentity;
        characterNameText.text = characterDetails.characterName;
        characterDescriptionText.text = characterDetails.characterDescription;
        strengthText.text = characterDetails.characterStats.strength.ToString();
        strengthProgress.fillAmount = (float) characterDetails.characterStats.strength / 100;
        agilityText.text = characterDetails.characterStats.agility.ToString();
        agilityProgress.fillAmount = (float) characterDetails.characterStats.agility / 100;
        intelligenceText.text = characterDetails.characterStats.intelligence.ToString();
        intelligenceProgress.fillAmount = (float) characterDetails.characterStats.intelligence / 100;
        healthText.text = characterDetails.characterStats.health.ToString();
        healthProgress.fillAmount = (float) characterDetails.characterStats.health / 100;
        damageText.text = characterDetails.characterStats.damage.ToString();
        damageProgress.fillAmount = (float) characterDetails.characterStats.damage / 100;
        magicText.text = characterDetails.characterStats.magic.ToString();
        magicProgress.fillAmount = (float) characterDetails.characterStats.magic / 100;
    }

    public void ClearCharacterDetails() {
        characterIdentityText.text = "";
        characterNameText.text = "";
        characterDescriptionText.text = "";
        strengthText.text = "";
        strengthProgress.fillAmount = 0;
        agilityText.text = "";
        agilityProgress.fillAmount = 0;
        intelligenceText.text = "";
        intelligenceProgress.fillAmount = 0;
        healthText.text = "";
        healthProgress.fillAmount = 0;
        damageText.text = "";
        damageProgress.fillAmount = 0;
        magicText.text = "";
        magicProgress.fillAmount = 0;
    }
}