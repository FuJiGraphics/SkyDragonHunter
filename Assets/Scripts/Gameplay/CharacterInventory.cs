using SkyDragonHunter.Scriptables;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class CharacterInventory : MonoBehaviour
    {
        // 필드 (Fields)
        public AttackDefinition[] weapons;
        public AttackDefinition CurrentWeapon { get; private set; }
        public Transform weaponDummy;

        private GameObject m_WeaponGo;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if (weapons != null && weapons.Length >= 1)
            {
                EquipWeapon(0);
            }
        }

        // Public 메서드
        public void EquipWeapon(int index)
        {
            if (0 > index || index >= weapons.Length || weapons[index] == null)
            {
                Debug.LogError($"잘못된 인덱스 접근입니다. {index}");
                return;
            }

            UnequipWeapon();

            CurrentWeapon = weapons[index];
            CurrentWeapon.owner = gameObject;
            CurrentWeapon.dummy = weaponDummy;
            if (weapons[index].prefab != null)
            {
                m_WeaponGo = Instantiate(weapons[index].prefab);
                m_WeaponGo.transform.SetParent(weaponDummy);
            }
        }

        public void UnequipWeapon()
        {
            CurrentWeapon = null;
            Destroy(m_WeaponGo);
        }

        // Private 메서드
        // Others

    } // Scope by class CharacterInventory
} // namespace Root
