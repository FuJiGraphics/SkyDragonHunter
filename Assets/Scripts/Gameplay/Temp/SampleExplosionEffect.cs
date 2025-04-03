using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test
{
    public class SampleExplosionEffect : MonoBehaviour
    {
        public float radius = 1f;

        private SpriteRenderer m_SpriteRenderer;
        private CircleCollider2D m_Collider;

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Collider = GetComponent<CircleCollider2D>();

            m_Collider.radius = radius;

            var sprite = m_SpriteRenderer.sprite;

            if (sprite == null)
                return;

            float spriteDiameter = sprite.bounds.size.x;
            float targetDiameter = radius * 2f;

            float scale = targetDiameter / spriteDiameter;
            transform.localScale = new Vector3(scale, scale, 1f);
        }

    }  // Scope by class SampleExplosionEffect
}  // namespace SkyDragonHunter 
