using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    [CreateAssetMenu(fileName = "CanonDefinition.asset", menuName = "Canons/CanonDefinition")]
    public class CanonDefinition : ScriptableObject
    {
        // 필드 (Fields)
        public int id;                      // 대포ID
        public string canName;              // 대포 이름
        public int canGrade;                // 노말=0 레어=1 유니크=2 레전드=3 
        public string canEqATK;             // 기본 장착 공격력
        public string canEqDEF;             // 기본 장착 방어력
        public string canHoldATK;           // 기본 보유 공격력
        public string canHoldDEF;           // 기본 보유 방어력
        public string canLvUpCost;          // 레벨 업 비용 기준 값
        public string canEqATKup;           // 장착 공격력 상승량
        public string canEqDEFup;           // 장착 방어력 상승량
        public string canHoldATKup;         // 보유 공격력 상승량
        public string canHoldDEFup;         // 보유 방어력 상승량
        public int canEffectID;             // 대포 특수 효과 ID
        public int canUpgradeID;            // 대포 합성 결과 ID
        public float canCooldown;           // 대포 쿨다운
        public int canAilmentID;            // 상태 이상 ID
        public float canAilmentDuration;    // 상태 이상 지속 시간

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        public Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            if (aStats == null || dStats == null)
            {
                Debug.LogWarning("[CanonDefinition]: CreateAttack 도중 알 수 없는 오류");
                return new Attack();
            }

            Attack attack = new Attack();
            attack.defender = dStats.gameObject;
            BigInteger damage = aStats.MaxDamage.Value;
            float criticalChance = Mathf.Clamp01(aStats.CriticalChance);

            /* ccj)
             * double 이하의 값들은 1.5까지도 배수 처리 가능하게하고 
             * double을 초과하는 값들은 1.5의 소수점을 버리고 연산하는 식을 채택
             */
            if (criticalChance > UnityEngine.Random.value)
            {
                double criDamage = (double)damage * aStats.CriticalMultiplier;
                if (criDamage < double.MaxValue)
                {
                    damage = (BigInteger)criDamage;
                }
                else
                {
                    damage *= (BigInteger)aStats.CriticalMultiplier;
                }
                attack.isCritical = true;
            }
            if (dStats.gameObject.tag == "Boss")
            {
                double bossDamage = (double)damage * aStats.BossDamageMultiplier;
                if (bossDamage < double.MaxValue)
                {
                    damage = (BigInteger)bossDamage;
                }
                else
                {
                    damage *= (BigInteger)aStats.BossDamageMultiplier;
                }
            }
            attack.damage = damage;
            if (dStats != null)
            {
                attack.damage -= dStats.Armor;
                if (attack.damage <= 1.0)
                    attack.damage = 1.0;
            }
            return attack;
        }

        // Others

    } // Scope by class AttackDefinition
} // namespace SkyDragonHunter