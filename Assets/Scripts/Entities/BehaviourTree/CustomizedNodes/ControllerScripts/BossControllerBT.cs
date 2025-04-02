using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities 
{
    public enum BossAttackType
    {
        Melee,
        Ranged,
    }

    public class BossControllerBT : BaseControllerBT<BossControllerBT>
    {
        [SerializeField] private BossAttackType attackType;    

        


       
        public override void ResetTarget()
        {
            
        }

        protected override void InitBehaviourTree()
        {
            
        }
    } // Scope by class BossControllerBT

} // namespace Root