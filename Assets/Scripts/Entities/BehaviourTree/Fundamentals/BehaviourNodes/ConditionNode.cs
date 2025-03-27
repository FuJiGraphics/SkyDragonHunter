using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public abstract class ConditionNode<T> : BehaviourNode<T> where T : MonoBehaviour
    {
        // Protected 메서드
        protected ConditionNode(T context) : base(context)
        {
        }
    
    } // Scope by class ConditionNode

} // namespace Root