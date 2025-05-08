using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class MonsterDestructable : Destructable
    {
        // 필드 (Fields)
        public GameObject[] dropItems;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public override void OnDestruction(GameObject attacker)
        {
            // TODO: LJH
            if (dropItems != null && dropItems.Length != 0)
            {
                if (dropItems[Random.Range(0, dropItems.Length)] != null)
                {
                    var item = Instantiate(dropItems[Random.Range(0, dropItems.Length)]);
                    item.transform.position = gameObject.transform.position;
                }
            }
            // ~TODO
            Destroy(gameObject);
        }

        // Private 메서드
        // Others

    } // Scope by class MonsterDestructible

} // namespace Root