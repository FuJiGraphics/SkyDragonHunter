using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Structs
{
    /*
     * @brief ����(���� �̻�)�� �����ϴ� ����ü
     */
    public struct FreezeStatusAilment
    {
        public GameObject attacker;         // �����ϴ� ��ü
        public GameObject defender;         // ���� �޴� ���
        public float duration;              // ���� �ð�
        public float immunityMultiplier;    // ���� ���� ���� (���� ���� �ð� = ���� �ð� * ���� ����)
    
    }; // struct FreezeStatusAilment

} // namespace SkyDragonHunter.Structs