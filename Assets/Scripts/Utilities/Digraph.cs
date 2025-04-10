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
        private List<T> m_Edges = new List<T>();

        // 속성 (Properties)
        public List<T> Edges { get => m_Edges; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddEdge(T node)
            => m_Edges.Add(node);

        public virtual bool IsVisited { get; set; } = false;

        // Private 메서드
        // Others
    } // Scope by class GraphNode<T>

    public class Graph<T> : MonoBehaviour where T : GraphNode<T>
    {
        // 필드 (Fields)
        private List<T> m_Nodes = new List<T>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddNode(T node)
            => m_Nodes.Add(node);

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

                foreach (var adjNode in node.Edges)
                {
                    if (!adjNode.IsVisited)
                    {
                        queue.Enqueue(adjNode);
                        adjNode.IsVisited = true;
                    }
                }
            }
        }

        // Private 메서드
        // Others

    } // Scope by class Graph<T>
} // namespace SkyDragonHunter.Utility