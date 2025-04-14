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
        protected static readonly string s_EnemyTag = "Monster";
        protected static readonly string s_PlayerTag= "Player";
        protected static readonly string s_CrewTag = "Crew";
        protected static readonly string s_Creature = "Creature";

        protected static readonly string s_AnimNameIdle1 = "Idle_1";

        // 필드 (Fields)
        [SerializeField] protected string[] m_AttackAnimTriggers;
        [SerializeField] protected string[] m_SkillAnimTriggers;

        [SerializeField] protected float m_Speed;
        [SerializeField] protected float m_ChaseSpeed;
        [SerializeField] protected float m_AggroRange;
        [SerializeField] protected float m_AttackRange;
        [SerializeField] public float m_AttackInterval;

        [SerializeField] protected BehaviourTree<T> m_BehaviourTree;

        public CharacterInventory characterInventory;
        public int ID;
        public int projectileId;

        //public AttackDefinition attackDefinition;
        public CharacterStatus status;

        protected Animator m_Animator;
        public FloatingEffect floatingEffect;

        [SerializeField] protected Transform m_Target;

        public bool isDirectionToRight = false;
        public bool isMoving = false;
        public virtual bool IsChasing { get; set; }

        public float lastAttackTime;
        public bool attackAnimationPlaying;               

        // 속성 (Properties)
        public virtual bool IsSkillAvailable => false;
        public virtual bool IsTargetNull => m_Target == null;

        public Transform Target => m_Target;

        public virtual bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < m_AttackRange;
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

            // TODO : Temporary code for test only
            #region
            
            #endregion
        }

        protected virtual void FixedUpdate()
        {
            ResetTarget();
        }

        // Public 메서드
        public virtual void TriggerAttack()
        {
            if (characterInventory != null)
            {
                characterInventory.CurrentWeapon.Execute(gameObject, m_Target.gameObject);
                //Debug.LogError($"{gameObject.name} attacked {m_Target.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} Character inventory null");
            }
        }

        public virtual void SetAnimTrigger(int triggerHash)
        {
            //m_Animator.SetTrigger(triggerHash);

            if(characterInventory != null)
            {
                characterInventory.CurrentWeapon.Execute(gameObject, m_Target.gameObject);
                //Debug.LogError($"{gameObject.name} attacked {m_Target.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} Character inventory null");
            }
        }

        public abstract void ResetTarget();
        public abstract void SetDataFromTable(int id);

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

        [ContextMenu("Reset Health")]
        public abstract void ResetHealth();


    } // Scope by class BaseControllerBT

} // namespace Root