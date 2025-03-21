using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class InverterNode<T> : DecoratorNode<T> where T : MonoBehaviour
    {
        // Public 메서드
        public InverterNode(T context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus ProcessChild()
        {
            var childStatus = m_ChildNode.Execute();
            if (childStatus == NodeStatus.Success)
            {
                childStatus = NodeStatus.Failure;
            }
            else if (childStatus == NodeStatus.Failure)
            {
                childStatus = NodeStatus.Success;
            }
            return childStatus;
        }
    } // Scope by class InverterNode

} // namespace Root