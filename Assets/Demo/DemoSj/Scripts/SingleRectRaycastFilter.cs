using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    [RequireComponent(typeof(Image))]
    public class SingleRectRaycastFilter : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private RectTransform rootCanvasRect;
        [SerializeField] private Rect holeTarget;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        // 외부에서 뚫을 영역 설정
        public void SetHoleRect(Rect newRect)
        {
            holeTarget = newRect;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Debug.Log($"Raycast 위치: {sp}");

            if (rootCanvasRect == null)
                return true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvasRect, sp, eventCamera, out Vector2 localPoint);

            Debug.Log($"LocalPoint = {localPoint}, Hole = {holeTarget}");

            if (holeTarget.Contains(localPoint))
                return false; // 뚫린 영역 → 클릭 허용

            return true; // 나머지 → 클릭 차단
        }
        // Private 메서드
        // Others

    } // Scope by class SingleRectRaycastFilter

} // namespace Root