using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonExecutor : MonoBehaviour
    {
        // 필드 (Fields)
        public float attackRange = 10f;

        private CanonBase m_CurrentEquipCanonInstance = null;
        private GameObject m_EquipAnchorInstance = null;

        // 속성 (Properties)
        public bool IsEquip => m_CurrentEquipCanonInstance != null;
        public CanonBase CurrentEquipCanonInstance => m_CurrentEquipCanonInstance;

        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private IEquipAnchor m_EquipAnchor;
        [SerializeField] private IAttackTargetProvider m_AttackTargetProvider;
        [SerializeField] private EnemySearchProvider m_EnemySearchProvider;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_EquipAnchor == null)
            {
                m_EquipAnchor = GetComponentInChildren<IEquipAnchor>();
            }
            if (m_EnemySearchProvider == null)
            {
                m_EnemySearchProvider = GetComponent<EnemySearchProvider>();
            }
            if (m_AttackTargetProvider == null)
            {
                m_AttackTargetProvider = GetComponent<IAttackTargetProvider>();
            }
        }

        public void Equip(GameObject canonInstance)
        {
            if (canonInstance.TryGetComponent<CanonBase>(out var canonBase))
            {
                if (m_EquipAnchor != null && canonBase.CanonDummy != null)
                {
                    var currentEquipPreview = Instantiate(canonBase.CanonDummy);
                    currentEquipPreview.transform.SetParent(m_EquipAnchor.GetRangedWeaponAttachPoint());
                    currentEquipPreview.transform.localPosition = Vector3.zero;
                    m_EquipAnchorInstance = currentEquipPreview;
                }
                m_CurrentEquipCanonInstance = canonBase;
                m_CurrentEquipCanonInstance.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log($"[CanonExecutor]: 장착 실패. CanonBase를 찾을 수 없습니다. {canonInstance}");
            }
        }

        public void Unequip()
        {
            if (m_EquipAnchorInstance != null)
            {
                Destroy(m_EquipAnchorInstance);
                m_EquipAnchorInstance = null;
                m_CurrentEquipCanonInstance = null;
                m_CurrentEquipCanonInstance.gameObject.SetActive(false);
            }
        }

        // Public 메서드
        public void Execute()
        {
            if (m_EnemySearchProvider == null)
            {
                Debug.Log("[CanonExecutor]: 발사 실패. EnemySearchProvider을 찾을 수 없습니다.");
                return;
            }
            if (m_EnemySearchProvider.Target == null)
            {
                return;
            }
            if (m_AttackTargetProvider == null)
            {
                Debug.Log("[CanonExecutor]: 발사 실패. AttackTargetSelector을 찾을 수 없습니다.");
                return;
            }
            if (m_CurrentEquipCanonInstance == null)
            {
                Debug.Log("[CanonExecutor]: 발사 실패. 장착된 캐논을 찾을 수 없습니다.");
                return;
            }

            var target = m_EnemySearchProvider.Target;

            // 발사 위치에 대한 앵커가 있는지 체크
            Vector3 firePos = m_EquipAnchorInstance.transform.position;
            if (m_EquipAnchorInstance.TryGetComponent<IWeaponAnchorProvider>(out var anchor))
            {
                firePos = anchor.GetWeaponFirePoint().position;
            }

            // 거리 확인
            float distance = Vector2.Distance(target.transform.position, firePos);
            if (distance > attackRange)
                return;

            // 조준
            if (m_EquipAnchorInstance.TryGetComponent<LookAtable>(out var lookAtable))
            {
                lookAtable.LookAt(target);
            }

            m_CurrentEquipCanonInstance.Fire(
                firePos,
                gameObject,
                m_EnemySearchProvider.Target,
                m_AttackTargetProvider.AllowedTargetTags);
        }

        // Private 메서드
        // Others

    } // Scope by class CanonExecutor
} // namespace SkyDragonHunter.Gameplay