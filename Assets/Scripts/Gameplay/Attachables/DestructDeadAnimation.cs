using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {

    public class DestructDeadAnimation : Destructable
    {
        private static int DeadHash = Animator.StringToHash("Dead");
        public override void OnDestruction(GameObject attacker)
        {
            var animController = GetComponent<TestAniController>();
            var instantiated = Instantiate(animController, transform.position, transform.rotation);
            var animator = instantiated.GetComponent<Animator>();
            if(animator != null )
            {
                animator.enabled = true;
                animator.SetTrigger(DeadHash);
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
    } // Scope by class DestructDeadAnimation

} // namespace Root