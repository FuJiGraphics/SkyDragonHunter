using SkyDragonHunter.Scriptables;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class CharacterInventory : MonoBehaviour
    {
        // 필드 (Fields)
        public AttackDefinition[] weapons;
        public Transform weaponDummy;

        private GameObject m_WeaponGo;

        // 속성 (Properties)
        public AttackDefinition CurrentWeapon { get; private set; }
        public GameObject WeaponPrefabInstance { get; private set; }
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
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

            Debug.Log($"owner = {gameObject}");
            CurrentWeapon = weapons[index];
            CurrentWeapon.SetOwner(gameObject);
            CurrentWeapon.SetDummy(weaponDummy);
            if (weapons[index].weaponPrefab != null)
            {
                m_WeaponGo = Instantiate(weapons[index].weaponPrefab);
                m_WeaponGo.transform.SetParent(weaponDummy);
                m_WeaponGo.transform.localPosition = Vector3.zero;
                weapons[index].SetActivePrefabInstance(m_WeaponGo);
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
