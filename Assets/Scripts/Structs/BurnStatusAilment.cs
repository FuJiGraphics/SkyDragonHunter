using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Structs
{
    /*
     * @brief ȭ��(���� �̻�)�� �����ϴ� ����ü
     */
    public struct BurnStatusAilment
    {
        public GameObject attacker;         // �����ϴ� ��ü
        public GameObject defender;         // ���� �޴� ���
        public AlphaUnit damagePerSeconds;  // �ʴ� ������
        public float duration;              // ���� �ð�
    
    }; // struct Attack

} // namespace SkyDragonHunter.Structs