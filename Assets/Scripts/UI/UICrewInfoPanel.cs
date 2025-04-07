using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UICrewInfoPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Crew Status bar Settings")]
        [SerializeField] private TextMeshProUGUI m_UiCrewLevel;
        [SerializeField] private TextMeshProUGUI m_UiCrewGrade;
        [SerializeField] private Image m_UiCrewReady;
        [SerializeField] private Sprite m_UiCrewReadyYes;
        [SerializeField] private Sprite m_UiCrewReadyNo;

        [Header("Crew Main Status Settings")]
        [SerializeField] private string m_UiCrewNameFormat;
        [SerializeField] private TextMeshProUGUI m_UiCrewName;
        [SerializeField] private string m_UiCrewDamageFormat;
        [SerializeField] private TextMeshProUGUI m_UiCrewDamage;
        [SerializeField] private string m_UiCrewHealthFormat;
        [SerializeField] private TextMeshProUGUI m_UiCrewHealth;
        [SerializeField] private string m_UiCrewDefenseFormat;
        [SerializeField] private TextMeshProUGUI m_UiCrewDefense;

        [Header("Crew Details Status Settings")]
        [SerializeField] private GameObject m_UiCrewDetailsStatus;

        [Header("Crew Passive Effect Settings")]
        [SerializeField] private GameObject m_UiCrewPassiveEffect;

        [Header("Crew Preview Info Settings")]
        [SerializeField] private Image m_UiCrewPreviewInfo;

        [Header("Crew Skill Info Settings")]
        [SerializeField] private Button m_UiExitButton;
        [SerializeField] private GameObject m_UiCrewActiveSkill;
        [SerializeField] private GameObject m_UiCrewPassiveSkill;
        [SerializeField] private Button m_UiCrewLevelUpButton;

        [Header("Crew List Panel Settings")]
        [SerializeField] private GameObject m_UiCrewListContent;
        [SerializeField] private GameObject m_UiCrewListNodePrefab;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetName(string name)
            => m_UiCrewName.text = m_UiCrewNameFormat + name;

        public void SetDamage(string damage)
            => m_UiCrewDamage.text = m_UiCrewDamageFormat + damage;

        public void SetHealth(string health)
            => m_UiCrewHealth.text = m_UiCrewHealthFormat + health;

        public void SetDefense(string defense)
            => m_UiCrewDefense.text = m_UiCrewDefenseFormat + defense;

        public void SetPreview(Sprite sprite)
            => m_UiCrewPreviewInfo.sprite = sprite;

        public void SetMountedState(bool isMounted)
        {
            if (isMounted)
                m_UiCrewReady.sprite = m_UiCrewReadyYes;
            else
                m_UiCrewReady.sprite = m_UiCrewReadyNo;
        }

        public void AddCrewNode(GameObject crew)
        {
            if (crew.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                GameObject nodeGo = Instantiate<GameObject>(m_UiCrewListNodePrefab);
                if (nodeGo.TryGetComponent<Image>(out var nodeIcon))
                {
                    nodeIcon.sprite = provider.Icon;
                }
                if (nodeGo.TryGetComponent<Button>(out var nodeButton))
                {
                    nodeButton.onClick.AddListener(() => { 
                        SetName(provider.Name);
                        SetDamage(provider.Damage);
                        SetHealth(provider.Health);
                        SetDefense(provider.Defense);
                        SetPreview(provider.Preview);
                        SetMountedState(provider.IsMounted);
                    });
                }
                nodeGo.transform.SetParent(m_UiCrewListContent.transform);
            }
            else
            {
                Debug.LogWarning("[UICrewInfoPanel]: ICrewInfoProvider를 찾을 수 없습니다.");
            }
        }
        
        // Private 메서드
        // Others

    } // Scope by class UICharacterInfoPanel
} // namespace SkyDragonHunter