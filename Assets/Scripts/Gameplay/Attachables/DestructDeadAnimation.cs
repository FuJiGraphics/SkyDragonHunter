using SkyDragonHunter.Managers;
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
            var prefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
            var animController = GetComponent<TestAniController>();
            var id = animController.ID;
            var instantiated = Instantiate(prefabLoader.GetRagdollAnimController(id), transform.position, transform.rotation);
            instantiated.transform.localScale = transform.localScale;
            var animator = instantiated.GetComponent<Animator>();
            if( animator != null )
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