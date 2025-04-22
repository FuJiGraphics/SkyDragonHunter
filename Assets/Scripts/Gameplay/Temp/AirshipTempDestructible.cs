using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class AirshipTempDestructible : MonoBehaviour
        , IDestructible
    {
        public TestWaveController testWaveController;

        public void OnDestruction(GameObject attacker)
        {
            var waveController = GameMgr.FindObject("WaveController");
            if (waveController != null)
            {
                var wavecomp = waveController.GetComponent<TestWaveController>();
                if (wavecomp != null)
                {
                    wavecomp.OnTestWaveFailedActive();
                }
                else
                {
                    testWaveController.gameObject.SetActive(true);
                }

            }
            else
            {
                testWaveController.gameObject.SetActive(true);
            }
            var stat = GetComponent<CharacterStatus>();
            stat.ResetAll();
        }

        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class AirshipTempDestructible

} // namespace Root