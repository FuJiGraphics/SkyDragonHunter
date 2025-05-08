using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class FavorabilityUIController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI expText;
        [SerializeField] private Slider expSlider;

        [SerializeField] private FavorailityMgr favorabilityMgr;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            UpdateUI();
            expSlider.value = (float)favorabilityMgr.GetCurrentExp();
            expSlider.maxValue = (float)favorabilityMgr.GetExpToNext();
        }
        // Public 메서드
        public void UpdateUI()
        {
            int level = favorabilityMgr.GetLevel();
            int currentExp = (int)favorabilityMgr.GetCurrentExp();
            int requiredExp = (int)favorabilityMgr.GetExpToNext();

            expSlider.value = currentExp;
            expSlider.maxValue = requiredExp;

            levelText.text = $"Lv. {level}";
            expText.text = $"{currentExp} / {requiredExp}";
        }
        // Private 메서드
        // Others

    } // Scope by class FavorabilityUIController

} // namespace Root