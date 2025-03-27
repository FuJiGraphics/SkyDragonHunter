using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Structs
{
    /*
     * @brief ��ȭ(���� �̻�)�� �����ϴ� ����ü
     */
    public struct SlowStatusAilment
    {
        public GameObject attacker;         // �����ϴ� ��ü
        public GameObject defender;         // ���� �޴� ���
        public float slowMultiplier;        // ���ο� ����
        public float duration;              // ���� �ð�
        public float immunityMultiplier;    // ��ȭ ���� ���� (���� ���� �ð� = ���� �ð� * ���� ����)
    }; // struct SlowStatusAilment

} // namespace SkyDragonHunter.Structs