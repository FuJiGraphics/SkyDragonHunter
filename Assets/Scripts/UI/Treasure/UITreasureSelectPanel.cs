using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public enum ArtifactSortedTypes
    {
        GetASC,     // 획득(오름차순)
        GetDESC,    // 획득(내림차순)
        GradeASC,   // 등급(오름차순)
        GradeDESC,  // 등급(내림차순)
        EquipASC,   // 장착(오름차순)
        EquipDESC,  // 장착(내림차순)
    }

    public class UITreasureSelectPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureEquipmentSlotPanel m_UiTreasureEquipPanel;
        [SerializeField] private UITreasureInfo m_UiTreasureInfo;
        [SerializeField] private UITreasureSlot m_SlotPrefab;
        [SerializeField] private Transform m_Content;
        [SerializeField] private TMP_Dropdown m_SortedDropdown;


        private ArtifactSortedTypes m_SortedType;
        private Dictionary<ArtifactDummy, UITreasureSlot> m_GenMap;
        private List<KeyValuePair<UITreasureSlot, ArtifactDummy>> m_SortedByAcquiredTime = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SortedDropdown.onValueChanged.RemoveAllListeners();
            m_SortedDropdown.onValueChanged.AddListener(OnChangedSortedType);
            m_SortedType = (ArtifactSortedTypes)m_SortedDropdown.value;
        }

        // Public 메서드
        public void AddSlot(ArtifactDummy artifact)
        {
            if (m_GenMap == null)
                m_GenMap = new Dictionary<ArtifactDummy, UITreasureSlot>();

            if (m_GenMap.ContainsKey(artifact))
            {
                DrawableMgr.Dialog("Alert", $"[UITreasureSelect]: 이미 추가된 슬롯입니다. {artifact}");
                return;
            }

            if (m_UiTreasureInfo == null)
            {
                DrawableMgr.Dialog("Error", $"[UITreasureSelect]: TreasureInfo가 null입니다!");
                return;
            }

            var instance = Instantiate(m_SlotPrefab);
            instance.TargetEquipPanel = m_UiTreasureEquipPanel;
            instance.TargetInfoPanel = m_UiTreasureInfo;
            instance.SetSlot(artifact);
            instance.transform.SetParent(m_Content);
            m_GenMap.Add(artifact, instance);
            m_SortedByAcquiredTime.Add(new(instance, artifact));
        }

        public void RemoveSlot(ArtifactDummy artifact)
        {
            if (m_GenMap == null)
                return;

            if (!m_GenMap.ContainsKey(artifact))
            {
                DrawableMgr.Dialog("Alert", $"[UITreasureSelect]: 슬롯을 찾을 수 없습니다. {artifact}");
                return;
            }
            Destroy(m_GenMap[artifact]);
            m_GenMap.Remove(artifact);
        }

        public void OnChangedSortedType(int type)
        {
            ArtifactSortedTypes sortedType = (ArtifactSortedTypes)type;
            switch (sortedType)
            {
                case ArtifactSortedTypes.GetASC:
                    OrderByGet(false);
                    break;
                case ArtifactSortedTypes.GetDESC:
                    OrderByGet(true);
                    break;
                case ArtifactSortedTypes.GradeASC:
                    OrderByGrade(false);
                    break;
                case ArtifactSortedTypes.GradeDESC:
                    OrderByGrade(true);
                    break;
                case ArtifactSortedTypes.EquipASC:
                    break;
                case ArtifactSortedTypes.EquipDESC:
                    break;
            }
        }


        public void OrderByGet(bool isDescending)
        {
            foreach (var element in m_SortedByAcquiredTime)
            {
                if (isDescending)
                    element.Key.transform.SetAsLastSibling();
                else
                    element.Key.transform.SetAsFirstSibling();
            }
        }

        public void OrderByGrade(bool isDescending)
        {
            List<KeyValuePair<UITreasureSlot, ArtifactDummy>> sortedList = null;
            if (isDescending)
                sortedList = m_SortedByAcquiredTime.OrderByDescending(a => a.Value.Grade).ToList();
            else
                sortedList = m_SortedByAcquiredTime.OrderBy(a => a.Value.Grade).ToList();

            foreach (var element in sortedList)
            {
                element.Key.transform.SetAsFirstSibling();
            }
        }
        // Private 메서드


        // Others

    } // Scope by class UITreasureSelect
} // namespace SkyDragonHunter