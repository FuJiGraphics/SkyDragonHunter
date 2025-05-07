using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{

    /// <summary>
    /// UI에서 어두운 마스크의 구멍 위치와 크기를 지정하는 스크립트
    /// </summary>
    public class SingleHoleMaskController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Material maskMat;              // 마스크 쉐이더가 적용된 머티리얼
        [SerializeField] private RectTransform rootCanvasRect;  // 기준이 되는 캔버스 RectTransform

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        /// <summary>
        /// 특정 UI 요소의 RectTransform을 기준으로 마스크에 구멍을 뚫는다.
        /// </summary>
        /// <param name="target">구멍을 뚫을 대상 UI의 RectTransform</param>
        public void SetHole(RectTransform target)
        {
            // 월드 좌표로 코너 포인트 계산
            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);

            // UV 좌표로 변환
            Vector2 min = WorldToCanvasUV(corners[0]);
            Vector2 max = WorldToCanvasUV(corners[2]);
            maskMat.SetVector("_Rect", new Vector4(min.x, min.y, max.x, max.y));

            // 로컬 좌표로 변환해서 RaycastFilter에도 전달
            Vector2 localMin = rootCanvasRect.InverseTransformPoint(corners[0]);
            Vector2 localMax = rootCanvasRect.InverseTransformPoint(corners[2]);

            var raycastFilter = GetComponent<SingleRectRaycastFilter>();
            if (raycastFilter != null)
            {
                Rect hole = new Rect(localMin, localMax - localMin);
                raycastFilter.SetHoleRect(hole); // 클릭 허용 영역 지정
            }
        }

        /// <summary>
        /// 구멍 비활성화: 마스크 전역 클릭 허용
        /// </summary>
        public void DisableHole()
        {
            // 화면 밖으로 구멍 이동 처리 (마스크 색상 유지)
            maskMat.SetVector("_Rect", new Vector4(1, 1, 1, 0));

            var raycastFilter = GetComponent<SingleRectRaycastFilter>();
            if (raycastFilter != null)
            {
                // 수정됨: 구멍 영역 제거 + 클릭 전면 허용을 위해 width와 height를 0으로 설정
                raycastFilter.SetHoleRect(new Rect(0, 0, 0, 0)); // 수정됨
            }
        }

        /// <summary>
        /// RectTransform 좌표를 UV(0~1) 좌표로 변환
        /// </summary>
        private Vector2 WorldToCanvasUV(Vector3 worldPos)
        {
            Vector2 localPos = rootCanvasRect.InverseTransformPoint(worldPos);
            Rect rect = rootCanvasRect.rect;

            return new Vector2(
                (localPos.x - rect.x) / rect.width,
                (localPos.y - rect.y) / rect.height
            );
        }

        // Others

    } // Scope by class SingleHoleMaskController

} // namespace Root