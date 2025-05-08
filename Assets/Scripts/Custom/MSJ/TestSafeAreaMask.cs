using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkyDragonHunter.test
{

    public class TestSafeAreaMask : MonoBehaviour
    {
        // �ʵ� (Fields)
        // ������ ����ŷ�� UI �гε�
        public RectTransform blackTop;
        public RectTransform blackBottom;
        public RectTransform blackLeft;
        public RectTransform blackRight;

        // SafeArea�� ����� ������ ���� ���
        private RectTransform safeAreaTarget;
        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        void Awake()
        {
            // �� ��ũ��Ʈ�� ���� ������Ʈ�� RectTransform�� SafeArea ������� ����
            safeAreaTarget = GetComponent<RectTransform>();
            ApplySafeAreaMask();
        }

        // Public �޼���
        // Private �޼���
        void ApplySafeAreaMask()
        {
            Rect safeArea = Screen.safeArea;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // SafeArea ���� anchor ���
            float leftAnchorX = safeArea.x / screenWidth;
            float rightAnchorX = (safeArea.x + safeArea.width) / screenWidth;
            float bottomAnchorY = safeArea.y / screenHeight;
            float topAnchorY = (safeArea.y + safeArea.height) / screenHeight;

            float thickness = 200f; // ����ŷ �β� (UI ����)

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

            // ���̵��� ����
            blackTop.gameObject.SetActive(true);
            blackBottom.gameObject.SetActive(true);
            blackLeft.gameObject.SetActive(true);
            blackRight.gameObject.SetActive(true);
        }
        // Others

    } // Scope by class TestSafeAreaFitter

} // namespace Root