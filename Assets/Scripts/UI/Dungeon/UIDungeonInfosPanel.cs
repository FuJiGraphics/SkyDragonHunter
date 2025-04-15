using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIDungeonInfosPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr       m_DungeonUIMgr;

        [SerializeField] private TextMeshProUGUI    m_DungeonInfoText;
        [SerializeField] private Slider             m_ProgressSlider;
        [SerializeField] private TextMeshProUGUI    m_ProgressText;
        [SerializeField] private Slider             m_TimerSlider;
        [SerializeField] private TextMeshProUGUI    m_TimerText;
        [SerializeField] private Button             m_EscapeButton;

        // Unity Methods
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
            m_EscapeButton.onClick.AddListener(OnClickEscapeButton);
        }
        
        private void OnClickEscapeButton()
        {
            m_DungeonUIMgr.EnablePausedPanel(true);
        }
    } // Scope by class UIDungeonInfosPanel

} // namespace Root