using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UIClickEffectOnUI : MonoBehaviour
    {
        [Header("UI 이펙트 프리팹 (ParticleSystem 포함)")]
        public GameObject effectPrefab;

        [Header("부모가 될 Canvas의 RectTransform")]
        public RectTransform uiRoot;

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ShowEffect(Input.GetTouch(0).position);
            }
        }

        private void ShowEffect(Vector2 screenPosition)
        {
            Vector2 anchoredPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiRoot, screenPosition, null, out anchoredPos))
            {
                GameObject effectGO = Instantiate(effectPrefab, uiRoot);

                // 위치 설정
                RectTransform effectRT = effectGO.GetComponent<RectTransform>();
                if (effectRT != null)
                {
                    effectRT.anchoredPosition = anchoredPos;
                    effectRT.localScale = Vector3.one;
                }

                // 🔥 최상단으로 올리기
                effectGO.transform.SetAsLastSibling();

                Destroy(effectGO, 1f);
            }
        }
    }
}
