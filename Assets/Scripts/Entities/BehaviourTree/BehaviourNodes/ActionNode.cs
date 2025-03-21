using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public abstract class ActionNode<T> : BehaviourNode<T> where T : MonoBehaviour
    {
        // Protected 메서드
        protected ActionNode(T context) : base(context)
        {
        }
    } // Scope by class ActionNode

} // namespace Root