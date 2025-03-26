using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BehaviourTree<T> where T : MonoBehaviour
    {
        // 필드 (Fields)
        private BehaviourNode<T> m_rootNode;

        // 외부 종속성 필드 (External dependencies field)
        private readonly T m_context;

        // Public 메서드
        public BehaviourTree(T context)
        {
            m_context = context;
        }

        public void SetRoot(BehaviourNode<T> node)
        {
            m_rootNode = node;
        }

        public NodeStatus Update()
        {
            if(m_rootNode == null)
            {
                return NodeStatus.Failure;
            }

            return m_rootNode.Execute();
        }

        public void Reset()
        {
            if(m_rootNode != null)
            {
                m_rootNode.Reset();
            }
            else
            {
                Debug.LogError($"ROOTNODE RESET FAILED, ROOTNODE NULL");
            }
        }

        // Private 메서드
        // Others
    
    } // Scope by class BehaviourTree

} // namespace Root