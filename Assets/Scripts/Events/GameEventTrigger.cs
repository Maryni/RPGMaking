using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
   [SerializeField] private bool m_TriggerOnce;
   private bool m_hasGameEventBeenTriggered;
   
   [SerializeField] private GameEvent gameEvent;
   private bool m_hasGameEvent = true;

   [SerializeField] [TagSelector] private string[] m_TriggerTags = new[] {"Player"};
   private bool m_hasTags;

   private void Start()
   {
      m_hasGameEvent = gameEvent != null;
      m_hasTags = m_TriggerTags == null || m_TriggerTags.Length < 1;

      if (!m_hasGameEvent)
      {
         Debug.LogWarning($"No Game Event assigned");
      }

      if (!m_hasTags)
      {
         Debug.LogWarning($"No Tags assigned");
         enabled = false;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if(!m_hasGameEvent || !m_hasTags) return;
      if(m_TriggerOnce && m_hasGameEventBeenTriggered) return;
      
      Debug.Assert(m_TriggerTags != null, nameof(m_TriggerTags) + " != null");
      foreach (string triggerTag in m_TriggerTags)
      {
         Debug.Assert(other != null, nameof(other) + " != null");
         if(!other.CompareTag(triggerTag)) continue;

         m_hasGameEventBeenTriggered = true;
         Debug.Assert(gameEvent != null, nameof(gameEvent) + " != null");
         gameEvent.Raise();
      }
   }
}

public class TagSelectorAttribute : PropertyAttribute
{
   public bool UseDefaultTagFieldDrawer = false;
}
