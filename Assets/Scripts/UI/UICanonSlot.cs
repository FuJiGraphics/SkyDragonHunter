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
                if (canonLockIcon != null)
                {
                    canonLockIcon.enabled = false;
                }
                else
                {
                    canonIcon.color = Color.white;
                }
            }
            else
            {
                if (canonLockIcon != null)
                {
                    canonLockIcon.enabled = true;
                }
                else
                {
                    canonIcon.color = Color.gray;
                }
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UICanonSlot
} // namespace SkyDragonHunter