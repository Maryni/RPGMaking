using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodedGameEventListener : IGameEventListener
{
    [SerializeField] private GameEvent gameEvent;
    private Action m_onResponse;

    public void OnEventRaised()
    {
        m_onResponse?.Invoke();
    }

    public void OnEnable(Action response)
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
            m_onResponse = response;
        }
    }

    public void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
            m_onResponse = null;
        }
    }
}
