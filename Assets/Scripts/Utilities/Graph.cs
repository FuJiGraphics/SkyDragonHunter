using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Utility {

    public interface IGraphVisible
    {
        public bool IsVisited { get; set; }
    }

    public class GraphNode<T> : MonoBehaviour
    {
        // 필드 (Fields)
        private List<int> m_Edges = new List<int>();

        // 속성 (Properties)
        public int ID { get; set; } = 0;
        public List<int> Edges { get => m_Edges; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddEdge(int nodeIndex)
            => m_Edges.Add(nodeIndex);

        public virtual bool IsVisited { get; set; } = false;
        public virtual void OnVisitAfter() { }

        // Private 메서드
        // Others
    } // Scope by class GraphNode<T>

    public class Graph<T> : MonoBehaviour where T : GraphNode<T>
    {
        // 필드 (Fields)
        private List<T> m_Nodes = new List<T>();
        private Dictionary<int, T> m_NodeMap = new Dictionary<int, T>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddNode(T node)
        {
            m_Nodes.Add(node);
            m_NodeMap.Add(node.ID, node);
        }

        public void TraverseBFS()
        {
            if (m_Nodes == null || m_Nodes.Count <= 0)
                return;

            Queue<GraphNode<T>> queue = new Queue<GraphNode<T>>(); 

            T startNode = m_Nodes[0];
            if (!startNode.IsVisited)
            {
                queue.Enqueue(startNode);
                startNode.IsVisited = true;
            }

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                foreach (var adjNodeID in node.Edges)
                {
                    if (!m_NodeMap.ContainsKey(adjNodeID))
                        continue;

                    if (!m_NodeMap[adjNodeID].IsVisited)
                    {
                        queue.Enqueue(m_NodeMap[adjNodeID]);
                        m_NodeMap[adjNodeID].IsVisited = true;
                    }
                }
            }
        }

        // Private 메서드
        // Others

    } // Scope by class Graph<T>
} // namespace SkyDragonHunter.Utility