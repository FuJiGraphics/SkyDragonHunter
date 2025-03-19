using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public class BackGroundScrolling : MonoBehaviour
    {
        // 필드 (Fields)
        public float backGroundSpeed;
        private bool isStopped;
        private SpriteRenderer spriteRenderer;
        private Material mat;
        private Vector2 offset;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
        }
    
        private void Update()
        {
            offset += new Vector2(backGroundSpeed * Time.deltaTime, 0);
            mat.mainTextureOffset = offset;
        }
    
        // Public 메서드
        public void SetBackGroundSpeed(int speed)
        {
            backGroundSpeed = speed;
        }

        public void SetBackGroundSprite(string spriteName)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        }
        // Private 메서드
        // Others
    
    } // Scope by class NewScript

} // namespace Root