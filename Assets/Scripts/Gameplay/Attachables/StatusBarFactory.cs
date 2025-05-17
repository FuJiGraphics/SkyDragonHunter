using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class StatusBarFactory : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UIShieldAndHealth m_UIStatusBarPrefab;
        [SerializeField] private Color m_UIHealthColor = Color.green;
        [SerializeField] private Transform m_SpawnAnchor;
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private Vector2 m_Scale = Vector2.one;
        [SerializeField] private bool m_IsLeftToRight = true;
        [SerializeField] private CharacterStatus m_SyncStats;

        private UIShieldAndHealth m_StatusBarInstance = null;
        private Color m_StatusBarColor = Color.green;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            GenerateStatusBarInstance();
            SetAnchor();
            SyncCharacterStatus();
        }
    
        // Public 메서드
        public void OnChagnedMaxShield(BigNum maxShield)
        {
            m_StatusBarInstance.UIShieldBar.maxValue = maxShield;
            m_StatusBarInstance.UIShieldBar.ResetValue();
            m_StatusBarInstance.UIShieldBar.UpdateSlider();
            if (maxShield <= 0)
            {
                m_StatusBarInstance.UIShieldBar.gameObject.SetActive(false);
                m_StatusBarInstance.UIHealthBar.gameObject.SetActive(true);
            }
            else if (!m_StatusBarInstance.UIShieldBar.gameObject.activeSelf)
            {
                m_StatusBarInstance.UIShieldBar.gameObject.SetActive(true);
                m_StatusBarInstance.UIHealthBar.gameObject.SetActive(false);
            }
        }

        public void OnChagnedShield(BigNum shield)
        {
            if (!m_StatusBarInstance.UIShieldBar.gameObject.activeSelf)
                return;

            m_StatusBarInstance.UIShieldBar.currentValue = shield;
            m_StatusBarInstance.UIShieldBar.UpdateSlider();

            if (shield <= 0)
            {
                m_StatusBarInstance.UIShieldBar.gameObject.SetActive(false);
                m_StatusBarInstance.UIHealthBar.gameObject.SetActive(true);
            }
            else if (!m_StatusBarInstance.UIShieldBar.gameObject.activeSelf)
            {
                m_StatusBarInstance.UIShieldBar.gameObject.SetActive(true);
                m_StatusBarInstance.UIHealthBar.gameObject.SetActive(false);
            }
        }

        public void OnChagnedMaxHealth(BigNum maxHealth)
        {
            m_StatusBarInstance.UIHealthBar.maxValue = maxHealth;
            m_StatusBarInstance.UIHealthBar.ResetValue();
            m_StatusBarInstance.UIHealthBar.UpdateSlider();
        }

        public void OnChagnedHealth(BigNum health)
        {
            m_StatusBarInstance.UIHealthBar.currentValue = health;
            m_StatusBarInstance.UIHealthBar.UpdateSlider();
        }

        // Private 메서드
        private void GenerateStatusBarInstance()
        {
            if (m_StatusBarInstance == null)
            {
                if (m_UIStatusBarPrefab != null)
                {
                    m_StatusBarInstance = Instantiate(m_UIStatusBarPrefab);
                    var uibar = m_StatusBarInstance.GetComponent<UIShieldAndHealth>();
                    uibar.SetHealthColor(m_UIHealthColor);
                    uibar.SetDirection(m_IsLeftToRight);
                }
                else
                {
                    DrawableMgr.Dialog("Error", "[StatusBarFactory]: Status bar prefab을 찾을 수 없습니다.");
                }
            }
        }

        private void SetAnchor()
        {
            if (m_StatusBarInstance == null)
            {
                Debug.LogError("[StatusBarFactory]: m_StatusBarInstance의 Instance를 찾을 수 없습니다.");
                return;
            }

            if (m_SpawnAnchor != null)
            {
                m_StatusBarInstance.transform.SetParent(m_SpawnAnchor);
            }
            else
            {
                m_StatusBarInstance.transform.SetParent(transform);
            }

            m_StatusBarInstance.transform.localPosition = new Vector3(m_Offset.x, m_Offset.y, -7);
            m_StatusBarInstance.transform.localScale = m_Scale;
        }

        private void SyncCharacterStatus()
        {
            if (m_SyncStats == null)
            {
                if (TryGetComponent<CharacterStatus>(out var cStatsComp))
                {
                    m_SyncStats = cStatsComp;
                }
                else
                {
                    DrawableMgr.Dialog("Warn", "[StatusBarFactory]: CharacterStatus를 찾을 수 없습니다.");
                }
            }

            m_StatusBarInstance.UIHealthBar.maxValue = m_SyncStats.MaxHealth;
            m_StatusBarInstance.UIShieldBar.maxValue = m_SyncStats.MaxShield;
            m_StatusBarInstance.UIHealthBar.ResetValue();
            m_StatusBarInstance.UIShieldBar.ResetValue();

            m_SyncStats.AddChangedEvent(StatusChangedEventType.MaxShield, OnChagnedMaxShield);
            m_SyncStats.AddChangedEvent(StatusChangedEventType.Shield, OnChagnedShield);
            m_SyncStats.AddChangedEvent(StatusChangedEventType.MaxHealth, OnChagnedMaxHealth);
            m_SyncStats.AddChangedEvent(StatusChangedEventType.Health, OnChagnedHealth);
        }

        // Others

    } // Scope by class StatusBarGenerator
} // namespace SkyDragonHunter