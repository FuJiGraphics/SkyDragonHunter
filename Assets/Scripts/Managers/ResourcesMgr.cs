using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SkyDragonHunter.Managers {

    public static class ResourcesMgr
    {
        // �ʵ� (Fields)
        private static Dictionary<string, UnityEngine.Object> s_Cache = new();

        // �Ӽ� (Properties)
        public static bool AddressableMode { get; set; } = true;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
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
                    Debug.LogError($"[ResourcesMgr]: ������ �ε忡 �����Ͽ����ϴ�. {path}");
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

        // Private �޼���
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
            //������ �����ͼ� �ҷ�����
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
