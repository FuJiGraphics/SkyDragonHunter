using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonBase : MonoBehaviour 
    {
        // 필드 (Fields)
        [SerializeField] private CanonDefinition m_CanonDefinition;
        [SerializeField] private GameObject m_CanonDummy;
        [SerializeField] private BallBase m_BallPrefab;

        private Coroutine m_FireCoroutine = null;

        // 속성 (Properties)
        public CanonDefinition CanonData => m_CanonDefinition;
        public GameObject CanonDummy => m_CanonDummy;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_CanonDummy == null)
            {
                var canonDummyProvider = GetComponentInChildren<ICanonDummyProvider>(true);
                if (canonDummyProvider != null)
                {
                    m_CanonDummy = canonDummyProvider.CanonDummy;
                }
            }
        }

        // Public 메서드
        public void Fire(Vector3 firePos, GameObject attacker, GameObject target, string[] targetTags)
        {
            if (m_FireCoroutine != null)
                return;

            if (target == null)
            {
                Debug.LogWarning("[CanonBase]: 공격할 타겟을 찾을 수 없습니다.");
                return;
            }
            if (m_CanonDefinition == null)
            {
                Debug.LogError("[CanonBase]: CanonDefinition을 찾을 수 없습니다.");
                return;
            }
            if (m_BallPrefab == null)
            {
                Debug.LogError("[CanonBase]: Ball Prefab을 찾을 수 없습니다.");
                return;
            }
            if (targetTags == null || targetTags.Length <= 0)
            {
                Debug.LogError("[CanonBase]: Target Tag를 찾을 수 없습니다.");
                return;
            }

            // TODO: 나중에 오브젝트 풀 활용 예정
            m_FireCoroutine = StartCoroutine(FireCo(firePos, attacker, target, targetTags));
        }

        // Private 메서드
        private IEnumerator FireCo(Vector3 firePos, GameObject attacker, GameObject target, string[] targetTags)
        {
            BallBase ballInstance = GameObject.Instantiate(m_BallPrefab);
            if (ballInstance != null)
            {
                // TODO: 임시 구현
                ballInstance.Caster = attacker;
                ballInstance.Receiver = target;
                ballInstance.CanonData = CanonData;

                // 공격할 타겟 등록
                if (ballInstance.TryGetComponent<AttackTargetSelector>(out var selector))
                {
                    selector.SetAllowedTarget(targetTags);
                }

                // 발사 위치 설정
                ballInstance.transform.position = firePos;

                // 방향성이 있는지 체크 / 있다면 방향 설정함
                if (ballInstance.TryGetComponent<IDirectional>(out var dir))
                {
                    Vector2 aPos = firePos;
                    Vector2 dPos = ballInstance.Receiver.transform.position;
                    dir.SetDirection(dPos - aPos);
                }
            }
            yield return new WaitForSeconds(ballInstance.CanonData.canCooldown);
            m_FireCoroutine = null;
        }

        // Others

    } // Scope by class CanonBase
} // namespace SkyDragonHunter.Gameplay