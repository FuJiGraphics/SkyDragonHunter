using SkyDragonHunter.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIDungeonPausedPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Button m_CancelButton;
        [SerializeField] private Button m_ExitButton;

        public void Start()
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
            m_CloseButton.onClick.AddListener(OnClickCloseButton);
            m_CancelButton.onClick.AddListener(OnClickCloseButton);
            m_ExitButton.onClick.AddListener(OnClickExitButton);
        }

        private void OnClickCloseButton()
        {
            m_DungeonUIMgr.EnablePausedPanel(false);
        }

        private void OnClickExitButton()
        {
            Time.timeScale = 1f;
            SceneChangeMgr.LoadScene((int)SceneIds.GameScene);
        }
    } // Scope by class UIDungeonPausedPanel

} // namespace Root