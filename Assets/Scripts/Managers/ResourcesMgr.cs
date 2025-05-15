using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SkyDragonHunter.Managers {

    public static class ResourcesMgr
    {
        // 필드 (Fields)
        private static Dictionary<string, UnityEngine.Object> s_Cache = new();

        // 속성 (Properties)
        public static bool AddressableMode { get; set; } = true;
        public static Sprite EmptySprite => Load<Sprite>("TransparentImage");

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            T result = null;
            if (!string.IsNullOrEmpty(path))
            {
                if (!AddressableMode)
                    result = LoadResource<T>(path);
                else
                    result = LoadAssetSync<T>(path);
                if (result == null)
                {
                    Debug.LogError($"[ResourcesMgr]: 데이터 로드에 실패하였습니다. {path}");
                }
            }
            return result;
        }

        public static async Task<T> LoadAsync<T>(string path)
            where T : UnityEngine.Object
        {
            if (s_Cache.TryGetValue(path, out var cached))
                return cached as T;

            var handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var result = handle.Result;
                s_Cache[path] = result;
                return result;
            }
            else
            {
                Debug.LogError($"[ResourcesMgr]: Failed to async load addressable: {path}");
                return null;
            }
        }

        public static void ClearCache()
        {
            s_Cache.Clear();
        }

        // Private 메서드
        private static T LoadResource<T>(string path)
             where T : UnityEngine.Object
        {
            T result = null;
            if (s_Cache.TryGetValue(path, out var cached))
            {
                result = cached as T;
            }
            else
            {
                result = Resources.Load<T>(path);
                s_Cache.Add(path, result);
            }
            return result;
        }


        private static T LoadAssetSync<T>(string path)
             where T : UnityEngine.Object
            //변수로 가져와서 불러오기
        {
            if (s_Cache.TryGetValue(path, out var cached))
            {
                return cached as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(path);
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var result = handle.Result;
                s_Cache[path] = result;
                return result;
            }
            else
            {
                Debug.LogError($"[ResourcesMgr]: Failed to load addressable: {path}");
                return null;
            }
        }

        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
