using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{
    public class DroppingBody : MonoBehaviour
    {        
        private static int dropHash = Animator.StringToHash("Drop");
        private static int deadHash = Animator.StringToHash("Dead");
        private static float dropSpeed = 30f;
        public float timer = 4f;
        public bool isDropping = false;

        private void Update()
        {
            if (isDropping)
            {
                timer -= Time.deltaTime;
                var dir = Vector3.down;
                transform.position += dir * dropSpeed * Time.deltaTime;
                if(timer < 0f)
                    Destroy(gameObject);
            }
        }

        public void StartDrop()
        {
            isDropping = true;
            var animator = GetComponent<Animator>();            
            if (animator != null)
            {
                // TODO: Maybe consider replacing "Bee" with their IDs?
                if(gameObject.name.Contains("Bee"))
                {
                    animator.SetTrigger(deadHash);
                }
                else
                {
                    animator.SetTrigger(dropHash);
                }
                animator.enabled = true;
                
            }
            else
            {
                Debug.LogError($"Could not find animator component");
            }
        }
        public void OnDeadAnimationEnd()
        {
            Destroy(gameObject);
        }
    } // Scope by class DroppingBody

} // namespace Root