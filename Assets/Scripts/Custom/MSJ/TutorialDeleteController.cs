using NPOI.SS.Formula.PTG;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class TutorialDeleteController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private GameObject[] m_DeleteList;
        [SerializeField] private GameObject[] m_DisableList;
        [SerializeField] private Button[] m_DeleteEvents;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if (SaveLoadMgr.GameData.savedTutorialData.tutorialCleared)
            {
                GameMgr.FindObject<TutorialMgr>("TutorialMgr").IsStartTutorial = false;
                Delete();
                Disable();
            }
        }
    
        // Public 메서드
        public void Delete()
        {
            foreach (var button in m_DeleteEvents)
            {
                button.onClick?.RemoveAllListeners();
            }
            foreach (var node in m_DeleteList)
            {
                GameObject.Destroy(node, 1f);
            }
        }

        public void Disable()
        {
            foreach (var node in m_DisableList)
            {
                node.SetActive(false);
            }
        }

        // Private 메서드
        // Others
    
    } // Scope by class TutorialDeleteController
} // namespace SkyDragonHunter