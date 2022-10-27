using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityGameEventListener : MonoBehaviour, IGameEventListener
{
   [SerializeField] private GameEvent gameEvent;
   [SerializeField] private UnityEvent response;

   public void OnEnable()
   {
       if (gameEvent != null)
       {
           gameEvent.RegisterListener(this);
       }
   }

   public void OnDisable()
   {
       gameEvent.UnregisterListener(this);
   }

   public void OnEventRaised()
    {
        response?.Invoke();
    }
}
