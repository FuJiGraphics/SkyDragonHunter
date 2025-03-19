using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIHealthBar : MonoBehaviour
    {
        // 필드 (Fields)
        public float maxHealth;
        public float currentHealth;
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

        private void Update()
        {
            UpdateSlider();
        }

        private void Reset()
        {
            ResetHP();
        }

        // Public 메서드
        public void ResetHP()
        {
            UpdateSlider();
            currentHealth = maxHealth;
            SetHP(currentHealth);
        }

        public void TakeDamage(float damage)
        {
            SetHP(currentHealth - damage);
        }

        public void SetHP(float health)
        {
            if (m_Slider == null)
            {
                Debug.LogError("Did not found Slider Component!");
            }
            currentHealth = Mathf.Clamp(health, 0f, maxHealth);
            m_Slider.value = currentHealth;
            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                OnHealthDepleted.Invoke();
            }
        }

        private void UpdateSlider()
        {
            if (m_Slider.maxValue != maxHealth)
            {
                m_Slider.maxValue = maxHealth;
            }
            if (m_Slider.value != currentHealth)
            {
                m_Slider.value = currentHealth;
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UIHealthBar
} // namespace Root
