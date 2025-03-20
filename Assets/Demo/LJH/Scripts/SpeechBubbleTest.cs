using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public class SpeechBubbleTest : MonoBehaviour
    {
        // 필드 (Fields)
        private string[] testStrings =
        {
            "Short",
            "Middle Sized",
            "Some random sentence for the test",
            "Longest sentence for the sake of tesing how it will be displayed on the field",
            "Four\nDifferent\nLines\nFor test"
        };

        private Vector3 m_cachedPos;
        private int m_stringIndex;       // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_cachedPos = transform.position;
            m_stringIndex = 0;
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ResetPos();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ResetPos(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CallSpeechBubble();
            }
        }
    
        // Public 메서드
        // Private 메서드
        private void ResetPos(bool rand = false)
        {
            if (rand)
            {
                var newPos = m_cachedPos;
                newPos.x += Random.Range(-3, 3);
                newPos.z += Random.Range(-3, 3);
                newPos.y = Random.Range(1, 3);
                transform.position = newPos;
            }
            else
            {
                transform.position = m_cachedPos;
            }            
        }

        private void CallSpeechBubble(int index = int.MinValue)
        {
            if (!(index == int.MinValue || index >= 0 || index < testStrings.Length))
                return;
            if(index == int.MinValue)
                index = Random.Range(0, testStrings.Length);

            
        }

        // Others
    
    } // Scope by class SpeechBubbleTest

} // namespace Root