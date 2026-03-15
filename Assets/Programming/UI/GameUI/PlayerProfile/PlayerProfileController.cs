using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileController : MonoBehaviour
{
    public static PlayerProfileController instance;

    public GameObject playerContainer;
    public Slider healthBar;
    public Text damageText;
    public Text moralityText;

    #region delegates
    public delegate void HealthChangesDelegate(int healthAmount);
    public HealthChangesDelegate OnHealthChanges;

    public delegate void DamageChangesDelegate(int playerDamage);
    public DamageChangesDelegate OnDamageChanges;

    public delegate void MoralityChangesDelegate(Morality playerMorality);
    public MoralityChangesDelegate OnMoralityChanges;
    #endregion


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
    }

    void Start()
    {
        CharacterModel characterData = GlobalData.characterDetails;
        SetProfilePicture(characterData.profileSprite);

        OnHealthChanges += SetHealth;
        OnDamageChanges += SetDamage;
        OnMoralityChanges += SetMorality;
    }

    public void SetProfilePicture(Sprite profileSprite)
    {
        playerContainer.transform.Find("Image").GetComponent<Image>().sprite = profileSprite;
    }

    public void SetHealth(int healthAmount)
    {
        healthBar.value = healthAmount;
    }

    public void SetDamage(int playerDamage)
    {
        damageText.text = "Damage: " + playerDamage;
    }

    public void SetMorality(Morality playerMorality)
    {
        moralityText.text = "Morality: " + playerMorality;
    }
}