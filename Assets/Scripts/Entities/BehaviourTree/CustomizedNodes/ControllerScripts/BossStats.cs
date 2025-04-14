using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class BossStats : MonoBehaviour
    {
        // Fields
        public float attackRange;
        public float aggroRange;
        public float speed;
        public float chaseSpeed;
        public float attackInterval;

        // External Dependencies Field
        public CharacterStatus status;
        public CharacterInventory inventory;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            inventory = GetComponent<CharacterInventory>();
        }

        public void ForceInit()
        {
            status = GetComponent<CharacterStatus>();
            inventory = GetComponent<CharacterInventory>();
        }

        public void SetDataFromTable(int id)
        {

        }
    } // Scope by class BossStats

} // namespace Root