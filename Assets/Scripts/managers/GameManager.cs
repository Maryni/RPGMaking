using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
   #region private variables

   [Inject] private UIManager uiManager;
   [Inject] private Stats playerStats;
   [Inject] private LoadManager loadManager;
   [Inject] private PlayerController playerController;

   #endregion private variables

   #region Unity functions

   private void Start()
   {
      UpdateUIFromStats();
      SetActions();
   }

   #endregion Unity functions

   #region private functions

   private void UpdateUIFromStats()
   {
      UpdateLevelFromStats();
      UpdateCurrentHealthFromStats();
      UpdateMaxHealthFromStats();
      UpdateCurrentStaminaFromStats();
      UpdateMaxStaminaFromStats();
      UpdateCurrentExperienceFromStats();
      UpdateMaxExperienceFromStats();
   }

   private void SetActions()
   {
      var sliderHP = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectHealthSlider).GetComponent<Slider>();
      var sliderStamina = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectStaminaSlider).GetComponent<Slider>();
      var sliderExperience = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectExperienceSlider).GetComponent<Slider>();
      playerStats.SetActionsOnLevelUp(UpdateUIFromStats);
      
      
      sliderHP.maxValue = playerStats.GetValueFromDictionary(TypeStats.HealthMax);
      sliderStamina.maxValue = playerStats.GetValueFromDictionary(TypeStats.StaminaMax);
      sliderExperience.maxValue = playerStats.GetValueFromDictionary(TypeStats.ExperienceMax);
      
      playerController.SetActionsOnGetDamage(
         () => sliderHP.value = playerStats.GetValue(TypeStats.HealthMax) - playerStats.GetValue(TypeStats.HealthCurrent),
         UpdateCurrentHealthFromStats);
      playerController.SetActionsOnGetStamina(
         () => sliderStamina.value = playerStats.GetValue(TypeStats.StaminaMax) - playerStats.GetValue(TypeStats.StaminaCurrent),
         UpdateCurrentStaminaFromStats);
      playerController.SetActionsOnGetExperience(
         () => sliderExperience.value = playerStats.GetValue(TypeStats.ExperienceCurrent),
         UpdateCurrentExperienceFromStats);
   }

   private void UpdateLevelFromStats()
   {
      Text level = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextLevel).GetComponent<Text>();
      level.text = playerStats.GetValue(TypeStats.Level).ToString();
   }

   private void UpdateCurrentHealthFromStats()
   {
      Text healthCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextHealth).GetComponent<Text>();
      healthCurrent.text = playerStats.GetValue(TypeStats.HealthCurrent).ToString();
   }

   private void UpdateMaxHealthFromStats()
   {
      Text healthMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextHealth).GetComponent<Text>();
      healthMax.text = playerStats.GetValue(TypeStats.HealthMax).ToString();
   }

   private void UpdateCurrentStaminaFromStats()
   {
      Text staminaCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextStamina).GetComponent<Text>();
      staminaCurrent.text = playerStats.GetValue(TypeStats.StaminaCurrent).ToString();
   }

   private void UpdateMaxStaminaFromStats()
   {
      Text staminaMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextStamina).GetComponent<Text>();
      staminaMax.text = playerStats.GetValue(TypeStats.StaminaMax).ToString();
   }

   private void UpdateCurrentExperienceFromStats()
   {
      Text experienceCurrent = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextExperience).GetComponent<Text>();
      experienceCurrent.text = playerStats.GetValue(TypeStats.ExperienceCurrent).ToString();
   }

   private void UpdateMaxExperienceFromStats()
   {
      Text experienceMax = uiManager.GetComponentFromDictionary(TypeUIComponent.MaximumTextExperience).GetComponent<Text>();
      experienceMax.text = playerStats.GetValue(TypeStats.ExperienceMax).ToString();
   }

   #endregion private functions
}
