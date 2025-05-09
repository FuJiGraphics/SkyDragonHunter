using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class TutorialButtonBlockActiveFalse : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TutorialMgr tutorialMgr;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (tutorialMgr != null)
            {
                tutorialMgr = FindAnyObjectByType<TutorialMgr>();
            }
        }

        private void Update()
        {
            if (!tutorialMgr.IsStartTutorial)
                return;
            
            if (this.gameObject.name == "SkillButton1Block")
            {
                if (tutorialMgr.step > 45)
                {
                    Destroy(this.gameObject);
                }

            }
            else if (this.gameObject.name == "CanonInfoBlock")
            {
                if (tutorialMgr.step > 74)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "EquipAndLevelUpButtonBlock")
            {
                if (tutorialMgr.step > 79)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "LevelUpSliderRectBlock")
            {
                if (tutorialMgr.step > 92)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "LevelUpButtonBlock")
            {
                if (tutorialMgr.step > 94)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "GrowthPanelBlock")
            {
                if (tutorialMgr.step > 95)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "LevelUpPanelBlock")
            {
                if (tutorialMgr.step > 97)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "ATKPanelBlock")
            {
                if (tutorialMgr.step > 99)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "KichenBlock")
            {
                if (tutorialMgr.step > 108)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "KichenBlock")
            {
                if (tutorialMgr.step > 108)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "GearFactoryBlock" ||
                this.gameObject.name == "CanningPlantBlock" ||
                this.gameObject.name == "MagicWoeksBlock")
            {
                if (tutorialMgr.step > 112)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "DungeonInfoPanelBlock")
            {
                if (tutorialMgr.step > 124)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "FacilityLevelUpInfoPanelBlock")
            {
                if (tutorialMgr.step > 119)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "Dungeon1Block")
            {
                if (tutorialMgr.step > 126)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "Dungeon2Block")
            {
                if (tutorialMgr.step > 128)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "Dungeon3Block")
            {
                if (tutorialMgr.step > 130)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "StageSelectPanelBlock")
            {
                if (tutorialMgr.step > 132)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (this.gameObject.name == "EntryButtonsBlock")
            {
                if (tutorialMgr.step > 134)
                {
                    Destroy(this.gameObject);
                }
            }

        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class TutorialButtonBlockActiveFalse

} // namespace Root