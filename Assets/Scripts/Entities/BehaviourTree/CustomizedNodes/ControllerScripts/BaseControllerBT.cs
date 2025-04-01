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
        // 필드 (Fields)
        [SerializeField] protected float m_Speed;
        [SerializeField] protected float m_AggroRange;

        [SerializeField] protected BehaviourTree<T> m_BehaviourTree;

        public CharacterInventory m_CharacterInventory;
        //public AttackDefinition attackDefinition;
        public CharacterStatus status;

        protected Animator m_Animator;
        public FloatingEffect floatingEffect;

        [SerializeField] protected Transform m_Target;

        public bool isDirectionToRight = false;
        public bool isMoving = false;
        public bool isChasing = false;

        // 속성 (Properties)
        public Transform Target => m_Target;

        public virtual bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < m_CharacterInventory.CurrentWeapon.WeaponData.range;
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
            m_CharacterInventory = GetComponent<CharacterInventory>();
            floatingEffect = GetComponent<FloatingEffect>();
        }

        protected virtual void Start()
        {
            if(m_CharacterInventory == null)
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

            if(m_CharacterInventory != null)
            {
                m_CharacterInventory.CurrentWeapon.Execute(gameObject, m_Target.gameObject);
                //Debug.LogError($"{gameObject.name} attacked {m_Target.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} Character inventory null");
            }
        }

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