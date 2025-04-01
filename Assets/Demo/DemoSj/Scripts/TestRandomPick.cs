using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class TestRandomPick : MonoBehaviour
    {
        // 필드 (Fields)
        private int[] ints;
        private int count = 17;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            ints = new int[count];
            for (int i = 0; i < count - 1; i++)
            {
                ints[i] = i + 1;
            }
        }

        // Public 메서드
        public void RandomPick()
        {
            Debug.Log(ints[Random.Range(0, ints.Length - 1)]);
        }

        public void RandomTenPick()
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.Log(ints[Random.Range(0, ints.Length - 1)]);
            }
        }
        // Private 메서드
        // Others

    } // Scope by class TestRandomPick

} // namespace Root