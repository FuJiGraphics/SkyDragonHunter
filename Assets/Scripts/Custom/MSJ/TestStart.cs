using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter {

    public class TestStart : MonoBehaviour
    {
        // 필드 (Fields)
        public string loadingSceneName;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        public void OnStart()
        {
            SceneMgr.LoadScene(loadingSceneName);
        }

        public void ClearSaveData()
        {
            string saveDirectory = $"{Application.persistentDataPath}/Save";
            string[] saveFilePaths = Directory.GetFiles(saveDirectory, "*", SearchOption.TopDirectoryOnly);
            if (saveFilePaths == null || saveFilePaths.Length <= 0)
                return;

            foreach (string saveFilePath in saveFilePaths)
            {
                string fileName = Path.GetFileName(saveFilePath);
                if (fileName.StartsWith("SDH_SavedGameData"))
                {
                    Debug.Log($"[SaveLoadMgr]: 세이브 데이터 삭제: {fileName}");
                    File.Delete(saveFilePath);
                }
            }

            Debug.Log("[SaveLoadMgr]: 세이브 데이터 삭제 완료");
        }

        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class TestStart

} // namespace Root