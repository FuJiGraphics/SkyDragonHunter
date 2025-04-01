using NPOI.POIFS.FileSystem;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "AttackRangeSC.asset", menuName = "Weapon/AttackRangeSC")]
    public class RangedAttackSC : AttackDefinition
        , IWeaponable
    {
        // 필드 (Fields)
        public string itemName;
        public Sprite icon; 

        public GameObject projectilePrefab;
        public float projectileSpeed = 5f;

        public string fireDummyName;

        // private LinkedList<GameObject> m_ExecutorUsers;
        // private Dictionary<GameObject, RangedAttackExecutor> m_Executors;
        // private LinkedList<GameObject> m_PreviewItemUsers;
        // private Dictionary<GameObject, GameObject> m_PreviewObjects;
        // private Dictionary<GameObject, GameObject> m_PreviewFireDummyObjects;

        // 속성 (Properties)
        public override ItemType Type => ItemType.Weapon;
        public AttackDefinition WeaponData => this;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public void Execute(GameObject attacker, GameObject defender)
        {
            Use(attacker, defender);
        }

        // Private 메서드
        // Others
        public override Sprite GetIcon() => icon;
        public override string GetName() => itemName;

        public override void Use(GameObject caster, GameObject receiver)
        {
            RangedAttackExecutor executor = caster.GetComponent<RangedAttackExecutor>();
            if (executor == null)
            {
                Debug.LogWarning("RangedAttackExecutor를 찾을 수 없습니다.");
                return;
            }

            //    if (m_PreviewFireDummyObjects.ContainsKey(caster))
            //        executor.Fire(m_PreviewFireDummyObjects[caster].transform, caster, receiver);
            //    else
            //        executor.Fire(m_PreviewObjects[caster].transform, caster, receiver);
            executor.Fire(this, caster, receiver);
        }

    } // Scope by class NewScript
} // namespace SkyDragonHunter