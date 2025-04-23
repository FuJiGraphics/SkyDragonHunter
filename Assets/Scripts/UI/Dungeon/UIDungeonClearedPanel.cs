using SkyDragonHunter.Managers;
using SkyDragonHunter.Utility;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter 
{
    public class UIDungeonClearedPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        [SerializeField] private Button m_ExitButton;
        [SerializeField] private Button m_RetryButton;
        [SerializeField] private Button m_NextRoundButton;

        [SerializeField] private TextMeshProUGUI m_GoldText;
        [SerializeField] private TextMeshProUGUI m_DiamondText;
        [SerializeField] private TextMeshProUGUI m_TicketText;

        [SerializeField] private TextMeshProUGUI m_StageInfo;
        [SerializeField] private Image m_RewardIcon;
        [SerializeField] private TextMeshProUGUI m_RewardText;

        [SerializeField] private TextMeshProUGUI m_TimerText;

        private float m_ClearedTimer;
        private const string c_TimerTextFormat = "<color=#FFFF00>{0}</color>초 후 자동 나가기";

        // Unity Methods
        private void Start()
        {
            AddListeners();            
        }

        private void Update()
        {
            UpdateAutoExitTimer();
        }

        private void OnEnable()
        {
            SetDisplayedContents();
        }

        // Public Methods
        public void SetUIMgr(DungeonUIMgr uiMgr)
        {
            m_DungeonUIMgr = uiMgr;
        }

        // Private Methods
        private void SetDisplayedContents()
        {
            m_GoldText.text = ItemMgr.GetItem(Gameplay.ItemType.Coin).ItemCount.ToString();
            m_DiamondText.text = ItemMgr.GetItem(Gameplay.ItemType.Diamond).ItemCount.ToString();
            m_TicketText.text = ItemMgr.GetItem(Gameplay.ItemType.Ticket).ItemCount.ToString();
            m_ClearedTimer = 3f;

            m_TimerText.text = string.Format(c_TimerTextFormat, Mathf.CeilToInt(m_ClearedTimer));

            StringBuilder sb = new StringBuilder();
            DungeonMgr.TryGetStageData(out var dungeonType, out var stageIndex);
            sb.Append($"{dungeonType} - ");
            sb.Append($"{stageIndex}단계");
            m_StageInfo.text = sb.ToString();
        }

        private void UpdateAutoExitTimer()
        {
            m_ClearedTimer -= Time.unscaledDeltaTime;
            int ceiled = Mathf.CeilToInt(m_ClearedTimer);
            m_TimerText.SetText(c_TimerTextFormat, ceiled);
            Debug.Log($"{string.Format(c_TimerTextFormat, ceiled)}");
            if (m_ClearedTimer <= 0)
            {
                OnClickExitButton();
            }
        }

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
            if (ItemMgr.GetItem(Gameplay.ItemType.Ticket).ItemCount > 0)
            {
                Time.timeScale = 1f;
                DungeonMgr.TryGetStageData(out var dungeonType, out var stageIndex);
                DungeonMgr.EnterDungeon(dungeonType, stageIndex);
            }
            else
            {
                Debug.LogError($"Insufficient Dungeon Ticket");
            }
        }

        private void OnClickNextLevelButton()
        {
            if (ItemMgr.GetItem(Gameplay.ItemType.Ticket).ItemCount > 0)
            {
                Time.timeScale = 1f;
                DungeonMgr.TryGetStageData(out var dungeonType, out var stageIndex);
                DungeonMgr.EnterDungeon(dungeonType, stageIndex);
            }
            else
            {
                Debug.LogError($"Insufficient Dungeon Ticket");
            }
        }

    } // Scope by class UIDungeonClearedPanel

} // namespace Root