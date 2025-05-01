using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    /// <summary>
    /// UI에서 어두운 마스크의 구멍 위치와 크기를 지정하는 스크립트
    /// </summary>
    public class SingleHoleMaskController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Material maskMat; // 쉐이더가 적용된 마테리얼
        [SerializeField] private RectTransform rootCanvasRect; // 캔버스 기준

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        /// <summary>
        /// 특정 UI 요소(RectTransform)의 화면 내 위치를 구멍으로 설정
        /// </summary>
        public void SetHole(RectTransform target)
        {
            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);

            Vector2 min = WorldToCanvasUV(corners[0]);
            Vector2 max = WorldToCanvasUV(corners[2]);

            maskMat.SetVector("_Rect", new Vector4(min.x, min.y, max.x, max.y));

            // Raycast 필터에도 같은 영역 전달
            Rect canvasRect = rootCanvasRect.rect;

            Vector2 localMin = rootCanvasRect.InverseTransformPoint(corners[0]);
            Vector2 localMax = rootCanvasRect.InverseTransformPoint(corners[2]);

            var raycastFilter = GetComponent<SingleRectRaycastFilter>();
            if (raycastFilter != null)
            {
                Rect hole = new Rect(localMin, localMax - localMin);
                raycastFilter.SetHoleRect(hole);
                Debug.Log($"SetHoleRect: {hole.position}, size = {hole.size}");
            }
        }

        // Private 메서드
        // RectTransform 좌표를 UV(0~1) 좌표로 변환
        private Vector2 WorldToCanvasUV(Vector3 worldPos)
        {
            Vector2 localPos = rootCanvasRect.InverseTransformPoint(worldPos);
            Rect rect = rootCanvasRect.rect;

            // (0,0)~(1,1)로 정규화된 UV 좌표 반환
            return new Vector2(
                (localPos.x - rect.x) / rect.width,
                (localPos.y - rect.y) / rect.height
            );
        }

        // Others

    } // Scope by class SingleHoleMaskController

} // namespace Root