using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter {

    public class PickUpInfoUI : MonoBehaviour
    {
        // 필드 (Fields)
        public TextMeshProUGUI crewNameText;
        public TextMeshProUGUI crewCountText;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public void SetData(string name, int count)
        {
            crewNameText.text = name;
            crewCountText.text = count.ToString();
        }
        // Private 메서드
        // Others

    } // Scope by class PickUpInfoUI

} // namespace Root