using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Utility {

    public interface IGraphVisible
    {
        public bool IsVisited { get; set; }
    }

    public class GraphNode<T> : MonoBehaviour where T : class
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

        public bool IsVisited { get; set; } = false;
        public virtual void OnVisited(T[] egdeNodes) { }

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

        public void ResetVisitedFlags()
        {
            foreach (var node in m_NodeMap)
            {
                node.Value.IsVisited = false;
            }
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
                node.OnVisited(this.GetNodeList(node.Edges));

                foreach (var adjNodeID in node.Edges)
                {
                    if (!m_NodeMap.ContainsKey(adjNodeID))
                        continue;

                    var targetNode = m_NodeMap[adjNodeID];
                    if (!targetNode.IsVisited)
                    {
                        queue.Enqueue(targetNode);
                        targetNode.IsVisited = true;
                    }
                }
            }
        }

        // Private 메서드
        T[] GetNodeList(List<int> indices)
        {
            List<T> result = null;
            foreach (var index in indices)
            {
                if (index == -1)
                    continue;

                if (result == null)
                    result = new List<T>();
                result.Add(m_NodeMap[index]);
            }
            return result?.ToArray();
        }

        // Others

    } // Scope by class Graph<T>
} // namespace SkyDragonHunter.Utility