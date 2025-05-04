using SkyDragonHunter.Tables;
using SkyDragonHunter.Structs;

namespace SkyDragonHunter.Gameplay {

    public class Crystal
    {
        // �ʵ� (Fields)
        private int m_Id;
        private int m_CurrentLevel;
        private BigNum m_IncDamage;
        private BigNum m_InchpUp;
        private BigNum m_NeedExp;
        private int m_CurrLevelId;
        private int m_NextLevelId;

        // �Ӽ� (Properties)
        public int ID => m_Id;
        public int CurrentLevel => m_CurrentLevel;
        public int CurrLevelId => m_CurrLevelId;
        public int NextLevelId => m_NextLevelId;
        public BigNum IncreaseDamage => m_IncDamage;
        public BigNum IncreaseHealth => m_InchpUp;
        public BigNum NeedExp => m_NeedExp;

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
            m_IncDamage = new BigNum(tableData.AtkUP);
            m_InchpUp = new BigNum(tableData.HPUP);
            m_NeedExp = new BigNum(tableData.NeedEXP);
        }

        // Private �޼���
        // Others

    } // Scope by class Crystal
} // namespace SkyDragonHunter.Gameplay
