using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class AutoAttack : MonoBehaviour
    {
        // 필드 (Fields)
        public string[] targetTags;
        private CharacterInventory m_Inventory;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_Inventory = GetComponent<CharacterInventory>();
        }

        private void Update()
        {
            if (m_Inventory == null)
            {
                Debug.Log("공격할 수 있는 무기가 존재하지 않습니다.");
                return;
            }

            foreach (string tag in targetTags)
            {
                var findGo = GameObject.FindWithTag(tag);
                if (findGo != null)
                {
                    m_Inventory.CurrentWeapon?.Execute(gameObject, findGo);
                }
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class AutoAttack
} // namespace Root
