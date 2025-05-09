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

        [SerializeField] private float m_EndTime;

        private NewCrewControllerBT m_CurrentBT;
        private UISkillButtons m_SkillButtons;

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
            }
        }

        public bool IsCooldownComplete => Time.time >= m_EndTime;

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

        private void Start()
        {
            m_SkillButtons = GameMgr.FindObject<UISkillButtons>("UISkillButtons");
        }

        private void Update()
        {
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
            {
                Debug.LogWarning($"{gameObject.name} m_Slot Null");
                return;
            }

            if (m_EnemySearchProvider == null || m_EnemySearchProvider.Target == null)
            {
                if (m_EnemySearchProvider == null)
                {
                    Debug.LogWarning($"{gameObject.name} EnemySearchProvider Null");
                }
                else
                {
                    Debug.LogWarning($"{gameObject.name} EnemySearchProvider Target Null");
                }
                return;
            }

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

        // Private 메서드
        private void Init()
        {
            m_EnemySearchProvider = GetComponent<EnemySearchProvider>();
            m_SkillAnchor = GetComponent<SkillAnchorProvider>();
            ResetEndTime();

            if (TryGetComponent<NewCrewControllerBT>(out var bt))
            {
                m_CurrentBT = bt;
            }
        }

        private void ResetEndTime()
        {
            m_EndTime = Time.time + m_SkillCooldown;
        }

        // Others

    } // Scope by class SkillExecutor
} // namespace SkyDragonHunter