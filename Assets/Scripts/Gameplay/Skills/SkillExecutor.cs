using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    [System.Serializable]
    public class SkillSlotUI
    {
        public Button button;       // 스킬이 발동할 버튼
        public Slider cooldown;     // 스킬 쿨다운
        public SkillBase skill;     // 스킬 프리팹에 붙어있는 스킬 베이스
        public string[] targetTags;
    }

    public class SkillExecutor : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private SkillSlotUI[] m_Slots;
        [SerializeField] private float m_SkillCooldown;
        [SerializeField] private bool m_IsAutoExecute;
        [SerializeField] private float m_Distance;

        private float m_EndTime;
        // 속성 (Properties)
        public float CooldownProgress
        {
            get
            {
                if (Time.time >= m_EndTime)
                    return 1f;
                return 1f - Mathf.Clamp01((m_EndTime - Time.time) / m_SkillCooldown);
            }
        }

        public bool IsAutoExecute
        {
            get
            {
                return m_IsAutoExecute;
            }
            set
            {
                m_IsAutoExecute = value;
                ActiveButtons(!value);
            }
        }
        public bool IsCooldownComplete => Time.time >= m_EndTime;

        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private EnemySearchProvider m_EnemySearchProvider;
        private SkillAnchorProvider m_SkillAnchor;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (IsAutoExecute)
            {
                for (int i = 0; i < m_Slots.Length; ++i)
                {
                    Execute(i);
                }
            }
        }

        // Public 메서드
        public void Execute(int index)
        {
            if (!IsCooldownComplete)
            {
                UpdateCooldownSlider(index);
                return;
            }

            // TODO: 임시적으로 단원 장착 시에만 발사하도록 조건
            if (TryGetComponent<CrewEquipmentController>(out var crewEquipComp))
            {
                if (!crewEquipComp.IsEquip)
                    return;
            }

            if (m_Slots == null || index < 0 || index > m_Slots.Length)
                return;
            if (m_EnemySearchProvider.Target == null)
                return;

            float targetDistance = 0f;
            if (m_SkillAnchor != null)
            {
                targetDistance = Vector2.Distance(
                    m_SkillAnchor.firePoint.position, 
                    m_EnemySearchProvider.Target.transform.position);
            }
            else
            {
                targetDistance = Vector2.Distance(
                    transform.position, 
                    m_EnemySearchProvider.Target.transform.position);
            }

            if (targetDistance > m_Distance)
                return;

            ResetEndTime();

            // TODO: 임시 구현
            SkillBase skill = Instantiate(m_Slots[index].skill);
            skill.Caster = gameObject;
            skill.Receiver = m_EnemySearchProvider.Target;
            // 스킬을 적용 받을 타겟을 등록
            if (skill.TryGetComponent<AttackTargetSelector>(out var selector))
            {
                selector.SetAllowedTarget(m_Slots[index].targetTags);
            }

            // 스킬 시전 위치에 대한 앵커가 있는지 체크
            Vector3 firePos = skill.Caster.transform.position;
            if (skill.Caster.TryGetComponent<ISkillAnchorProvider>(out var anchor))
            {
                firePos = anchor.GetSkillFirePoint().position;
            }
            skill.transform.position = firePos;

            // 방향성이 있는지 체크 / 있다면 방향 설정함
            if (skill.TryGetComponent<IDirectional>(out var dir))
            {
                Vector2 aPos = firePos;
                Vector2 dPos = skill.Receiver.transform.position;
                dir.SetDirection(dPos - aPos);
            }
        }

        // Private 메서드
        private void Init()
        {
            for (int i = 0; i < m_Slots.Length; ++i)
            {
                int capturedIndex = i;
                m_Slots[i].button.onClick.AddListener(() => { Execute(capturedIndex); });
            }
            m_EnemySearchProvider = GetComponent<EnemySearchProvider>();
            m_SkillAnchor = GetComponent<SkillAnchorProvider>();
            ResetEndTime();
            ActiveButtons(!m_IsAutoExecute);
        }

        private void UpdateCooldownSlider(int slotIndex)
        {
            if (m_Slots == null || slotIndex < 0 || slotIndex > m_Slots.Length)
                return;
            if (m_Slots[slotIndex].cooldown == null)
                return;

            m_Slots[slotIndex].cooldown.value = 1f - CooldownProgress;
        }

        private void ResetEndTime()
        {
            m_EndTime = Time.time + m_SkillCooldown;
        }

        private void ActiveButtons(bool enabled)
        {
            foreach (var slot in m_Slots)
            {
                slot.button.enabled = enabled;
            }
        }

        // Others

    } // Scope by class SkillExecutor
} // namespace SkyDragonHunter