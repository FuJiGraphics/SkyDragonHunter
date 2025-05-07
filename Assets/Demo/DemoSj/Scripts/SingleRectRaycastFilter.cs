using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    [RequireComponent(typeof(Image))]
    public class SingleRectRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
    {
        [SerializeField] private RectTransform rootCanvasRect; // 캔버스 기준 좌표
        [SerializeField] private Rect holeTarget;              // 구멍 영역 (로컬 좌표 기준)

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        /// <summary>
        /// 외부에서 전달받은 구멍 영역 Rect를 저장
        /// </summary>
        public void SetHoleRect(Rect newRect)
        {
            holeTarget = newRect; // 수정됨: 클릭 통과 영역 설정
            Debug.Log($"[RaycastFilter] SetHoleRect: {newRect.position}, size = {newRect.size}");
        }

        public void SetHole(int step) 
        {
            //혹시 특정 스텝에서만 클릭가능하게 할경우 그때 사용할 메서드
        }

        /// <summary>
        /// UI 클릭 시 이 위치를 통과시킬지 결정하는 메서드 (마스크 클릭 필터 핵심)
        /// </summary>
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (rootCanvasRect == null)
                return true;

            // 수정됨: width나 height가 0이면 구멍이 없다는 뜻이므로 전체 클릭 허용
            if (holeTarget.width <= 0 || holeTarget.height <= 0)
            {
                return true; // 수정됨: 마스크 클릭 전체 통과 허용
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvasRect, sp, eventCamera, out Vector2 localPoint);
            return !holeTarget.Contains(localPoint);
        }
        // Private 메서드
        // Others

    } // Scope by class SingleRectRaycastFilter

} // namespace Root