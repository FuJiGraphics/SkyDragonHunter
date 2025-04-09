using System.Collections;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class CanonAutoAttacker : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private CanonExecutor m_CanonExecutor;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            AutoAttack();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            if (m_CanonExecutor == null)
            {
                m_CanonExecutor = GetComponent<CanonExecutor>();
            }
        }

        private void AutoAttack()
        {
            if (m_CanonExecutor == null)
                return;

            if (m_CanonExecutor.IsEquip)
            {
                m_CanonExecutor.Execute();
            }
        }

        // Others

    } // Scope by class AutoAttack
} // namespace Root
