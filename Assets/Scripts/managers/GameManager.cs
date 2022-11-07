using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
   [Inject] private UIManager uiManager;
   [Inject] private Stats playerStats;
   [Inject] private LoadManager loadManager;

   private void Start()
   {
      LoadStatsToUI();
   }

   private void LoadStatsToUI()
   {
      var level = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextLevel).GetComponent<Text>();
      level.text = playerStats.GetValueFromDictionary(TypeStats.Level).ToString();
      
      var healthCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextHealth).GetComponent<Text>();
      healthCurrent.text = playerStats.GetValueFromDictionary(TypeStats.HealthCurrent).ToString();
   
      var healthMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextHealth).GetComponent<Text>();
      healthMax.text = playerStats.GetValueFromDictionary(TypeStats.HealthMax).ToString();
      
      var staminaCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextStamina).GetComponent<Text>();
      staminaCurrent.text = playerStats.GetValueFromDictionary(TypeStats.StaminaCurrent).ToString();
      
      var staminaMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextStamina).GetComponent<Text>();
      staminaMax.text = playerStats.GetValueFromDictionary(TypeStats.StaminaMax).ToString();
      
      var experienceCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextExperience).GetComponent<Text>();
      experienceCurrent.text = playerStats.GetValueFromDictionary(TypeStats.ExperienceCurrent).ToString();
      
      var experienceMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextExperience).GetComponent<Text>();
      experienceMax.text = playerStats.GetValueFromDictionary(TypeStats.ExperienceMax).ToString();
   }
}
