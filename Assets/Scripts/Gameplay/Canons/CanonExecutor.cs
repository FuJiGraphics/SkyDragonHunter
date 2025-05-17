 using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonExecutor : MonoBehaviour
        , IStateResetHandler
    {
        // 필드 (Fields)
        public float attackRange = 10f;

        private CanonBase m_CurrentEquipCanonInstance = null;
        private CanonDummy m_CurrentEquipCanonDummy = null;
        private GameObject m_EquipAnchorInstance = null;

        private Coroutine m_FireCoroutine = null;
        private Vector3 m_PrevFirePos;

        // 속성 (Properties)
        public bool IsEquip => m_CurrentEquipCanonInstance != null;
        public CanonBase CurrentEquipCanonInstance => m_CurrentEquipCanonInstance;

        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private IEquipAnchor m_EquipAnchor;
        [SerializeField] private IAttackTargetProvider m_AttackTargetProvider;
        [SerializeField] private EnemySearchProvider m_EnemySearchProvider;

        // 이벤트 (Events)
        public event Action<CanonDummy> onEquipEvents;
        public event Action onUnequipEvents;

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

        private void Start()
        {
            LoadUserData();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            m_FireCoroutine = null;
        }

        public void Equip(CanonDummy canonDummy)
        {
            if (canonDummy == null)
            {
                DrawableMgr.Dialog("Error", "[CanonExecutor]: Equip Error => canonDummy is null!");
                return;
            }
            m_CurrentEquipCanonDummy = canonDummy;
            m_CurrentEquipCanonDummy.IsEquip = true;

            var canonInstance = canonDummy.GetCanonInstance();
            if (canonInstance.TryGetComponent<CanonBase>(out var canonBase))
            {
                Unequip();
                if (m_EquipAnchor != null && canonBase.CanonDummy != null)
                {
                    var currentEquipPreview = Instantiate(canonBase.CanonDummy);
                    currentEquipPreview.transform.SetParent(m_EquipAnchor.GetRangedWeaponAttachPoint());
                    currentEquipPreview.transform.localPosition = Vector3.zero;
                    m_EquipAnchorInstance = currentEquipPreview;
                }
                m_CurrentEquipCanonInstance = canonBase;
                m_CurrentEquipCanonInstance.gameObject.SetActive(true);
                onEquipEvents?.Invoke(canonDummy);
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
                if (m_CurrentEquipCanonDummy != null)
                {
                    m_CurrentEquipCanonDummy.IsEquip = false;
                }
                Destroy(m_EquipAnchorInstance);
                m_EquipAnchorInstance = null;
                m_CurrentEquipCanonInstance = null;
                m_CurrentEquipCanonDummy = null;
                onUnequipEvents?.Invoke();
            }
        }

        // Public 메서드
        public void Execute()
        {
            if (m_CurrentEquipCanonDummy == null)
            {
                Unequip();
                return;
            }
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
            // 캐싱하지 않으면 코루틴과 오차 발생함.
            m_PrevFirePos = m_EquipAnchorInstance.transform.position;
            if (m_EquipAnchorInstance.TryGetComponent<IWeaponAnchorProvider>(out var anchor))
            {
                m_PrevFirePos = anchor.GetWeaponFirePoint().position;
            }

            // 거리 확인
            float distance = Vector2.Distance(target.transform.position, m_PrevFirePos);
            if (distance > attackRange)
                return;

            // 조준
            if (m_EquipAnchorInstance.TryGetComponent<LookAtable>(out var lookAtable))
            {
                lookAtable.LookAt(target);
                if (!lookAtable.IsTargetLook)
                {
                    return;
                }
            }

            if (m_FireCoroutine == null)
            {
                m_FireCoroutine = StartCoroutine(CoFire());
            }
        }

        public void ResetState()
        {
            StopAllCoroutines();
        }

        // Private 메서드
        private IEnumerator CoFire()
        {
            yield return new WaitForSeconds(m_CurrentEquipCanonInstance.CanonData.canCooldown);

            if (m_CurrentEquipCanonInstance != null)
            {
                m_CurrentEquipCanonInstance.Fire(m_PrevFirePos, gameObject, m_EnemySearchProvider.Target, m_AttackTargetProvider.AllowedTargetTags);
            }

            m_FireCoroutine = null;
        }

        private void LoadUserData()
        {
            foreach (var canonDummy in AccountMgr.HeldCanons)
            {
                if (canonDummy.IsEquip)
                {
                    Equip(canonDummy);
                    break;
                }
            }
        }

        // Others

    } // Scope by class CanonExecutor
} // namespace SkyDragonHunter.Gameplay