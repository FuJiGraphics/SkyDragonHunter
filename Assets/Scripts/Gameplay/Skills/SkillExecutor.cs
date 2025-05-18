using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillExecutor : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private SkillBase m_Skill;
        [SerializeField] private string[] m_TargetTags;
        [SerializeField] private float m_SkillCooldown;
        [SerializeField] private bool m_IsAutoExecute;
        [SerializeField] private float m_Distance;


        private float m_CooldownTimer = 0f;
        private float m_CooldownRec = 1f;

        // 속성 (Properties)
        public float CooldownProgress => Mathf.Clamp01(m_CooldownTimer / m_SkillCooldown);
        public bool IsCooldownComplete => m_CooldownTimer >= m_SkillCooldown;
        public bool IsChaseMode => m_Skill.IsInstantCast;

        public bool IsAutoExecute
        {
            get
            {
                return m_IsAutoExecute;
            }
            set
            {
                m_IsAutoExecute = value;
            }
        }

        public SkillDefinition SkillData => m_Skill.SkillData;
        public SkillType SkillType => m_Skill != null ? m_Skill.SkillType : SkillType.Undefined;
        public bool IsNull => m_Skill == null;

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
            if (m_CooldownTimer < m_SkillCooldown)
                m_CooldownTimer += Time.deltaTime * m_CooldownRec;

            if (IsAutoExecute)
            {
                Execute();
            }
        }

        // Public 메서드
        public void Execute()
        {
            if (!IsCooldownComplete)
                return;

            // TODO: 임시적으로 단원 장착 시에만 발사하도록 조건
            //if (TryGetComponent<CrewEquipmentController>(out var crewEquipComp))
            //{
            //    if (!crewEquipComp.IsEquip)
            //        return;
            //}

            if (m_Skill == null)
                return;

            if (m_EnemySearchProvider == null || m_EnemySearchProvider.Target == null)
                return;

            if (SkillType.Affect == SkillType)
            {
                if (TryGetComponent<BuffExecutor>(out var buffExecutor))
                {
                    // 현재 버프가 시전중이면 스킬 진행 취소함
                    if (buffExecutor.HasBuff(SkillData.BuffData))
                    {
                        Debug.LogWarning($"{gameObject.name} Has Buff");
                        return;
                    }
                }
                else
                {
                    Debug.LogError($"[SkillExecutor]: Buff를 사용할 BuffExecutor 컴포넌트를 찾을 수 없습니다. Caster: {gameObject}");
                    return;
                }
            }

            float targetDistance = 0f;
            if (SkillType.Damage == SkillType)
            {
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
            }

            ResetEndTime();
            //Debug.LogWarning($"{gameObject.name} Skill Reset End Time");
            SkillBase skill = Instantiate(m_Skill);
            skill.Init(gameObject, m_EnemySearchProvider.Target);
            // 스킬을 적용 받을 타겟을 등록
            if (skill.TryGetComponent<AttackTargetSelector>(out var selector))
            {
                // 타겟팅 설정이 Caster에 종속적인지 체크
                if (!selector.IsSelfTargeting)
                {
                    selector.SetAllowedTarget(m_TargetTags);
                }
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

        public void AdjustCooldownDecreaseStatus(float weight)
        {
            m_CooldownRec = weight;
        }

        public void RollbackCooldownDecreaseStatus()
        {
            m_CooldownRec = 1f;
        }

        // Private 메서드
        private void Init()
        {
            m_EnemySearchProvider = GetComponent<EnemySearchProvider>();
            m_SkillAnchor = GetComponent<SkillAnchorProvider>();
            ResetEndTime();
        }

        private void ResetEndTime()
        {
            m_CooldownTimer = 0f;
        }

        // Others

    } // Scope by class SkillExecutor
} // namespace SkyDragonHunter