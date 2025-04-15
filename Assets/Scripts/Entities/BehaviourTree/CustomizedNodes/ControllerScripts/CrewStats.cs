using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    [RequireComponent(typeof(CharacterStatus))]
    public class CrewStats : MonoBehaviour
    {
        // Fields
        

        public float attackRange;
        public float aggroRange;
        public float speed;
        public float chaseSpeed;
        public float attackInterval;
        public float skillCoolTime;
        public float skillInitialDelay;

        // External Dependencies Field
        public CharacterStatus status;
        public CharacterInventory inventory;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            inventory = GetComponent<CharacterInventory>();
        }

        // TODO: TEMP INITIALIZE
        private void Start()
        {
            TempInitialization();
        }

        private void TempInitialization()
        {
            if(attackRange == 0)
            {
                attackRange = 5;
            }
            if(aggroRange == 0)
            {
                aggroRange = 15;
            }
            if(speed == 0)
            {
                speed = 10;
            }
            if(chaseSpeed == 0)
            {
                chaseSpeed = 30;
            }
            if(attackInterval == 0)
            {
                attackInterval = 1f;
            }
            if(skillCoolTime == 0)
            {
                skillCoolTime = 1f;
            }
            if(skillInitialDelay == 0)
            {
                skillInitialDelay = 1f;
            }
        }

        // Public Methods

        public void SetDataFromTable(int id)
        {

        }


    } // Scope by class CrewStatus

} // namespace Root