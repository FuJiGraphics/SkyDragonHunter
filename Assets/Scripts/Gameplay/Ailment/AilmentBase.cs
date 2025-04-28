using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class AilmentBase : MonoBehaviour
        , IAilmentEffect
    {
        // 필드 (Fields)
        [SerializeField] private int m_TableId;
        [SerializeField] private AilmentType m_Type;
        [SerializeField] private float m_Duration;
        [SerializeField] private float m_Tick;
        [SerializeField] private float m_ImmuneTime;
        [SerializeField] private int m_CurrentStackCount;
        [SerializeField] private int m_MaxStackCount;
        [SerializeField] private bool m_IsRefreshable;
        [SerializeField] private bool m_IsStackable;

        private IAilmentLifecycleHandler[] m_Handlers;

        // 속성 (Properties)
        public int ID => m_TableId;
        public AilmentType Type { get => m_Type; set => m_Type = value; }
        public float Duration { get => m_Duration; set => m_Duration = value; }
        public float Tick { get => m_Tick; set => m_Tick = value; }
        public float ImmuneTime { get => m_ImmuneTime; set => m_ImmuneTime = value; }
        public int CurrentStackCount { get => m_CurrentStackCount; set => m_CurrentStackCount = value; }
        public int MaxStackCount { get => m_MaxStackCount; set => m_MaxStackCount = value; }
        public bool IsRefreshable { get => m_IsRefreshable; set => m_IsRefreshable = value; }
        public bool IsStackable { get => m_IsStackable; set => m_IsStackable = value; }

        public GameObject Caster { get; set; } = null;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_Handlers = GetComponents<IAilmentLifecycleHandler>();

            var data = DataTableMgr.AilmentTable.Get(m_TableId);
            m_IsRefreshable = data.Refreshable;

            int ailmentTypeMaxCount = (int)AilmentType.Max;
            if (data.AilmentType < 0 || data.AilmentType >= ailmentTypeMaxCount)
            {
                Debug.LogError($"[AilmentBase]: 등록 실패. 잘못된 타입 인자 &{data.AilmentType}");
            }
            m_Type = (AilmentType)data.AilmentType;
            m_ImmuneTime = data.ImmuneTime;
        }

        // Public 메서드
        // Private 메서드
        // Others
        public void OnEnter(GameObject receiver)
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnEnter(receiver);
            }
        }

        public void OnStay()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnStay();
            }
        }

        public void OnExit()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnExit();
            }
        }

    } // Scope by class AilmentBurn
} // namespace Root