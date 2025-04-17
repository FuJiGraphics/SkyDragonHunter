using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIDungeonClearedPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        [SerializeField] private Button m_ExitButton;
        [SerializeField] private Button m_RetryButton;
        [SerializeField] private Button m_NextRoundButton;

        // Unity Methods
        private void Start()
        {
            AddListeners();
        }

        // Public Methods
        public void SetUIMgr(DungeonUIMgr uiMgr)
        {
            m_DungeonUIMgr = uiMgr;
        }

        // Private Methods
        private void AddListeners()
        {
            m_ExitButton.onClick.AddListener(OnClickExitButton);
            m_RetryButton.onClick.AddListener(OnClickRetryButton);
            m_NextRoundButton.onClick.AddListener(OnClickNextLevelButton);
        }
        private void OnClickExitButton()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene((int)SceneIds.GameScene);
        }
        private void OnClickRetryButton()
        {

        }
        private void OnClickNextLevelButton()
        {

        }

    } // Scope by class UIDungeonClearedPanel

} // namespace Root