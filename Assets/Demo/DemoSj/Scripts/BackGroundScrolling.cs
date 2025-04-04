using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public class BackGroundScrolling : MonoBehaviour
    {
        // 필드 (Fields)
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
        private void Start()
        {
            m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            m_Mat = m_SpriteRenderer.material;
            m_LastCameraPos = Camera.main.transform.position;
            m_LastCameraPos.z = transform.position.z;
            m_LastCameraHeight = Camera.main.orthographicSize;
            m_OriginScaleY = transform.localScale.y;
            Resize();
            Repos();
        }
    
        private void Update()
        {
            Resize();
            Repos();
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
            if (m_LastCameraPos == Camera.main.transform.position)
                return;

            m_LastCameraPos = Camera.main.transform.position;
            m_LastCameraPos.z = transform.position.z;
            transform.position = m_LastCameraPos;
        }

        private void Resize()
        {
            if (m_LastCameraHeight == Camera.main.orthographicSize)
                return;

            m_LastCameraHeight = Camera.main.orthographicSize;
            Vector2 spriteHalfSize = m_SpriteRenderer.size * 0.5f;
            float scaledHeight = Mathf.Abs(m_LastCameraHeight / spriteHalfSize.y);
            transform.localScale = new Vector2(m_OriginScaleY * scaledHeight, scaledHeight);
        }

        private void UpdateTextureOffset()
        {
            m_Offset += new Vector2(backGroundSpeed * Time.deltaTime, 0);
            m_Mat.mainTextureOffset = m_Offset;
        }

        // Private 메서드
        // Others

    } // Scope by class NewScript

} // namespace Root