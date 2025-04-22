using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace SkyDragonHunter.Utility {

    public static class ResourcesMgr
    {
        // �ʵ� (Fields)
        private static readonly Dictionary<string, Object> m_Cache = new();

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public static T Load<T>(string path) where T : Object
        {
            T result = null;
            if (m_Cache.TryGetValue(path, out var cached))
            {
                result = cached as T;
            }
            else
            {
                result = Resources.Load<T>(path);
                m_Cache.Add(path, result);
            }
            return result;
        }

        // ��ε�
        // ��ü ĳ�� Ŭ����
        public static void ClearCache()
        {
            m_Cache.Clear();
        }

        // Private �޼���
        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
