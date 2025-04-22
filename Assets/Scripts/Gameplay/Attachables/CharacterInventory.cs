using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class CharacterInventory : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Settings")]
        [SerializeField] private AttackDefinition[] itemPrefabs;

        // 속성 (Properties)
        public GameObject CurrentEquipPreview { get; private set; }
        public IWeaponable CurrentWeapon { get; private set; }
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (itemPrefabs != null && itemPrefabs.Length >= 1)
            {
                EquipWeapon(0);
            }
        }

        // Public 메서드
        public void EquipWeapon(int index)
        {
            if (0 > index || index >= itemPrefabs.Length || itemPrefabs[index] == null)
            {
                Debug.LogError($"잘못된 인덱스 접근입니다. {index}");
                return;
            }

            UnequipWeapon();

            if (itemPrefabs[index] is IWeaponable weapon)
            {
                CurrentWeapon = weapon;

                IEquipAnchor equipAnchor = GetComponent<IEquipAnchor>();
                if (equipAnchor != null)
                {
                    CurrentEquipPreview = Instantiate(CurrentWeapon.WeaponData.previewPrefab);
                    CurrentEquipPreview.transform.SetParent(equipAnchor.GetRangedWeaponAttachPoint());
                    CurrentEquipPreview.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void UnequipWeapon()
        {
            if (CurrentEquipPreview == null)
                return;

            Destroy(CurrentEquipPreview);
            CurrentEquipPreview = null;
        }

        // Private 메서드
        // Others

    } // Scope by class CharacterInventory
} // namespace Root
