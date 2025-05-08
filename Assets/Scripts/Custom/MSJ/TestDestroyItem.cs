using SkyDragonHunter.Tables;
using UnityEngine;

namespace SkyDragonHunter {

    public class TestDestroyItem : MonoBehaviour
    {
        // 필드 (Fields)
        public ItemType itemType;
        private float effectSpeed = 1f;
        private float fadeTimer = 0f;
        private SpriteRenderer spriteRenderer;
        private Color targetColor;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            spriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            targetColor = new Color(1f, 1f, 1f, 0f);
        }

        private void Update()
        {
            transform.position += Vector3.up * effectSpeed * Time.deltaTime;

            fadeTimer += Time.deltaTime * effectSpeed;
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, fadeTimer);

            if (spriteRenderer.color.a <= 0.01f) // 부동소수점 비교는 정확히 0f 대신 작은 값 사용
            {
                // ItemMgr.Add(itemType);
                Destroy(gameObject);
            }
        }
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class TestDestroyItem

} // namespace Root