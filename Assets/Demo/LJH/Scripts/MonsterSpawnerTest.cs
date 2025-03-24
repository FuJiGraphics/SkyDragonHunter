using SkyDragonHunter.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class MonsterSpawnerTest : MonoBehaviour
    {
        public EnemyControllerBT monsterPrefab;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(monsterPrefab, transform.position, transform.rotation);
            }
        }

    } // Scope by class MonsterSpawnerTest

} // namespace Root