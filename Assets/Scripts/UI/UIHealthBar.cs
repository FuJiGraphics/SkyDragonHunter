using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIHealthBar : MonoBehaviour
    {
        // 필드 (Fields)
        public BigInteger maxHealth;
        public BigInteger currentHealth;
        private Slider m_Slider;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        [Tooltip("CurrentHealth가 0f가 되었을 때 호출되는 이벤트")]
        [SerializeField] private UnityEvent OnHealthDepleted;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Slider = GetComponentInChildren<Slider>();
            ResetHP();
        }

        private void Reset()
        {
            ResetHP();
        }

        // Public 메서드
        public void ResetHP()
        {
            currentHealth = maxHealth;
            SetHP(currentHealth);
        }

        public void TakeDamage(AlphaUnit damage)
        {
            //Debug.Log($"Damage: {damage}, CurrentHP: {currentHealth}, Value: {m_Slider.value}");
            SetHP(currentHealth - damage.Value);
            UpdateSlider();
        }

        public void SetHP(AlphaUnit health)
        {
            if (m_Slider == null)
            {
                Debug.LogError("Did not found Slider Component!");
                return;
            }

            currentHealth = Math2DHelper.Max(0, Math2DHelper.Min(health.Value, maxHealth));
            UpdateSlider();

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnHealthDepleted.Invoke();
            }
        }

        private void UpdateSlider()
        {
            if (m_Slider == null || maxHealth <= 0) 
                return;

            m_Slider.minValue = 0f;
            m_Slider.maxValue = 1f;

            m_Slider.value = (float)((double)currentHealth / (double)maxHealth);
        }

        // Private 메서드
        // Others

    } // Scope by class UIHealthBar
} // namespace Root
