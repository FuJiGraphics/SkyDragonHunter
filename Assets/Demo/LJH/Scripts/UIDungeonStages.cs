using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Structs;
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

        private int m_Level;
        private bool isCleared;
        private AlphaUnit m_RecommendedAtk;
        private AlphaUnit m_RecommendedHp;

        // Properties
        public int Level => m_Level;

        // Public Methods
        public void SetLevel(int level)
        {
            m_Level = level;
            m_RecommendedAtk = Mathf.Pow(15, level);
            m_RecommendedHp = Mathf.Pow(14, level);

            m_RecommendedAtkText.text = m_RecommendedAtk.ToString();
            m_RecommendedHpText.text = m_RecommendedHp.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Level);
            sb.Append("´Ü°è");
            m_LevelText.text = sb.ToString();
        }

        public void AddListener(UnityAction func)
        {
            m_PrefabButton.onClick.AddListener(func);
        }

        // Private Metohds
        private void SetDataFromTable()
        {

        }


    } // Scope by class DungeonStages

} // namespace Root