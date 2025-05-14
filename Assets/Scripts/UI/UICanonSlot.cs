using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UICanonSlot : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image canonIcon;
        [SerializeField] private Sprite canonSlotIcon;
        [SerializeField] private Sprite canonDownIcon;
        [SerializeField] private Image canonLockIcon;

        // 속성 (Properties)
        public Image CanonIcon => canonIcon;
        public CanonDummy CanonDummy { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            UIUpdateUnlockState();
        }

        private void Update()
        {
            UIUpdateUnlockState();
        }

        // Public 메서드
        public void ShowDownIconImage()
        {
            GetComponent<Image>().sprite = canonDownIcon;
        }

        public void ShowIdleIconImage()
        {
            GetComponent<Image>().sprite = canonSlotIcon;
        }

        public void UIUpdateUnlockState()
        {
            if (CanonDummy != null && CanonDummy.IsUnlock)
            {
                canonLockIcon.gameObject.SetActive(false);
                GetComponent<Image>().color = Color.white;
                GetComponent<Button>().interactable = true;
            }
            else
            {
                var tutorialMgr = GameMgr.FindObject<TutorialMgr>("TutorialMgr");
                if (tutorialMgr != null)
                {
                    if (tutorialMgr.TutorialEnd)
                        GetComponent<Button>().interactable = true;
                    else
                        GetComponent<Button>().interactable = false;
                }

                canonLockIcon.gameObject.SetActive(true);
                GetComponent<Image>().color = Color.gray;
            }
        }
        // Private 메서드
        // Others

    } // Scope by class UICanonSlot
} // namespace SkyDragonHunter