using SkyDragonHunter.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkyDragonHunter {

    public class DestructableEvent : MonoBehaviour, IDestructible
    {
        public UnityEvent destructEvent;
        
        public void OnDestruction(GameObject attacker)
        {
            destructEvent?.Invoke();
        }
    } // Scope by class DestructableEvent

} // namespace Root