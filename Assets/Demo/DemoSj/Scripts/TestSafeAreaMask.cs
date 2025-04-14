using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkyDragonHunter.test
{

    public class TestSafeAreaMask : MonoBehaviour
    {
        // 필드 (Fields)
        // 검은색 마스킹용 UI 패널들
        public RectTransform blackTop;
        public RectTransform blackBottom;
        public RectTransform blackLeft;
        public RectTransform blackRight;

        // SafeArea가 적용될 콘텐츠 영역 대상
        private RectTransform safeAreaTarget;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        void Awake()
        {
            // 이 스크립트가 붙은 오브젝트의 RectTransform을 SafeArea 대상으로 설정
            safeAreaTarget = GetComponent<RectTransform>();
            ApplySafeAreaMask();
        }

        // Public 메서드
        // Private 메서드
        void ApplySafeAreaMask()
        {
            Rect safeArea = Screen.safeArea;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // SafeArea 기준 anchor 계산
            float leftAnchorX = safeArea.x / screenWidth;
            float rightAnchorX = (safeArea.x + safeArea.width) / screenWidth;
            float bottomAnchorY = safeArea.y / screenHeight;
            float topAnchorY = (safeArea.y + safeArea.height) / screenHeight;

            float thickness = 200f; // 마스킹 두께 (UI 단위)

            // Left
            blackLeft.anchorMin = new Vector2(leftAnchorX, 0);
            blackLeft.anchorMax = new Vector2(leftAnchorX, 1);
            blackLeft.pivot = new Vector2(1, 0.5f);
            blackLeft.sizeDelta = new Vector2(thickness, 0);
            blackLeft.anchoredPosition = Vector2.zero;
            blackLeft.offsetMin = new Vector2(blackLeft.offsetMin.x, -2f);
            blackLeft.offsetMax = new Vector2(blackLeft.offsetMax.x, 2f);

            // Right
            blackRight.anchorMin = new Vector2(rightAnchorX, 0);
            blackRight.anchorMax = new Vector2(rightAnchorX, 1);
            blackRight.pivot = new Vector2(0, 0.5f);
            blackRight.sizeDelta = new Vector2(thickness, 0);
            blackRight.anchoredPosition = Vector2.zero;
            blackRight.offsetMin = new Vector2(blackRight.offsetMin.x, -2f);
            blackRight.offsetMax = new Vector2(blackRight.offsetMax.x, 2f);

            // Bottom
            blackBottom.anchorMin = new Vector2(0, bottomAnchorY);
            blackBottom.anchorMax = new Vector2(1, bottomAnchorY);
            blackBottom.pivot = new Vector2(0.5f, 1);
            blackBottom.sizeDelta = new Vector2(0, thickness);
            blackBottom.anchoredPosition = Vector2.zero;
            blackBottom.offsetMin = new Vector2(-2f, blackBottom.offsetMin.y);
            blackBottom.offsetMax = new Vector2(2f, blackBottom.offsetMax.y);

            // Top
            blackTop.anchorMin = new Vector2(0, topAnchorY);
            blackTop.anchorMax = new Vector2(1, topAnchorY);
            blackTop.pivot = new Vector2(0.5f, 0);
            blackTop.sizeDelta = new Vector2(0, thickness);
            blackTop.anchoredPosition = Vector2.zero;
            blackTop.offsetMin = new Vector2( -2f, blackTop.offsetMin.y);
            blackTop.offsetMax = new Vector2( 2f, blackTop.offsetMax.y);

            // 보이도록 보장
            blackTop.gameObject.SetActive(true);
            blackBottom.gameObject.SetActive(true);
            blackLeft.gameObject.SetActive(true);
            blackRight.gameObject.SetActive(true);
        }
        // Others

    } // Scope by class TestSafeAreaFitter

} // namespace Root