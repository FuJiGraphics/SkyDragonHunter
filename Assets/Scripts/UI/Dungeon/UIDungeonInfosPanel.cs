using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    public class UIDungeonInfosPanel : MonoBehaviour
    {
        private const string c_TimerTextFormat = "<color=#FFFF00>남은 시간</color> {0}초";

        // Fields
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        [SerializeField] private TextMeshProUGUI m_DungeonInfoText;
        [SerializeField] private Slider m_ProgressSlider;
        [SerializeField] private TextMeshProUGUI m_ProgressText;
        [SerializeField] private Slider m_TimerSlider;
        [SerializeField] private TextMeshProUGUI m_TimerText;
        [SerializeField] private Button m_EscapeButton;

        // Unity Methods
        public void Start()
        {
            Init();
        }

        // Public Methods
        public void SetUIMgr(DungeonUIMgr uiMgr)
        {
            m_DungeonUIMgr = uiMgr;
        }

        public void SetDungeonInfoText(string dungeonInfo)
        {

        }

        public void SetDungeonProgress(AlphaUnit bossHP, AlphaUnit bossMaxHP)
        {
            var sb = new StringBuilder();
            sb.Append(bossHP.ToString());
            sb.Append(" / ");
            sb.Append(bossMaxHP.ToString());
            m_ProgressText.text = sb.ToString();

            float hpPercentage = (float)bossHP.Value / (float)bossMaxHP.Value;
            m_ProgressSlider.value = hpPercentage;
        }

        public void SetDungeonProgress(int killCount, int goalKillCount)
        {
            var sb = new StringBuilder();
            sb.Append((goalKillCount - killCount).ToString());
            sb.Append(" / ");
            sb.Append(goalKillCount.ToString());
            m_ProgressText.text = sb.ToString();

            float progressPercentage = (float)(goalKillCount - killCount) / (float)goalKillCount;
            m_ProgressSlider.value = progressPercentage;
        }

        public void SetDungeonTimer(float remainingTime, float initialTime)
        {
            remainingTime = Mathf.Clamp(remainingTime, 0f, initialTime);
            string.Format(c_TimerTextFormat, Mathf.FloorToInt(remainingTime));
            m_TimerText.text = string.Format(c_TimerTextFormat, Mathf.FloorToInt(remainingTime));
            m_TimerSlider.value = remainingTime / initialTime;
        }

        // Private Methods
        private void Init()
        {
            SetDungeonInfos();
            InitializeSliders();
            AddListeners();
        }
        private void SetDungeonInfos()
        {
            StringBuilder sb = new StringBuilder();
            DungeonMgr.TryGetStageData(out var dungeonType, out var level);
            sb.Append($"{level}단계 ");
            sb.Append($"{dungeonType}");
            m_DungeonInfoText.text = sb.ToString();
        }
        private void InitializeSliders()
        {
            m_ProgressSlider.minValue = 0f;
            m_ProgressSlider.maxValue = 1f;

            m_TimerSlider.minValue = 0f;
            m_TimerSlider.maxValue = 1f;
        }

        private void AddListeners()
        {
            m_EscapeButton.onClick.AddListener(OnClickEscapeButton);
        }

        private void OnClickEscapeButton()
        {
            m_DungeonUIMgr.EnablePausedPanel(true);
        }
    } // Scope by class UIDungeonInfosPanel

} // namespace Root