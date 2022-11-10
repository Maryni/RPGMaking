using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeUIComponent
{
    None,
    GameObjectLevel,
    GameObjectHealthSlider,
    GameObjectStaminaSlider,
    GameObjectExperienceSlider,
    CurrentTextHealth,
    CurrentTextStamina,
    CurrentTextLevel,
    CurrentTextExperience,
    MaximumTextHealth,
    MaximumTextStamina,
    MaximumTextExperience,
    CurrentTextStrength,
    CurrentTextDexterity,
    CurrentTextVitality,
    CurrentTextIntelligence,
    CurrentTextWisdom
}

public enum TypeUIButtons
{
    None,
    StrengthPlus,
    StrengthMinus,
    DexterityPlus,
    DexterityMinus,
    VitalityPlus,
    VitalityMinus,
    IntelligencePlus,
    IntelligenceMinus,
    WisdomPlus,
    WisdomMinus
}

public class UIManager : MonoBehaviour
{
    #region Inspector variables

    [SerializeField, Header("Health")] private GameObject gameObjectHealthSlider;
    [SerializeField] private Text textCurrentHealth;
    [SerializeField] private Text textMaxHealth;
    [SerializeField, Header("Stamina")] private GameObject gameObjectStaminaSlider;
    [SerializeField] private Text textCurrentStamina;
    [SerializeField] private Text textMaxStamina;
    [SerializeField, Header("Level")] private GameObject gameObjectLevel;
    [SerializeField] private Text textCurrentLevel;
    [SerializeField, Header("Experience")] private GameObject gameObjectExperienceSlider;
    [SerializeField] private Text textCurrentExperience;
    [SerializeField] private Text textMaxExperience;
    [SerializeField, Header("Stats")] private Text textStatStrenght;
    [SerializeField] private Text textStatDexterity;
    [SerializeField] private Text textStatVitality;
    [SerializeField] private Text textStatIntelligence;
    [SerializeField] private Text textStatWisdom;
    [SerializeField,Header("Buttons")] private List<Button> listPlusButtons = new List<Button>(); //better to change on other variant
    [SerializeField] private List<Button> listMinusButtons = new List<Button>();

    #endregion Inspector variables
    
    #region private variables

    private Dictionary<TypeUIComponent, GameObject> uiDictionary = new Dictionary<TypeUIComponent, GameObject>();
    private Dictionary<TypeUIButtons, Button> uiButtonsDictionary = new Dictionary<TypeUIButtons, Button>();

    #endregion private variables

    #region Unity functions

    private void OnEnable()
    {
        SetObjectsToDictionary();
    }

    #endregion Unity functions

    #region public functions

    public GameObject GetComponentFromDictionary(TypeUIComponent component)
    {
        return uiDictionary[component];
    }

    public Button GetComponentFromDictionary(TypeUIButtons button)
    {
        return uiButtonsDictionary[button];
    }
    
    public void RemoveAllListenersFromButtons()
    {
        uiButtonsDictionary[TypeUIButtons.StrengthPlus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.DexterityPlus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.VitalityPlus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.IntelligencePlus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.WisdomPlus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.StrengthMinus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.DexterityMinus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.VitalityMinus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.IntelligenceMinus].onClick.RemoveAllListeners();
        uiButtonsDictionary[TypeUIButtons.WisdomMinus].onClick.RemoveAllListeners();
    }

    #endregion public functions
    
    #region private functions

    private void SetObjectsToDictionary()
    {
        uiDictionary.Add(TypeUIComponent.GameObjectLevel, gameObjectLevel);
        uiDictionary.Add(TypeUIComponent.GameObjectHealthSlider, gameObjectHealthSlider);
        uiDictionary.Add(TypeUIComponent.GameObjectStaminaSlider, gameObjectStaminaSlider);
        uiDictionary.Add(TypeUIComponent.GameObjectExperienceSlider, gameObjectExperienceSlider);
        uiDictionary.Add(TypeUIComponent.CurrentTextHealth, textCurrentHealth.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextStamina, textCurrentStamina.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextLevel, textCurrentLevel.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextExperience, textCurrentExperience.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextHealth, textMaxHealth.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextStamina, textMaxStamina.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextExperience, textMaxExperience.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextStrength, textStatStrenght.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextDexterity, textStatDexterity.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextVitality, textStatVitality.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextIntelligence, textStatIntelligence.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextWisdom, textStatWisdom.gameObject);
        
        if (listPlusButtons.Count > 0 && listMinusButtons.Count > 0)
        {
            uiButtonsDictionary.Add(TypeUIButtons.StrengthPlus, listPlusButtons[0]);
            uiButtonsDictionary.Add(TypeUIButtons.DexterityPlus, listPlusButtons[1]);
            uiButtonsDictionary.Add(TypeUIButtons.VitalityPlus, listPlusButtons[2]);
            uiButtonsDictionary.Add(TypeUIButtons.IntelligencePlus, listPlusButtons[3]);
            uiButtonsDictionary.Add(TypeUIButtons.WisdomPlus, listPlusButtons[4]);
            uiButtonsDictionary.Add(TypeUIButtons.StrengthMinus, listMinusButtons[0]);
            uiButtonsDictionary.Add(TypeUIButtons.DexterityMinus, listMinusButtons[1]);
            uiButtonsDictionary.Add(TypeUIButtons.VitalityMinus, listMinusButtons[2]);
            uiButtonsDictionary.Add(TypeUIButtons.IntelligenceMinus, listMinusButtons[3]);
            uiButtonsDictionary.Add(TypeUIButtons.WisdomMinus, listMinusButtons[4]);  
        }
        else
        {
            Debug.LogWarning($"All values from listPlusButtons and listMinusButtons are gone");
        }

    }

    #endregion private functions


    
}
