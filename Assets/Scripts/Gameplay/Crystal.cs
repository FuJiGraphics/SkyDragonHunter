using SkyDragonHunter.Tables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class Crystal
    {
        // �ʵ� (Fields)
        private int m_Id;
        private int m_CurrentLevel;
        private AlphaUnit m_IncDamage;
        private AlphaUnit m_InchpUp;
        private AlphaUnit m_NeedExp;
        private int m_CurrLevelId;
        private int m_NextLevelId;

        // �Ӽ� (Properties)
        public int ID => m_Id;
        public int CurrentLevel => m_CurrentLevel;
        public int CurrLevelId => m_CurrLevelId;
        public int NextLevelId => m_NextLevelId;
        public AlphaUnit IncreaseDamage => m_IncDamage;
        public AlphaUnit IncreaseHealth => m_InchpUp;
        public AlphaUnit NeedExp => m_NeedExp;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public Crystal(CrystalLevelData tableData)
        {
            m_Id = tableData.ID;
            m_CurrentLevel = tableData.Level;
            m_CurrLevelId = tableData.ID;
            m_NextLevelId = tableData.NextLvID;
            m_IncDamage = new AlphaUnit(tableData.AtkUP);
            m_InchpUp = new AlphaUnit(tableData.HPUP);
            m_NeedExp = new AlphaUnit(tableData.NeedEXP);
        }

        // Private �޼���
        // Others

    } // Scope by class Crystal
} // namespace SkyDragonHunter.Gameplay
