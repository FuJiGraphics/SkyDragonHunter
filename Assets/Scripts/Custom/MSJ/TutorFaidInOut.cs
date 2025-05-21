using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class TutorFaidInOut : MonoBehaviour
    {
        // 필드 (Fields)
        private TextMeshProUGUI textMeshProUGUI;
        private Coroutine fadeCoroutine;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();

            var outline = gameObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = gameObject.AddComponent<Outline>();
            }
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1.5f, -1.5f);
        }

        private void OnEnable()
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeInAndOutLoop());
        }

        private void OnDisable()
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        }


        // Public 메서드
        // Private 메서드
        // Others
        private IEnumerator FadeInAndOutLoop()
        {
            while (true)
            {
                yield return StartCoroutine(Fade(0f, 1f, 1f)); // Fade In
                yield return new WaitForSecondsRealtime(0.5f);
                yield return StartCoroutine(Fade(1f, 0f, 1f)); // Fade Out
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
        {
            float elapsed = 0f;
            Color baseColor = Color.white;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                baseColor.a = Mathf.Lerp(fromAlpha, toAlpha, t);
                textMeshProUGUI.color = baseColor;
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            baseColor.a = toAlpha;
            textMeshProUGUI.color = baseColor;
        }
    } // Scope by class TutorFaidInOut

} // namespace Root