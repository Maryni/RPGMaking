using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadManager : MonoBehaviour
{
    #region private variables

    [Inject] private Stats playerStats;
    [Inject] private UIManager uiManager;

    #endregion private variables

    #region Unity functions

    private void Start()
    {
        LoadStatsToUI();
    }

    #endregion Unity functions

    #region private functions

    public void UpdateUIByStats(TypeParameter typeParameter)
    {
        switch (typeParameter)
        {
            case TypeParameter.Strenght: LoadStrenght(); break;
            case TypeParameter.Dexterity: LoadDexterity(); break;
            case TypeParameter.Vitality: LoadVitality(); break;
            case TypeParameter.Intelligence: LoadIntelligence(); break;
            case TypeParameter.Wisdom: LoadWisdom(); break;
            case TypeParameter.StatPoints: LoadStatsPoints(); break;
            default: LoadStatsToUI(); break;
        }
    }
    
    private void LoadStatsToUI()
    {
        LoadStrenght();
        LoadDexterity();
        LoadVitality();
        LoadIntelligence();
        LoadWisdom();
        LoadStatsPoints();
    }

    private void LoadStrenght()
    {
        Text strength = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextStrength).GetComponent<Text>();
        strength.text = playerStats.BaseStats.Strength.ToString(); 
    }

    private void LoadDexterity()
    {
        Text dexterity = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextDexterity).GetComponent<Text>();
        dexterity.text = playerStats.BaseStats.Dexterity.ToString();
    }

    private void LoadVitality()
    {
        Text vitality = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextVitality).GetComponent<Text>();
        vitality.text = playerStats.BaseStats.Vitality.ToString(); 
    }

    private void LoadIntelligence()
    {
        Text intelligence = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextIntelligence).GetComponent<Text>();
        intelligence.text = playerStats.BaseStats.Intelligence.ToString();
    }

    private void LoadWisdom()
    {
        Text wisdom = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextWisdom).GetComponent<Text>();
        wisdom.text = playerStats.BaseStats.Wisdom.ToString();
    }

    private void LoadStatsPoints()
    {
        Text stats = uiManager.GetComponentFromDictionary(TypeUIComponent.CurrentTextStatPoints).GetComponent<Text>();
        stats.text = playerStats.CountFreeStatPoints.ToString();
    }
    
    #endregion private functions
}
