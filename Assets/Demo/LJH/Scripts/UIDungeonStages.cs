using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIDungeonStages : MonoBehaviour
    {
        // Fields
        [SerializeField] private TextMeshProUGUI m_LevelText;
        [SerializeField] private TextMeshProUGUI m_RecommendedAtkText;
        [SerializeField] private TextMeshProUGUI m_RecommendedHpText;
        [SerializeField] private TextMeshProUGUI m_ClearedText;
        [SerializeField] private Button m_PrefabButton;
        [SerializeField] private GameObject m_Blind;
        private Outline m_Outline;

        private int m_Level;
        private bool isCleared;
        private AlphaUnit m_RecommendedAtk;
        private AlphaUnit m_RecommendedHp;

        // Properties
        public int Level => m_Level;

        // Unity Methods
        private void Awake()
        {
            m_Outline = GetComponent<Outline>();
        }

        // Public Methods
        public void SetLevel(DungeonType dungeonType, int level)
        {
            m_Level = level;
            m_RecommendedAtk = Mathf.Pow(15, level);
            m_RecommendedHp = Mathf.Pow(14, level);

            int clearedStage = DungeonMgr.GetClearedStage(dungeonType);
            
            if (level <= clearedStage)
            {
                isCleared = true;
            }

            if (level > clearedStage + 1)
            {
                SetInteractable(false);
            }
            else
            {
                SetInteractable(true);
            }

            StringBuilder sb = new StringBuilder();
            if (isCleared)
            {               
                sb.Append("Cleared!");
                m_ClearedText.text = sb.ToString();
            }
            m_RecommendedAtkText.text = m_RecommendedAtk.ToString();
            m_RecommendedHpText.text = m_RecommendedHp.ToString();
            sb.Clear();
            sb.Append(m_Level);
            sb.Append("´Ü°è");
            m_LevelText.text = sb.ToString();
        }

        public void AddListener(UnityAction func)
        {
            m_PrefabButton.onClick.AddListener(func);
        }

        public void OnSelectStage(int stageIndex)
        {
            if(stageIndex == m_Level)
            {
                m_Outline.enabled = true;
            }
            else
            {
                m_Outline.enabled = false;
            }
        }

        // Private Metohds
        private void SetInteractable(bool enabled)
        {
            m_PrefabButton.interactable = enabled;
            m_Blind.SetActive(!enabled);
        }
        private void SetDataFromTable()
        {

        }        
    } // Scope by class DungeonStages

} // namespace Root