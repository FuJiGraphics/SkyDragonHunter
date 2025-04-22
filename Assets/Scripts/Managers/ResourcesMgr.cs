using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace SkyDragonHunter.Utility {

    public static class ResourcesMgr
    {
        // 필드 (Fields)
        private static readonly Dictionary<string, Object> m_Cache = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
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

        // 언로드
        // 전체 캐시 클리어
        public static void ClearCache()
        {
            m_Cache.Clear();
        }

        // Private 메서드
        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
