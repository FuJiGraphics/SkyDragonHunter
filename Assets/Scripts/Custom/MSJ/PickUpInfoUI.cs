using NPOI.SS.Formula.Functions;
using SkyDragonHunter;
using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class PickUpInfoUI : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI crewNameText;
        [SerializeField] private TextMeshProUGUI crewCountText;
        [SerializeField] private Image crewIcon;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public void SetData(string name, int count)
        {
            crewNameText.text = name;
            crewCountText.text = count.ToString();
        }

        public void SetDataWithCrewID(int crewID, int count)
        {
            crewNameText.text = DataTableMgr.CrewTable.Get(crewID).UnitName;
            // Need to assign crew Icon
            //DataTableMgr.CrewTable.Get(crewID).
            crewCountText.text = $"신규 획득!";            
        }

        public void SetDataForMasteryResource(int count)
        {
            crewNameText.text = $"마스터리 재화";
            crewCountText.text = count.ToString();
        }

        public void SetDataWithCannonId(CanonGrade grade, CanonType type, int count = 0)
        {
            crewNameText.text = $"{grade} / {type}";
            switch (grade)
            {
                case CanonGrade.Normal:
                    crewNameText.color = Color.green;
                    break;
                case CanonGrade.Rare:
                    crewNameText.color = Color.blue;
                    break;
                case CanonGrade.Unique:
                    crewNameText.color = Color.magenta;
                    break;
                case CanonGrade.Legend:
                    crewNameText.color = Color.yellow;
                    break;
            }
            if (count == 0)
                crewCountText.text = "";
            else
                crewCountText.text = count.ToString();
        }

        public void SetDataWithRepairId(RepairGrade grade, RepairType type, int count = 0)
        {
            crewNameText.text = $"{grade} / {type}";
            switch (grade)
            {
                case RepairGrade.Normal:
                    crewNameText.color = Color.green;
                    break;
                case RepairGrade.Rare:
                    crewNameText.color = Color.blue;
                    break;
                case RepairGrade.Unique:
                    crewNameText.color = Color.magenta;
                    break;
                case RepairGrade.Legend:
                    crewNameText.color = Color.yellow;
                    break;
            }
            if (count == 0)
                crewCountText.text = "";
            else
                crewCountText.text = count.ToString();
        }

        public void SetDataWithRepairId(int repairId)
        {

        }
        // Private 메서드
        // Others

    } // Scope by class PickUpInfoUI

} // namespace Root