using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities
{
    public abstract class BaseControllerBT<T> : MonoBehaviour where T : BaseControllerBT<T>
    {
        // 필드 (Fields)
        [SerializeField] protected float m_Speed;
        [SerializeField] protected float m_AggroRange;

        protected BehaviourTree<T> m_BehaviourTree;

        public AttackDefinition attackDefinition;
        public CharacterStatus status;

        protected Animator m_Animator;

        [SerializeField] protected Transform m_Target;

        public bool isDirectionToRight = false;
        public bool isMoving = false;
        public bool isChasing = false;

        // 속성 (Properties)
        public virtual bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < attackDefinition.range;
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

                return Mathf.Abs(distance);
            }
        }

        // 유니티 (MonoBehaviour 기본 메서드)
        protected virtual void Start()
        {
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
            if (m_Target != null)
            {
                var inven = gameObject.GetComponent<CharacterInventory>();
                if (inven != null)
                {
                    inven.CurrentWeapon.SetActivePrefabInstance(gameObject);
                    inven.CurrentWeapon.Execute(gameObject, m_Target.gameObject);
                }
            }
        }

        public abstract void ResetTarget();

        public virtual void ResetBehaviourTree()
        {
            m_BehaviourTree.Reset();
        }

        // Protected 메서드
        protected abstract void InitBehaviourTree();

        // Others

    } // Scope by class BaseControllerBT

} // namespace Root