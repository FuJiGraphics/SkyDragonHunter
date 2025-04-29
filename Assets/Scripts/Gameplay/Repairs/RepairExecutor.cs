using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using System.Collections;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class RepairExecutor : MonoBehaviour
        , IStateResetHandler
    {
        // 필드 (Fields)
        [Tooltip("수리공이 실드를 다시 채워주는 시간 간격")]
        [SerializeField] private float m_ShieldDuration = 8f;

        private RepairDummy m_CurrentEquipRepairInstance = null;

        private Coroutine m_RecoverCoroutine = null;
        private Coroutine m_DivineCoroutine = null;
        private Coroutine m_ShieldCoroutine = null;

        // 속성 (Properties)
        public bool IsEquip => m_CurrentEquipRepairInstance != null;
        public RepairDummy CurrentEquipRepairDummy => m_CurrentEquipRepairInstance;
        public bool IsActiveDivineShield { get; set; } = false;

        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private CharacterStatus m_Status;

        // 이벤트 (Events)
        public event Action<RepairDummy> onEquipEvents;
        public event Action onUnequipEvents;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_Status == null)
            {
                m_Status = GetComponent<CharacterStatus>();
            }
        }

        private void Update()
        {
            Execute();
        }

        private void Start()
        {
            LoadUserData();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            m_RecoverCoroutine = null;
        }

        public void Equip(RepairDummy repairDummy)
        {
            if (repairDummy == null)
            {
                DrawableMgr.Dialog("Error", "[RepairExecutor]: Equip Error => repairDummy is null!");
                return;
            }
            Unequip();
            m_CurrentEquipRepairInstance = repairDummy;
            m_CurrentEquipRepairInstance.IsEquip = true;

            var reapirData = repairDummy.GetData();
            onEquipEvents?.Invoke(repairDummy);
        }

        public void Unequip()
        {
            if (m_CurrentEquipRepairInstance != null)
            {
                m_CurrentEquipRepairInstance.IsEquip = false;
            }
            m_CurrentEquipRepairInstance = null;
            onUnequipEvents?.Invoke();
        }

        // Public 메서드
        public void Execute()
        {
            if (m_CurrentEquipRepairInstance == null)
            {
                Unequip();
                return;
            }

            if (m_RecoverCoroutine == null)
            {
                m_RecoverCoroutine = StartCoroutine(CoRecover());
            }

            if (m_CurrentEquipRepairInstance.Type == RepairType.Divine)
            {
                m_Status.MaxShield = 0;
                m_Status.ResetShield();
                if (IsActiveDivineShield == false && m_DivineCoroutine == null)
                {
                    m_DivineCoroutine = StartCoroutine(CoDivine());
                }
            }
            else if (m_CurrentEquipRepairInstance.Type == RepairType.Shield)
            {
                IsActiveDivineShield = false;
                if (m_ShieldCoroutine == null && m_Status.Shield <= 0)
                {
                    m_ShieldCoroutine = StartCoroutine(CoShield());
                }
            }

        }

        public void ResetState()
        {
            StopAllCoroutines();
        }

        // Private 메서드
        private IEnumerator CoRecover()
        {
            yield return new WaitForSeconds(1f);

            if (!m_Status.IsFullHealth)
            {
                m_Status.Health = (m_Status.Health + m_Status.MaxResilient);

                // TODO: 테스트 용
                DrawableMgr.Text(transform.position, "Recover " + m_Status.MaxResilient.ToUnit() + "++", Color.green);
            }

            m_RecoverCoroutine = null;
        }

        private IEnumerator CoDivine()
        {
            if (m_CurrentEquipRepairInstance == null)
            {
                IsActiveDivineShield = false;
                yield break;
            }
            float duration = m_CurrentEquipRepairInstance.GetData().RepDivineStride;
            yield return new WaitForSeconds(duration);
            IsActiveDivineShield = true;
            m_DivineCoroutine = null;
        }
        private IEnumerator CoShield()
        {
            yield return new WaitForSeconds(m_ShieldDuration);
            if (m_CurrentEquipRepairInstance == null)
            {
                m_Status.MaxShield = 0;
                m_Status.ResetShield();
                yield break;
            }
            m_Status.MaxShield = m_Status.MaxHealth * new BigNum(m_CurrentEquipRepairInstance.GetData().RepShield);
            m_Status.ResetShield();
            m_ShieldCoroutine = null;
        }

        private void LoadUserData()
        {
            foreach (var repairDummy in AccountMgr.HeldRepairs)
            {
                if (repairDummy.IsEquip)
                {
                    Equip(repairDummy);
                    break;
                }
            }
        }

        // Others

    } // Scope by class RepairExecutor
} // namespace SkyDragonHunter.Gameplay