using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter 
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class CardGridResizer : MonoBehaviour
    {
        // 필드 (Fields)
        // 콘텐츠 영역의 RectTransform (ScrollRect의 Content)
        [SerializeField] private RectTransform contentRect;
        // Scroll View의 ScrollRect (스크롤 위치 초기화용)
        [SerializeField] private ScrollRect scrollRect;
        // 고정 열 개수
        [SerializeField] private int columnCount = 4;

        // GridLayoutGroup에 설정된 padding 값들
        [SerializeField] private float spacingX = 50f;        // 가로 간격
        [SerializeField] private float spacingY = 100f;       // 세로 간격
        [SerializeField] private float paddingLeft = 10f;
        [SerializeField] private float paddingRight = 10f;
        [SerializeField] private float paddingTop = 10f;
        [SerializeField] private float paddingBottom = 0f;

        [SerializeField] private float cellAspectRatio = 1.4285f; // [차이점 시작] 세로가 긴 셀을 위한 비율 (height / width)

        // 내부 참조 변수들
        private GridLayoutGroup grid;         // GridLayoutGroup 컴포넌트
        private int lastChildCount = -1;      // 마지막으로 확인한 자식 수
        private float lastWidth = -1f;        // 마지막으로 확인한 content width
        private bool dirty = false;           // 변경 여부 플래그
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            grid = contentRect.GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            // 강제로 레이아웃 갱신하도록 표시
            dirty = true;

            // 다음 프레임 이후 레이아웃 적용
            StartCoroutine(ForceUpdateNextFrame());
        }

        // GameObject가 활성화될 때 호출됨 (SetActive(true))
        private void OnEnable()
        {
            // 강제로 레이아웃 갱신하도록 표시
            dirty = true;

            // 다음 프레임 이후 레이아웃 적용
            StartCoroutine(ForceUpdateNextFrame());
        }

        // 매 프레임마다 자식 수나 콘텐츠 크기 변화 감지
        private void Update()
        {
            // 자식 수 변화 감지
            if (contentRect.childCount != lastChildCount)
            {
                lastChildCount = contentRect.childCount;
                dirty = true;
            }

            // 콘텐츠 너비 변화 감지 (UI 리사이즈 포함)
            float currentWidth = contentRect.rect.width;
            if (!Mathf.Approximately(currentWidth, lastWidth))
            {
                lastWidth = currentWidth;
                dirty = true;
            }

            // 변화가 감지된 경우 반영
            if (dirty)
            {
                dirty = false;
                ApplyLayout();
            }
        }

        // Public 메서드
        // Private 메서드
        private void OnRectTransformDimensionsChange()
        {
            dirty = true;
        }

        private float GetCanvasScale()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
            {
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                if (scaler != null)
                {
                    return canvas.scaleFactor;
                }
            }

            // fallback
            return contentRect.lossyScale.x;
        }

        // UI가 활성화되었을 때 다음 프레임에 강제로 레이아웃 갱신 (CanvasScaler 대응)
        System.Collections.IEnumerator ForceUpdateNextFrame()
        {
            yield return new WaitUntil(() => contentRect.rect.width > 0f);
            ApplyLayout();     // 강제 적용
        }

        // 셀 크기 및 콘텐츠 높이 재계산 적용
        private void ApplyLayout()
        {
            if (contentRect.rect.width <= 0f)
            {
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            
            float totalSpacingX = spacingX * (columnCount - 1);
            float totalPaddingX = paddingLeft + paddingRight;
            float availableWidth = contentRect.rect.width - totalSpacingX - totalPaddingX;

            float cellWidth = availableWidth / columnCount;

            // 셀 높이를 비율 기반으로 계산
            float cellHeight = cellWidth * cellAspectRatio;
            grid.cellSize = new Vector2(cellWidth, cellHeight); // [차이점 끝]

            int rowCount = Mathf.CeilToInt((float)contentRect.childCount / columnCount);

            // spacingY와 paddingY 반영
            float totalSpacingY = spacingY * (rowCount - 1);
            float totalPaddingY = paddingTop + paddingBottom;
            float newHeight = rowCount * cellHeight + totalSpacingY + totalPaddingY;

            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            // 스크롤 위치 최상단으로 초기화
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }

    } // Scope by class SquareGridResizer

} // namespace Root