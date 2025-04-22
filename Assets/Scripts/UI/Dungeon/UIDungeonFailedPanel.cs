using UnityEngine.UI;
using UnityEngine;
using TMPro;
using SkyDragonHunter.Utility;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter
{
    public class UIDungeonFailedPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        [SerializeField] private Button m_ExitButton;
        [SerializeField] private Button m_RetryButton;
        [SerializeField] private Button m_NextRoundButton;

        [SerializeField] private TextMeshProUGUI m_DungeonInfo;
        [SerializeField] private TextMeshProUGUI m_AutoExitText;

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
    } // Scope by class UIDungeonFailedPanel

} // namespace Root