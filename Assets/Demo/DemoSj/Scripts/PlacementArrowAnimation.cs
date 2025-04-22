using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class PlacementArrowAnimation : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private float speed;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private Vector3 startPos;
        private RectTransform rectTransform;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }
    
        private void Update()
        {
            float t = Mathf.PingPong(Time.time * speed, 1f); // 0~1 사이에서 반복
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class PlacementArrowAnimation

} // namespace Root