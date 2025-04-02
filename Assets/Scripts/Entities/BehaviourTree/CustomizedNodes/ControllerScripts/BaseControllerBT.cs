using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities
{
    public abstract class BaseControllerBT<T> : MonoBehaviour, ISlowable 
        where T : BaseControllerBT<T>
    {
        // Static Fields
        protected static readonly string s_PlayerTag = "Player";
        protected static readonly string s_CrewTag = "Crew";
        protected static readonly string s_CreatureTag = "Creature";
        protected static readonly string s_EnemyTag = "Monster";

        // 필드 (Fields)
        [SerializeField] protected float m_Speed;
        [SerializeField] protected float m_AggroRange;
        [SerializeField] protected BehaviourTree<T> m_BehaviourTree;
        [SerializeField] protected Animator m_Animator;

        public string Name;

        public CharacterInventory characterInventory;
        public CharacterStatus status;

        public FloatingEffect floatingEffect;

        [SerializeField] protected Transform m_Target;

        public bool isDirectionToRight = false;
        public bool isMoving = false;
        public bool isChasing = false;
        
        public int ProjectileId;

        // 속성 (Properties)
        public Transform Target => m_Target;

        public virtual bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < characterInventory.CurrentWeapon.range;
            }
        }

        public virtual bool IsTargetInAggroRange
        {
            get
            {
                return TargetDistance < m_AggroRange;
            }
        }

        public virtual float TargetDistance
        {
            get
            {                
                if (m_Target == null)
                {
                    return float.MaxValue;
                }                

                var distance = m_Target.position.x - transform.position.x;                
                if (distance > 0)
                {
                    isDirectionToRight = true;
                }
                else
                {
                    isDirectionToRight = false;
                }

                var sr = m_Target.gameObject.GetComponentInChildren<SpriteRenderer>();
                var halfwidth = sr.bounds.size.x * 0.5f;

                if (isDirectionToRight)
                {
                    distance -= halfwidth;
                }
                else
                {
                    distance += halfwidth;
                }
                distance = Mathf.Abs(distance);

                if (distance < halfwidth)                
                    distance = 0f;

                return distance;
            }
        }

        // 유니티 (MonoBehaviour 기본 메서드)
        protected virtual void Awake()
        {
            characterInventory = GetComponent<CharacterInventory>();
            floatingEffect = GetComponent<FloatingEffect>();
        }

        protected virtual void Start()
        {
            if(characterInventory == null)
            {
                Debug.LogError($"Character Inventory null in {gameObject.name}");
            }
            InitBehaviourTree();
        }

        protected virtual void FixedUpdate()
        {
            ResetTarget();
        }

        // Public 메서드
        public virtual void SetAnimTrigger(string triggerName)
        {
            var triggerHash = Animator.StringToHash(triggerName);
            Debug.Log($"Recommended to use trigger hash({triggerHash}) instead of trigger name({triggerName})");
            SetAnimTrigger(triggerHash);
        }

        public virtual void SetAnimTrigger(int triggerHash)
        {
            //m_Animator.SetTrigger(triggerHash);

            if(characterInventory != null)
            {
                characterInventory.CurrentWeapon.SetActivePrefabInstance(gameObject);
                characterInventory.CurrentWeapon.Execute(gameObject, m_Target.gameObject);
                //Debug.LogError($"{gameObject.name} attacked {m_Target.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} Character inventory null");
            }
        }

        public abstract void SetDataFromTable(int id);

        public abstract void ResetTarget();

        public virtual void ResetBehaviourTree()
        {
            m_BehaviourTree.Reset();
        }

        // Protected 메서드
        protected abstract void InitBehaviourTree();

        public void OnSlowBegin(float slowMultiplier)
        {
            m_Speed *= slowMultiplier;
        }
        public void OnSlowEnd(float slowMultiplier)
        {
            m_Speed *= (1 / slowMultiplier);
        }
        // Others

    } // Scope by class BaseControllerBT

} // namespace Root