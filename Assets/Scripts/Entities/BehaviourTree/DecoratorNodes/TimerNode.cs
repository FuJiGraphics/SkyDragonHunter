using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class TimerNode<T> : DecoratorNode<T> where T : MonoBehaviour
    {
        public TimerNode(T context) : base(context)
        {
        }

        protected override NodeStatus ProcessChild()
        {
            throw new System.NotImplementedException();
        }
    } // Scope by class TimerNode

} // namespace Root