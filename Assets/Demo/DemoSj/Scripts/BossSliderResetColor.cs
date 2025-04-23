using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class BossSliderResetColor : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image bossHealthBackImage;
        [SerializeField] private Image bossHealthImage;
        [SerializeField] private Image bossTimerBackImage;
        [SerializeField] private Image bossTimerImage;
        private Color bossHealthBackColor;
        private Color bossHealthColor;
        private Color bossTimerBackColor;
        private Color bossTimerColor;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            bossHealthBackColor = bossHealthBackImage.color;
            bossHealthColor = bossHealthImage.color;
            bossTimerBackColor = bossTimerBackImage.color;
            bossTimerColor = bossTimerImage.color;
        }

        private void OnEnable()
        {
            bossHealthBackImage.color = bossHealthBackColor;
            bossHealthImage.color = bossHealthColor;
            bossTimerBackImage.color = bossTimerBackColor;
            bossTimerImage.color = bossTimerColor;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class BossSliderResetColor

} // namespace Root