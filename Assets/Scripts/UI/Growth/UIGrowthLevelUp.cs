using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIGrowthLevelUp : MonoBehaviour
    {
        // 필드 (Fields)
        private Animator m_Animator;
        private Image m_Image;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Image = GetComponent<Image>();
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UIGrowthLevelUp
} // namespace Root
