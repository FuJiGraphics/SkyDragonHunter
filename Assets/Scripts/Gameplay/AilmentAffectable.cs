using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.UI;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class AilmentAffectable : MonoBehaviour
        , IAilmentAffectable
    {
        // 필드 (Fields)
        private Coroutine m_BurnCoroutine;
        private WaitForSeconds m_BurningStride;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private DamageReceiver m_DamageReceiver;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_DamageReceiver = GetComponent<DamageReceiver>();
            m_BurnCoroutine = null;
            m_BurningStride = new WaitForSeconds(1f);
        }

        private IEnumerator CoBurning(BurnStatusAliment status)
        {
            float endTime = Time.time + status.duration;
            while (Time.time < endTime)
            {
                m_DamageReceiver.TakeDamage(status.attacker, status.damagePerSeconds);
                DrawableMgr.Text(transform.position, status.damagePerSeconds.ToString(), Color.green);
                yield return m_BurningStride;
            }
            m_BurnCoroutine = null;
        }

        // Others
        public void OnBurn(BurnStatusAliment status)
        {
            if (m_BurnCoroutine != null)
                StopCoroutine(m_BurnCoroutine);
            m_BurnCoroutine = StartCoroutine(CoBurning(status));
        }


    } // Scope by class AilmentAffectable
} // namespace SkyDragonHunter