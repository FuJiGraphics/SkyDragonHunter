using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Temp;
using SkyDragonHunter.UI;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UiCrewLevelUpPanel : MonoBehaviour
    {
        [SerializeField] private int cachedCrewId;
        [SerializeField] private BigNum requiredFood;
        [SerializeField] private UICrewInfoPanel m_CrewInfoPanel;

        [SerializeField] private TextMeshProUGUI m_UiCrewLevelText;
        [SerializeField] private TextMeshProUGUI m_UiCrewAttackText;
        [SerializeField] private TextMeshProUGUI m_UiCrewHpText;
        [SerializeField] private TextMeshProUGUI m_UiCrewArmorText;
        [SerializeField] private TextMeshProUGUI m_UiCrewResText;

        [SerializeField] private TextMeshProUGUI m_UiHoldEffectAtkText;
        [SerializeField] private TextMeshProUGUI m_UiHoldEffectDefText;

        [SerializeField] private TextMeshProUGUI m_UiHoldingResourceText;
        [SerializeField] private TextMeshProUGUI m_UiRequiredResourceText;

        [SerializeField] private Button m_UiCloseButton;
        [SerializeField] private Button m_UiLevelUpButton;

        public void SetPanel(int crewId)
        {
            if(!TempCrewLevelExpContainer.TryGetTempCrewData(crewId, out var crewLevelData))
            {
                Debug.LogError($"[UiCrewLevelUpPanel] Set Level up panel failed, Cannot find temp crew level data with id '{crewId}' ");
                return;
            }
            cachedCrewId = crewId;

            var crewLevel = crewLevelData.Level;
            var currentBasicStat = crewLevelData.BasicStat;

            var nextBasicStat = crewLevelData.BasicStatNextLevel;

            requiredFood = crewLevelData.RequiredExp;
            m_UiHoldingResourceText.text = AccountMgr.Food.ToUnit();

            if (crewLevel != 100)
            {
                m_UiCrewLevelText.text = crewLevel + " > " + (crewLevel + 1);
                m_UiCrewAttackText.text = currentBasicStat.MaxDamage.ToUnit() + " > " + nextBasicStat.MaxDamage.ToUnit();
                m_UiCrewHpText.text = currentBasicStat.MaxHealth.ToUnit() + " > " + nextBasicStat.MaxHealth.ToUnit();
                m_UiCrewArmorText.text = currentBasicStat.MaxArmor.ToUnit() + " > " + nextBasicStat.MaxArmor.ToUnit();
                m_UiCrewResText.text = currentBasicStat.MaxResilient.ToUnit() + " > " + nextBasicStat.MaxResilient.ToUnit();
                m_UiRequiredResourceText.text = crewLevelData.RequiredExp.ToUnit();
                m_UiLevelUpButton.enabled = AccountMgr.Food >= crewLevelData.RequiredExp;
            }
            else
            {
                string maxLevelString = "Max Level";
                m_UiCrewLevelText.text = crewLevel + " > " + maxLevelString;
                m_UiCrewAttackText.text = currentBasicStat.MaxDamage.ToUnit() + " > " + maxLevelString;
                m_UiCrewHpText.text = currentBasicStat.MaxHealth.ToUnit() + " > " + maxLevelString;
                m_UiCrewArmorText.text = currentBasicStat.MaxArmor.ToUnit() + " > " + maxLevelString;
                m_UiCrewResText.text = currentBasicStat.MaxResilient.ToUnit() + " > " + maxLevelString;
                m_UiRequiredResourceText.text = maxLevelString;
                m_UiLevelUpButton.enabled = false;
            }
        }

        public void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }

        public void OnClickLevelUpButton()
        {
            if(!TempCrewLevelExpContainer.TryGetTempCrewData(cachedCrewId, out var crewLevelData))
            {
                Debug.LogError($"Cannot Find crew level data with id '{cachedCrewId}'");
                return;
            }
            AccountMgr.Food = AccountMgr.Food - requiredFood;
            crewLevelData.AddAccumulatedExp(requiredFood);
            SetPanel(cachedCrewId);
            m_CrewInfoPanel.OnLevelUp();
        }

    } // Scope by class UiCrewLevelUpPanel

} // namespace Root