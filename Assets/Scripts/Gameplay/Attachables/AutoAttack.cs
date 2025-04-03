using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static NPOI.HSSF.Util.HSSFColor;

namespace SkyDragonHunter.Gameplay
{
    public class AutoAttack : MonoBehaviour
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private CharacterInventory m_Inventory;
        [SerializeField] private EnemySearchProvider m_EnemySearchProvider;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (m_Inventory == null)
            {
                Debug.Log("공격할 수 있는 무기가 존재하지 않습니다.");
                return;
            }

            AttackTarget();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_Inventory = GetComponent<CharacterInventory>();
            m_EnemySearchProvider = GetComponent<EnemySearchProvider>();
        }

        private void AttackTarget()
        {
            GameObject target = m_EnemySearchProvider.Target;
            if (target != null)
            {
                m_Inventory.CurrentWeapon?.Execute(gameObject, target);
            }
        }

        // Others

    } // Scope by class AutoAttack
} // namespace Root
