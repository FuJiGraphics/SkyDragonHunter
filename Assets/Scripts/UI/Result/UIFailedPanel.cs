using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UIFailedPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI m_StageNumber;
        [SerializeField] private TextMeshProUGUI m_StageName;
        [SerializeField] private Slider m_Slider;
        [SerializeField] private float m_LimitSeconds = 5f;

        private float m_StartTime = 0f;
        private float m_LastLimitTime = 0f;

        // 속성 (Properties)
        public string StageNumber
        {
            get => m_StageNumber.text;
            set => m_StageNumber.text = value;
        }

        public string StageName
        {
            get => m_StageName.text;
            set => m_StageName.text = value;
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_StartTime = Time.time;
            m_LastLimitTime = m_StartTime + m_LimitSeconds;
            m_Slider.value = 0f;
        }

        private void Update()
        {
            float elapsed = Time.time - m_StartTime;
            m_Slider.value = Mathf.Clamp01(elapsed / m_LimitSeconds);

            if (Time.time >= m_LastLimitTime)
            {
                m_LastLimitTime = 0f;
                var waveController = GameMgr.FindObject<TestWaveController>("WaveController");
                waveController.ReStartAll();
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UIFailedPanel
} // namespace Root
