using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class ForceSuccessNode<T> : DecoratorNode<T> where T : MonoBehaviour
    {
        // Public 메서드
        public ForceSuccessNode(T context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus ProcessChild()
        {
            var childStatus = m_ChildNode.Execute();
            if (childStatus == NodeStatus.Running)
            {
                return childStatus;
            }
            return NodeStatus.Success;
        }
    } // Scope by class FailerNode

} // namespace Root