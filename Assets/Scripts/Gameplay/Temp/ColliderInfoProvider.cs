using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public class ColliderInfoProvider : MonoBehaviour
    {
        // Properties
        public float ColliderHalfWidth {  get; private set; }

        // Unity Methods
        private void Awake()
        {
            SetColliderHalfWidth();
        }

        private void SetColliderHalfWidth()
        {
            var collider = GetComponent<Collider2D>();
            var colliderType = collider.GetType();

            if (colliderType == typeof(CircleCollider2D))
            {
                var circleCollider = (CircleCollider2D)collider;
                ColliderHalfWidth = circleCollider.radius;
                Debug.Log($"Type of [{gameObject.name}] : '{colliderType}', collider halfWidth = {ColliderHalfWidth}");
            }
            else if (colliderType == typeof(BoxCollider2D))
            {
                var boxCollider = (BoxCollider2D)collider;
                ColliderHalfWidth = (boxCollider.bounds.size.x + boxCollider.bounds.size.y) * 0.25f;
                Debug.Log($"Type of [{gameObject.name}] : '{colliderType}', collider halfWidth = {ColliderHalfWidth}");
            }
            else
            {
                ColliderHalfWidth = collider.bounds.size.x * 0.5f;
                Debug.LogWarning($"Type of [{gameObject.name}] : '{colliderType}', collider halfWidth = {ColliderHalfWidth}");
            }
        }

    } // Scope by class ColliderInfoProvider

} // namespace Root