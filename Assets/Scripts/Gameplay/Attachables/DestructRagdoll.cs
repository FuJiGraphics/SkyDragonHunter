using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {

    public class DestructRagdoll : Destructable
    {
        public override void OnDestruction(GameObject attacker)
        {
            var animController = GetComponent<TestAniController>();
            var instantiated = Instantiate(animController, transform.position, transform.rotation);
            var droppingBody = instantiated.AddComponent<DroppingBody>();
            droppingBody.StartDrop();
        }
    } // Scope by class DestructRagdoll

} // namespace Root