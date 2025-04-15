using SkyDragonHunter.Structs;
using System.Text;
using TMPro;
using UnityEngine;
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

        // Public Methods
        public void SetLevel(int level)
        {
            m_Level = level;
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Level);
            sb.Append("´Ü°è");
            m_LevelText.text = sb.ToString();
        }

        // Private Metohds
        private void SetDataFromTable()
        {

        }



    } // Scope by class DungeonStages

} // namespace Root