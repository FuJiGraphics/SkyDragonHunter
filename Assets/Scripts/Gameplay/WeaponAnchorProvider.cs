using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class WeaponAnchorProvider : MonoBehaviour
        , IWeaponAnchor
    {
        [Tooltip("���Ÿ� ���� �߻�Ǵ� ��ġ")]
        public Transform firePoint;

        public Transform GetWeaponFirePoint()
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