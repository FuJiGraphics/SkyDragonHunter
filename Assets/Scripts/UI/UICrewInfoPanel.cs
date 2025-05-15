using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Temp;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        // TODO: LJH - level up related
        private int m_cachedCrewID;
        [SerializeField] private string m_UiCrewLevelFormat;
        [SerializeField] private UiCrewLevelUpPanel m_UiCrewLevelUpPanel;
        // ~TODO

        [Header("Crew Details Status Settings")]
        [SerializeField] private GameObject m_UiCrewDetailsStatus;

        [Header("Crew Passive Effect Settings")]
        [SerializeField] private GameObject m_UiCrewPassiveEffect;

        [Header("Crew Preview Info Settings")]
        [SerializeField] private Image m_UiCrewPreviewInfo;

        [Header("Crew Skill Info Settings")]
        [SerializeField] private Button m_UiExitButton;
        [SerializeField] private Image m_UiCrewActiveSkillIcon;
        [SerializeField] private TextMeshProUGUI m_UiCrewActiveSkillNameText;
        [SerializeField] private TextMeshProUGUI m_UiCrewActiveSkillDescText;
        [SerializeField] private Image m_UiCrewPassiveSkillIcon;
        [SerializeField] private TextMeshProUGUI m_UiCrewPassiveSkillNameText;
        [SerializeField] private TextMeshProUGUI m_UiCrewPassiveSkillDescText;
        [SerializeField] private Button m_UiCrewLevelUpButton;

        [Header("Crew List Panel Settings")]
        [SerializeField] private GameObject m_UiCrewListContent;
        [SerializeField] private GameObject m_UiCrewListNodePrefab;

        private List<CrewNode> m_CrewListNodeObjects = new();
        private Button m_PrevClickButton = null;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (m_PrevClickButton == null)
            {
                if (m_CrewListNodeObjects != null && m_CrewListNodeObjects.Count > 0)
                {
                    Button button = m_CrewListNodeObjects.First().crewNode.GetComponent<Button>();
                    button.onClick.Invoke();
                }
            }
            else
            {
                m_PrevClickButton?.onClick?.Invoke();
            }
            SortedEquipCrewNode();
        }

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

        public void SetLevel(int level)
            => m_UiCrewLevel.text = m_UiCrewLevelFormat + level.ToString();

        public void SetMountedState(bool isMounted)
        {
            if (isMounted)
                m_UiCrewReady.sprite = m_UiCrewReadyYes;
            else
                m_UiCrewReady.sprite = m_UiCrewReadyNo;
        }

        public void SetSkillInfo(SkillDefinition skill)
        {
            if (skill == null)
            {
                m_UiCrewActiveSkillIcon.sprite = null;
                m_UiCrewActiveSkillNameText.text = "없음";
                m_UiCrewActiveSkillDescText.text = "없음";
                m_UiCrewPassiveSkillIcon.sprite = null;
                m_UiCrewPassiveSkillNameText.text = "없음";
                m_UiCrewPassiveSkillDescText.text = "없음";
            }
            else
            {
                m_UiCrewActiveSkillIcon.sprite = skill.ActiveSkillIcon;
                m_UiCrewActiveSkillNameText.text = skill.skillActiveName;
                m_UiCrewActiveSkillDescText.text = skill.skillActiveDesc;
                m_UiCrewPassiveSkillIcon.sprite = skill.PassiveSkillIcon;
                m_UiCrewPassiveSkillNameText.text = skill.skillPassiveName;
                m_UiCrewPassiveSkillDescText.text = skill.skillPassiveDesc;
            }
        }

        public void AddCrewNode(GameObject crew)
        {
            if (m_CrewListNodeObjects == null)
            {
                m_CrewListNodeObjects = new List<CrewNode>();
            }

            if (crew.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                // TODO: LJH
                var crewBT = crew.GetComponent<NewCrewControllerBT>();
                if (crewBT == null)
                {
                    Debug.LogWarning($"[UICrewInfoPanel]: Cannot find crewBT of '{gameObject.name}'");
                }
                int crewID = crewBT.ID;
                if(!TempCrewLevelExpContainer.TryGetTempCrewData(crewID, out var tempCrewLevelData))
                {
                    Debug.LogWarning($"[UICrewInfoPanel]: Cannot find crew Level data of '{gameObject.name}', id: {crewID} ");
                }
                var crewData = tempCrewLevelData.CrewData;
                if(crewData == null)
                {
                    Debug.LogWarning($"[UICrewInfoPanel]: Cannot find crew data of '{gameObject.name}', id: {crewID} ");
                }
                // ~TODO

                GameObject nodeGo = Instantiate<GameObject>(m_UiCrewListNodePrefab);
                m_CrewListNodeObjects.Add(new CrewNode{ crewNode = nodeGo, crewInstance = crew });
                if (nodeGo.TryGetComponent<Image>(out var nodeIcon))
                {
                    nodeIcon.sprite = provider.Icon;
                }
                if (nodeGo.TryGetComponent<Button>(out var nodeButton))
                {
                    nodeButton.onClick.AddListener(() => {
                        // TODO: LJH
                        //SetName(provider.Name);
                        //SetDamage(provider.Damage);
                        //SetHealth(provider.Health);
                        //SetDefense(provider.Defense);
                        //SetPreview(provider.Preview);
                        //SetMountedState(provider.IsEquip);
                        //SetSkillInfo(provider.Skill);

                        SetName(crewData.UnitName);
                        SetDamage(provider.Damage);
                        SetHealth(provider.Health);
                        SetDefense(provider.Defense);
                        SetPreview(provider.Preview);
                        SetMountedState(provider.IsEquip);
                        SetSkillInfo(provider.Skill);
                        SetLevel(tempCrewLevelData.Level);
                        
                        m_cachedCrewID = crewID;

                        if (m_UiCrewLevelUpPanel.gameObject.activeSelf)
                            m_UiCrewLevelUpPanel.SetPanel(m_cachedCrewID);
                        // ~TODO
                        m_PrevClickButton = nodeButton;
                    });
                }
                nodeGo.transform.SetParent(m_UiCrewListContent.transform);
                nodeIcon.transform.localScale = Vector3.one;
            }
            else
            {
                Debug.LogWarning("[UICrewInfoPanel]: ICrewInfoProvider를 찾을 수 없습니다.");
            }
        }

        public void SortedEquipCrewNode()
        {
            foreach (var node in m_CrewListNodeObjects)
            {
                if (node.crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
                {
                    if (provider.IsEquip)
                    {
                        node.crewNode.transform.SetAsFirstSibling();
                    }
                }
            }
        }

        public void SetClickNode(GameObject crewInstance)
        {
            foreach (var node in m_CrewListNodeObjects)
            {
                if (node.crewNode.TryGetComponent<Button>(out var button))
                {
                    if (node.crewInstance == crewInstance)
                    {
                        button.onClick.Invoke();
                        m_PrevClickButton = button;
                    }
                }
            }
        }

        // TODO: LJH LevelUP Panel
        public void OnClickLevelUpButton()
        {
            m_UiCrewLevelUpPanel.gameObject.SetActive(true);
            m_UiCrewLevelUpPanel.SetPanel(m_cachedCrewID);
        }

        public void OnLevelUp()
        {
            m_PrevClickButton.onClick.Invoke();
        }
        // ~TODO
        
        // Private 메서드
        // Others

    } // Scope by class UICharacterInfoPanel
} // namespace SkyDragonHunter