using SkyDragonHunter.Gameplay;
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
        private void Update()
        {
            if (CanonDummy.IsUnlock)
            {
                canonLockIcon.enabled = false;
                canonIcon.color = Color.white;
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                canonLockIcon.enabled = true;
                canonIcon.color = Color.gray;
                GetComponent<Image>().color = Color.gray;
            }
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

        // Private 메서드
        // Others

    } // Scope by class UICanonSlot
} // namespace SkyDragonHunter