using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    [System.Serializable]
    public struct SkillButtonSlot
    {
        // 필드 (Fields)
        public Button button;
        public Image image;
        public Slider slider;
        public GameObject autoEffect;

        private SkillExecutor m_Target;

        // 속성 (Properties)
        public SkillExecutor Target => m_Target;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public void Set(SkillExecutor target)
        {
            m_Target = target;
            button.onClick.RemoveListener(target.Execute);
            button.onClick.AddListener(target.Execute);
            autoEffect.SetActive(m_Target.IsAutoExecute);
            image.sprite = target.SkillData.ActiveSkillIcon;
            image.enabled = true;
            slider.value = target.CooldownProgress;
        }

        public void Clear()
        {
            if (m_Target == null)
                return;

            slider.value = 0;
            image.enabled = false;
            image.sprite = null;
            autoEffect.SetActive(false);
            button.onClick.RemoveListener(m_Target.Execute);
            m_Target = null;
        }

        public void Update()
        {
            if (m_Target == null)
            {
                autoEffect.SetActive(false);
                return;
            }

            button.interactable = !m_Target.IsAutoExecute;
            autoEffect.SetActive(m_Target.IsAutoExecute);
            slider.value = 1f - m_Target.CooldownProgress;
        }
    }

    public class UISkillButtons : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private SkillButtonSlot[] m_ButtonSlots;
        [SerializeField] private bool m_IsAllAutoExecute = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        public void Update()
        {
            foreach(var slot in m_ButtonSlots)
            {
                if (slot.Target != null)
                {
                    slot.Target.IsAutoExecute = m_IsAllAutoExecute;
                }
                slot.Update();
            }
        }

        // Public 메서드
        public void Equip(int index, SkillExecutor target)
        {
            if (!Validate(index))
            {
                throw new ArgumentOutOfRangeException("인덱스 범위 초과");
            }

            if (target == null)
                return;

            target.IsAutoExecute = m_IsAllAutoExecute;

            Unequip(index);
            if (FindSlotIndex(target, out var prevSlotIndex))
            {
                Unequip(prevSlotIndex);
            }
            m_ButtonSlots[index].Set(target);
        }

        public void Unequip(int index)
        {
            if (!Validate(index))
            {
                throw new ArgumentOutOfRangeException("인덱스 범위 초과");
            }

            m_ButtonSlots[index].Clear();
        }

        public bool FindSlotIndex(SkillExecutor target, out int dst)
        {
            bool result = false;
            dst = -1;

            if (target == null)
                return result;

            for (int i = 0; i < m_ButtonSlots.Length; ++i)
            {
                if (m_ButtonSlots[i].Target == target)
                {
                    result = true;
                    dst = i;
                    break;
                }
            }
            return result;
        }

        public void OnAutoButton()
        {
            m_IsAllAutoExecute = !m_IsAllAutoExecute;
            GameMgr.FindObject("AutoSkillRunEffect").SetActive(m_IsAllAutoExecute);
        }

        public bool Validate(int index)
            => m_ButtonSlots != null && index >= 0 && index < m_ButtonSlots.Length;

        // Private 메서드
        // Others

    } // Scope by class UISkillButtons
} // namespace SkyDragonHunter