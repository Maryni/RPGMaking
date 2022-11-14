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
      SetEventsInUI();
   }

   private void OnDisable()
   {
      uiManager.RemoveAllListenersFromButtons();
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
      loadManager.UpdateUIByStats(TypeParameter.StatPoints);
   }

   private void SetActions()
   {
      UpdateHealthSlider();
      UpdateStaminaSlider();
      UpdateExperienceSlider();
      
      playerStats.SetActionsOnLevelUp(UpdateUIFromStats);
      playerStats.SetActionsOnStatsChange(
         () => loadManager.UpdateUIByStats(TypeParameter.StatPoints),
         () => playerStats.SetStatsFromBaseStats(),
         UpdateHealthSlider, 
         UpdateStaminaSlider, 
         UpdateExperienceSlider,
         UpdateUIFromStats);
   }

   private void UpdateHealthSlider()
   {
      Slider sliderHP = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectHealthSlider).GetComponent<Slider>();
      sliderHP.maxValue = playerStats.GetValueFromDictionary(TypeStats.HealthMax);
      playerController.SetActionsOnGetDamage(
         () => sliderHP.value = playerStats.GetValue(TypeStats.HealthMax) - playerStats.GetValue(TypeStats.HealthCurrent),
         UpdateCurrentHealthFromStats);
   }

   private void UpdateStaminaSlider()
   {
      Slider sliderStamina = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectStaminaSlider).GetComponent<Slider>(); 
      sliderStamina.maxValue = playerStats.GetValueFromDictionary(TypeStats.StaminaMax);
      playerController.SetActionsOnGetStamina(
         () => sliderStamina.value = playerStats.GetValue(TypeStats.StaminaMax) - playerStats.GetValue(TypeStats.StaminaCurrent),
         UpdateCurrentStaminaFromStats);
   }
   
   private void UpdateExperienceSlider()
   {
      Slider sliderExperience = uiManager.GetComponentFromDictionary(TypeUIComponent.GameObjectExperienceSlider).GetComponent<Slider>();
      sliderExperience.maxValue = playerStats.GetValueFromDictionary(TypeStats.ExperienceMax);
      playerController.SetActionsOnGetExperience(
         () => sliderExperience.value = playerStats.GetValue(TypeStats.ExperienceCurrent),
         UpdateCurrentExperienceFromStats);
   }

   private void SetEventsInUI()
   {
      Button strPlus = uiManager.GetComponentFromDictionary(TypeUIButtons.StrengthPlus);
      Button dexPlus = uiManager.GetComponentFromDictionary(TypeUIButtons.DexterityPlus);
      Button vitPlus = uiManager.GetComponentFromDictionary(TypeUIButtons.VitalityPlus);
      Button intPlus = uiManager.GetComponentFromDictionary(TypeUIButtons.IntelligencePlus);
      Button wisPlus = uiManager.GetComponentFromDictionary(TypeUIButtons.WisdomPlus);

      strPlus.onClick.AddListener(() => playerStats.IncreaseStat(TypeParameter.Strenght)); 
      strPlus.onClick.AddListener(() => loadManager.UpdateUIByStats(TypeParameter.Strenght));
      dexPlus.onClick.AddListener(() => playerStats.IncreaseStat(TypeParameter.Dexterity));
      dexPlus.onClick.AddListener(() => loadManager.UpdateUIByStats(TypeParameter.Dexterity));
      vitPlus.onClick.AddListener(() => playerStats.IncreaseStat(TypeParameter.Vitality));
      vitPlus.onClick.AddListener(() => loadManager.UpdateUIByStats(TypeParameter.Vitality));
      intPlus.onClick.AddListener(() => playerStats.IncreaseStat(TypeParameter.Intelligence));
      intPlus.onClick.AddListener(() => loadManager.UpdateUIByStats(TypeParameter.Intelligence));
      wisPlus.onClick.AddListener(() => playerStats.IncreaseStat(TypeParameter.Wisdom));
      wisPlus.onClick.AddListener(() => loadManager.UpdateUIByStats(TypeParameter.Wisdom));
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
