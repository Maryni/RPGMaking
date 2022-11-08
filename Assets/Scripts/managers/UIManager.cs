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
    MaximumTextExperience
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

    #endregion Inspector variables
    
    #region private variables

    private Dictionary<TypeUIComponent, GameObject> uiDictionary = new Dictionary<TypeUIComponent, GameObject>();

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
    }

    #endregion private functions


    
}
