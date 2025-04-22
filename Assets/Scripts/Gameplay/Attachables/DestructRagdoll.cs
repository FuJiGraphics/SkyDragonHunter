using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {

    public class DestructRagdoll : Destructable
    {
        public override void OnDestruction(GameObject attacker)
        {
            var prefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
            var animController = GetComponent<TestAniController>();
            var id = animController.ID;
            var instantiated = Instantiate(prefabLoader.GetRagdollAnimController(id), transform.position, transform.rotation);
            instantiated.transform.localScale = transform.localScale;
            var droppingBody = instantiated.AddComponent<DroppingBody>();
            droppingBody.StartDrop();
        }
    } // Scope by class DestructRagdoll

} // namespace Root