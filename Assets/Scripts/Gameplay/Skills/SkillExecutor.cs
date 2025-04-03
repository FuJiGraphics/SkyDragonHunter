using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    [System.Serializable]
    public class SkillSlotUI
    {
        public Button button;
        public SkillBase skill;
        public string[] targetTags;
    }

    public class SkillExecutor : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private SkillSlotUI[] m_Slots;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private EnemySearchProvider m_EnemySearchProvider;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void Execute(int index)
        {
            if (m_Slots == null || index < 0 || index > m_Slots.Length)
                return;
            if (m_EnemySearchProvider.Target == null)
                return;

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
        }

        // Others

    } // Scope by class SkillExecutor
} // namespace SkyDragonHunter