using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIHealthBar : MonoBehaviour
        , IStateResetHandler
    {
        // 필드 (Fields)
        public BigNum maxValue;
        public BigNum currentValue;
        [SerializeField] Slider m_Slider;
        [SerializeField] Image m_FillImage;

        // 속성 (Properties)
        public Slider Slider => m_Slider;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        [Tooltip("CurrentHealth가 0f가 되었을 때 호출되는 이벤트")]
        [SerializeField] private UnityEvent OnHealthDepleted;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_Slider == null)
            {
                m_Slider = GetComponentInChildren<Slider>();
            }
            ResetValue();
        }

        private void Reset()
        {
            ResetValue();
        }

        // Public 메서드
        public void ResetValue()
        {
            currentValue = maxValue;
            SetValue(currentValue);
        }

        public void TakeDamage(BigNum damage)
        {
            //Debug.Log($"Damage: {damage}, CurrentHP: {currentHealth}, Value: {m_Slider.value}");
            SetValue(currentValue - damage);
            UpdateSlider();
        }

        public void SetValue(BigNum health)
        {
            if (m_Slider == null)
            {
                Debug.LogError("Did not found Slider Component!");
                return;
            }

            currentValue = Math2DHelper.Clamp(health, 0, maxValue);
            UpdateSlider();

            if (currentValue <= 0)
            {
                currentValue = 0;
                OnHealthDepleted.Invoke();
            }
        }

        public void SetColor(Color color)
        {
            m_FillImage.color = color;
        }

        public void UpdateSlider()
        {
            if (m_Slider == null || maxValue <= 0) 
                return;

            m_Slider.minValue = 0f;
            m_Slider.maxValue = 1f;

            m_Slider.value = BigNum.GetPercentage(currentValue, maxValue);
        }

        public void ResetState()
        {
            ResetValue();
        }

        // Private 메서드
        // Others

    } // Scope by class UIHealthBar
} // namespace Root
