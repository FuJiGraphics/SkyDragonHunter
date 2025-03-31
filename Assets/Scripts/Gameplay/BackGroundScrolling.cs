using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Game {

    public class BackGroundScrolling : MonoBehaviour
    {
        // 필드 (Fields)
        public float scrollSpeed = 1f;
        public float backGroundSpeed;

        private SpriteRenderer m_SpriteRenderer;
        private Material m_Mat;
        private Vector2 m_Offset;
        private float m_OriginScaleY;
        private Vector3 m_LastCameraPos;
        private float m_LastCameraHeight;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Reset()
        {
            m_LastCameraPos = Camera.main.transform.position;
            m_LastCameraPos.z = transform.position.z;
            m_LastCameraHeight = Camera.main.orthographicSize;
            Resize();
            Repos();
        }

        private void Start()
        {
             m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            m_Mat = m_SpriteRenderer.material;
            m_OriginScaleY = transform.localScale.y;
            m_LastCameraPos = Camera.main.transform.position;
            m_LastCameraPos.z = transform.position.z;
            m_LastCameraHeight = Camera.main.orthographicSize;
            Resize();
            Repos();
        }
    
        private void Update()
        {
            if (m_LastCameraHeight != Camera.main.orthographicSize)
            {
                m_LastCameraHeight = Camera.main.orthographicSize;
                Resize();
            }
            if (m_LastCameraPos != Camera.main.transform.position)
            {
                m_LastCameraPos = Camera.main.transform.position;
                m_LastCameraPos.z = transform.position.z;
                Repos();
            }
            UpdateTextureOffset();
        } 
    
        // Public 메서드
        public void SetBackGroundSpeed(int speed)
        {
            backGroundSpeed = speed;
        }

        public void SetBackGroundSprite(string spriteName)
        {
            m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        }

        private void Repos()
        {
            //transform.position = m_LastCameraPos;

            // 위치 조정 // 스프라이트 원본 크기
            Vector2 spriteLocalSize = m_SpriteRenderer.sprite.bounds.size;

            // 카메라 높이
            float cameraHeight = Camera.main.orthographicSize * 2f;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            // 스케일 계산
            float scaleX = cameraWidth / spriteLocalSize.x;
            float scaleY = cameraHeight / spriteLocalSize.y;
            transform.localScale = new Vector2(scaleX, scaleY);

            // 바닥 위치 조정: pivot이 center이므로, 스프라이트 전체 높이의 절반만큼 내려야 함
            float cameraBottomY = Camera.main.transform.position.y - (cameraHeight * 0.5f);
            float halfScaledHeight = (spriteLocalSize.y * scaleY) * 0.5f;

            transform.position = new Vector3(
                Camera.main.transform.position.x,
                cameraBottomY + halfScaledHeight,
                transform.position.z
            );
        }

        private void Resize()
        {
            //Vector2 spriteHalfSize = m_SpriteRenderer.size * 0.5f;
            //float scaledHeight = Mathf.Abs(m_LastCameraHeight / spriteHalfSize.y);
            //transform.localScale = new Vector2(m_OriginScaleY * scaledHeight, scaledHeight);

            Vector2 spriteLocalSize = m_SpriteRenderer.sprite.bounds.size;

            float cameraHeight = m_LastCameraHeight;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            float scaleX = cameraWidth / spriteLocalSize.x;
            float scaleY = cameraHeight / spriteLocalSize.y;

            transform.localScale = new Vector2(scaleX * 2, scaleY * 2);
        }

        private void UpdateTextureOffset()
        {
            m_Offset += new Vector2(backGroundSpeed * Time.deltaTime * scrollSpeed, 0);
            m_Mat.mainTextureOffset = m_Offset;
        }

        // Private 메서드
        // Others

    } // Scope by class NewScript

} // namespace Root