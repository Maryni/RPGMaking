using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeUIComponent
{
    GameObjectHealth,
    GameObjectStamina,
    GameObjectLevel,
    GameObjectExperience,
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
    private Dictionary<TypeUIComponent, GameObject> uiDictionary = new Dictionary<TypeUIComponent, GameObject>();
    [SerializeField, Header("Health")] private GameObject gameObjectHealth;
    [SerializeField] private Text textCurrentHealth;
    [SerializeField] private Text textMaxHealth;
    [SerializeField, Header("Stamina")] private GameObject gameObjectStamina;
    [SerializeField] private Text textCurrentStamina;
    [SerializeField] private Text textMaxStamina;
    [SerializeField, Header("Level")] private GameObject gameObjectLevel;
    [SerializeField] private Text textCurrentLevel;
    [SerializeField, Header("Experience")] private GameObject gameObjectExperience;
    [SerializeField] private Text textCurrentExperience;
    [SerializeField] private Text textMaxExperience;

    private void OnEnable()
    {
        SetObjectsToDictionary();
    }

    private void SetObjectsToDictionary()
    {
        Debug.Log($"$started, gameObject = {gameObject.name}");
        uiDictionary.Add(TypeUIComponent.GameObjectHealth, gameObjectHealth);
        uiDictionary.Add(TypeUIComponent.GameObjectStamina, gameObjectStamina);
        uiDictionary.Add(TypeUIComponent.GameObjectLevel, gameObjectLevel);
        uiDictionary.Add(TypeUIComponent.GameObjectExperience, gameObjectExperience);
        uiDictionary.Add(TypeUIComponent.CurrentTextHealth, textCurrentHealth.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextStamina, textCurrentStamina.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextLevel, textCurrentLevel.gameObject);
        uiDictionary.Add(TypeUIComponent.CurrentTextExperience, textCurrentExperience.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextHealth, textMaxHealth.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextStamina, textMaxStamina.gameObject);
        uiDictionary.Add(TypeUIComponent.MaximumTextExperience, textMaxExperience.gameObject);
        Debug.Log($"$finished, gameObject = {gameObject.name}");
    }

    public GameObject GetComponentFromDictionary(TypeUIComponent component)
    {
         return uiDictionary[component];
    }
}
