using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UISpeechBubble : MonoBehaviour
    {
        // 필드 (Fields)
        private string m_String;
        private int lifeTime;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            
        }
    
        private void Update()
        {
            
        }
    
        // Public 메서드
        public void SetString(string input, int lifeTime = 3)
        {
            m_String = input;
            this.lifeTime = lifeTime;
            Destroy(gameObject, lifeTime);
        }

        // Private 메서드
        // Others
    
    } // Scope by class UISpeechBubble

} // namespace Root