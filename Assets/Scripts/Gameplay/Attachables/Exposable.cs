using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class Exposable : MonoBehaviour
    {
        // �ʵ� (Fields)
        private AlphaUnit m_ExposeDamageMultiplier = 1;

        // �Ӽ� (Properties)
        public AlphaUnit ExposeDamageMultiplier 
        {
            get => m_ExposeDamageMultiplier;
            set
            {
                if (value > 0)
                {
                    m_ExposeDamageMultiplier = value;
                }
                else
                {
                    m_ExposeDamageMultiplier = 1;
                }
            }
        }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
        // Others

    } // Scope by class Attackable
} // namespace SkyDragonHunter