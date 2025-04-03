using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class SkillAnchorProvider : MonoBehaviour
        , ISkillAnchorProvider
    {
        [Tooltip("��ų �߻�Ǵ� ��ġ")]
        public Transform firePoint;

        public Transform GetSkillFirePoint()
            => firePoint;

        // �ʵ� (Fields)
        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
        // Others

    } // Scope by class WeaponAnchorProvider
} // namespace SkyDragonHunter