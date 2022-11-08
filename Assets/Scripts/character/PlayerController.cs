using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    #region private variables

    [Inject] private Stats playerStats;
    private event Action OnGetDamage;
    private event Action OnGetStamina;
    private event Action OnGetExperience;

    #endregion private variables

    #region Unity functions

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            GetValueByType(TypeStats.Damage,5);
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            GetValueByType(TypeStats.StaminaCurrent,5);
        }
        
        if (Input.GetKeyUp(KeyCode.X))
        {
            GetValueByType(TypeStats.ExperienceCurrent,5);
        }
        
    }

    #endregion Unity functions

    #region public functions

    public void GetValueByType(TypeStats typeStats,float value)
    {
        playerStats.SetValueByType(typeStats,value);
        if (typeStats == TypeStats.Damage)
        {
            OnGetDamage();
        }
        if (typeStats == TypeStats.StaminaCurrent)
        {
            OnGetStamina();
        }
        if (typeStats == TypeStats.ExperienceCurrent)
        {
            OnGetExperience();
        }
    }

    public void SetActionsOnGetDamage(params Action[] actions)
    {
        foreach (var action in actions)
        {
            OnGetDamage += action;
        }
    }
    
    public void SetActionsOnGetStamina(params Action[] actions)
    {
        foreach (var action in actions)
        {
            OnGetStamina += action;
        }
    }
    
    public void SetActionsOnGetExperience(params Action[] actions)
    {
        foreach (var action in actions)
        {
            OnGetExperience += action;
        }
    }
    
    #endregion public functions
    



}
