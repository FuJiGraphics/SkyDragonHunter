using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public enum EnemyType
    {
        Melee,
        Ranged,
        Stationary // Boss
    }

    public class EnemyControllerBT : BaseControllerBT<EnemyControllerBT>
    {
        // 필드 (Fields)
        [SerializeField] private EnemyType m_Type;

        private static readonly string s_PlayerTag = "Player";
        private static readonly string s_CrewTag = "Crew";
        private static readonly string s_CreatureTag = "Creature";

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            UpdatePosition();
            m_BehaviourTree.Update();
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, m_AggroRange);
            
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(transform.position, m_CharacterInventory.CurrentWeapon.range);                        
        //}

        // Public 메서드
        public override void ResetTarget()
        {
            bool resetRequired = false;
            if (m_Target == null || m_Target.gameObject.CompareTag(s_PlayerTag))
            {
                resetRequired = true;
            }
            else if (m_Target.gameObject.CompareTag(s_CrewTag))
            {
                var crewBT = m_Target.GetComponent<CrewControllerBT>();
                if (crewBT != null)
                {
                    if (crewBT.isOnBoard)
                        resetRequired = true;
                }                
            }

            if (!resetRequired)
                return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, m_AggroRange);
            if (colliders.Length > 0)
            {
                foreach(var collider in colliders)
                {
                    if (collider.CompareTag(s_CreatureTag))
                    {
                        m_Target = collider.transform;
                        return;
                    }
                    if (collider.CompareTag(s_CrewTag))
                    {
                        var crewBT = collider.GetComponent<CrewControllerBT>();
                        if (crewBT != null)
                        {
                            var onBoard = crewBT.isOnBoard;
                            if (onBoard)
                            {                                
                                continue;
                            }
                            else
                            {                                
                                m_Target = collider.transform;
                                return;
                            }
                        }
                    }
                    m_Target = GameObject.FindWithTag(s_PlayerTag).transform;
                    return;
                }
                if (m_Target == null)
                {
                    m_Target = GameObject.FindWithTag(s_PlayerTag).transform;
                    return;
                }
            }
            else
            {
                m_Target = GameObject.FindWithTag(s_PlayerTag).transform;
            }
        }

        public void NullTarget()
        {
            m_Target = null;
        }

        // Protected 메서드
        protected override void InitBehaviourTree()
        {
            switch (m_Type)
            {
                case EnemyType.Melee:     
                case EnemyType.Ranged:
                    InitMeleeBT();
                    break;
                case EnemyType.Stationary:
                    break;
            }
        }

        private void InitMeleeBT()
        {
            m_BehaviourTree = new BehaviourTree<EnemyControllerBT>(this);

            var rootSelector = new SelectorNode<EnemyControllerBT>(this);

            var attackSequence = new SequenceNode<EnemyControllerBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<EnemyControllerBT>(this));
            attackSequence.AddChild(new EntityAttackAction<EnemyControllerBT>(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<EnemyControllerBT>(this);
            chaseSequence.AddChild(new EntityChasableCondition<EnemyControllerBT>(this));
            chaseSequence.AddChild(new EntityChaseAction<EnemyControllerBT>(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<EnemyControllerBT>(this);
            moveSequence.AddChild(new EntityMoveCondition<EnemyControllerBT>(this));
            moveSequence.AddChild(new EntityMoveAction<EnemyControllerBT>(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }
                
        private void UpdatePosition()
        {
            var newPos = transform.position;
            
            //float newYPos = Mathf.Sin((Time.time + rand) * (2 * Mathf.PI / m_YaxisMovementPeriod)) * m_YaxisMovementAmplitude;
            //
            //newPos.y = m_InitialYPos + newYPos;                        

            if (isChasing || isMoving)
            {
                int toRight = 1;
                if (isChasing)
                    toRight *= 3;
                if (!isDirectionToRight)
                    toRight *= -1;
                newPos.x += Time.deltaTime * m_Speed * toRight;                
            }

            // Debug.Log($"new Position: {newPos}");
            transform.position = newPos;
        }
        // Others
    } // Scope by class EnemyControllerBT

} // namespace Root