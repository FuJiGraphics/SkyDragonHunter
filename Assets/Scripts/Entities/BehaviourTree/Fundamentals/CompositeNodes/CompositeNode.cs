using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public abstract class CompositeNode<T> : BehaviourNode<T> where T : MonoBehaviour
    {
        // 필드 (Fields)
        protected readonly List<BehaviourNode<T>> m_ChildNodes;
        protected int m_currentChild;

        // Public 메서드
        public void AddChild(BehaviourNode<T> child)
        {
            m_ChildNodes.Add(child);
        }

        public bool RemoveChild(BehaviourNode<T> child)
        {
            return m_ChildNodes.Remove(child);
        }

        public override void Reset()
        {
            base.Reset();
            foreach(var childNode in m_ChildNodes)
            {
                childNode.Reset();
            }
        }

        // Protected 메서드
        protected CompositeNode(T context) : base(context)
        {
            m_ChildNodes = new List<BehaviourNode<T>>();
        }
    } // Scope by class CompositeNode

} // namespace Root