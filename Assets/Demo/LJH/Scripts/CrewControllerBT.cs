using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public enum CrewType
    {
        OnBoard,
        OnField,
    }

    public class CrewControllerBT : BaseControllerBT<CrewControllerBT>
    {
        // 필드 (Fields)
        [SerializeField] private CrewType m_Type;

        private static readonly string s_EnemyTag = "Monster";

        public bool isIdle = false;
        public float lastIdleTime;

        [SerializeField] private float targetYPos = float.MaxValue;
        [SerializeField] private MountableSlot m_MountSlot;

        // 속성 (Properties)
        public override bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < m_CharacterInventory.CurrentWeapon.range;
            }
        }

        public override bool IsTargetInAggroRange
        {
            get
            {
                return TargetDistance < m_AggroRange;
            }
        }

        public override float TargetDistance
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
                    if(isDirectionToRight == false)
                    {
                        var newScale = transform.localScale;
                        newScale.y *= -1;
                        transform.localScale = newScale;
                    }
                    isDirectionToRight = true;                    
                }
                else
                {
                    if (isDirectionToRight == true)
                    {
                        var newScale = transform.localScale;
                        newScale.y *= -1;
                        transform.localScale = newScale;
                    }
                    isDirectionToRight = false;                    
                }

                float newYPos = 0f;
                if (targetYPos != float.MaxValue)
                    newYPos = targetYPos;

                Vector3 targetPos = new Vector3(m_Target.position.x, targetYPos, 0);                

                var sr = m_Target.gameObject.GetComponent<SpriteRenderer>();
                var distanceCallibrator = (sr.bounds.size.x + sr.bounds.size.y) * 0.25f;

                distance = Vector3.Distance(targetPos, transform.position);

                distance += distanceCallibrator;

                return distance;
            }
        }

        // 유니티 (MonoBehaviour 기본 메서드)
        //protected override void Start()
        //{
        //   
        //    InitBehaviourTree();
        //}

        private void Update()
        {
            UpdatePosition();
            m_BehaviourTree.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_AggroRange);

            if(m_CharacterInventory == null)
            {
                Debug.LogWarning($"Character inventory Null of {gameObject.name}");
            }
            else if (m_CharacterInventory.CurrentWeapon == null)
            {
                //Debug.LogWarning($"CurrentWeapon Null of {gameObject.name}");
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_CharacterInventory.weapons[0].range);            
        }

        // Public 메서드
        public override void ResetTarget()
        {            
            bool resetRequired = false;
            bool isPrevTargetNull = false;
            if (m_Target == null)
            {
                targetYPos = float.MaxValue;
                resetRequired = true;
                isPrevTargetNull = true;
            }

            if (!resetRequired)
                return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, m_AggroRange);
            if (colliders.Length > 0)
            { 
                foreach(var collider in colliders)
                {
                    if(collider.CompareTag(s_EnemyTag))
                    {
                        Debug.Log($"found Target {collider.name}");
                        if(!m_Target.Equals(collider.transform))
                        {
                            targetYPos = collider.GetComponent<FloatingEffect>().StartY;
                        }
                        m_Target = collider.transform;
                        if (isPrevTargetNull)
                        {
                            ResetBehaviourTree();
                            return;
                        }                        
                    }
                }                
            }
            else
            {
                Debug.Log($"crew target None");
            }
        }

        // Protected 메서드
        protected override void InitBehaviourTree()
        {
            switch (m_Type)
            {
                case CrewType.OnBoard:
                    InitOnBoardCrewBT();
                    break;
                case CrewType.OnField:
                    InitOnFieldCrewBT();
                    break;
            }
        }

        private void InitOnBoardCrewBT()
        {
            m_BehaviourTree = new BehaviourTree<CrewControllerBT>(this);
            var rootSelector = new SelectorNode<CrewControllerBT>(this);

            var attackSequence = new SequenceNode<CrewControllerBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<CrewControllerBT>(this));
            attackSequence.AddChild(new EntityAttackAction<CrewControllerBT>(this));
            rootSelector.AddChild(attackSequence);

            var IdleSequence = new SequenceNode<CrewControllerBT>(this);
            IdleSequence.AddChild(new OnFieldCrewIdleCondition(this));
            IdleSequence.AddChild(new OnFieldCrewIdleAction(this));
            rootSelector.AddChild(IdleSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void InitOnFieldCrewBT()
        {
            m_BehaviourTree = new BehaviourTree<CrewControllerBT>(this);
            var rootSelector = new SelectorNode<CrewControllerBT>(this);

            var attackSequence = new SequenceNode<CrewControllerBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<CrewControllerBT>(this));
            attackSequence.AddChild(new EntityAttackAction<CrewControllerBT>(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<CrewControllerBT>(this);
            chaseSequence.AddChild(new EntityChasableCondition<CrewControllerBT>(this));
            chaseSequence.AddChild(new EntityChaseAction<CrewControllerBT>(this));
            rootSelector.AddChild(chaseSequence);

            var IdleSequence = new SequenceNode<CrewControllerBT>(this);
            IdleSequence.AddChild(new OnFieldCrewIdleCondition(this));
            IdleSequence.AddChild(new OnFieldCrewIdleAction(this));
            rootSelector.AddChild(IdleSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void UpdatePosition()
        {
            var newPos = transform.position;
            var direction = Vector3.zero;

            //if (isChasing || isMoving)
            //{
            //    int toRight = 1;
            //    if (isChasing)
            //        toRight *= 3;
            //    if (!isDirectionToRight)
            //        toRight *= -1;                
            //    newPos.x += Time.deltaTime * m_Speed * toRight;
            //}                       

            if (m_Target != null && isChasing)
            {
                float multiplier = 3f;
                //if (isChasing)
                //    multiplier = 3f;

                direction.y = targetYPos - floatingEffect.StartY;

                if (targetYPos == float.MaxValue || direction.y < 0.1)
                    direction.y = 0;

                if (isDirectionToRight)
                {
                    direction.x = base.TargetDistance;
                }
                else
                {
                    direction.x = base.TargetDistance * -1;
                }

                direction.Normalize();
                newPos.x += Time.deltaTime * m_Speed * direction.x * multiplier;
                floatingEffect.StartY += Time.deltaTime * m_Speed * direction.y * multiplier;

                transform.position = newPos;
            }
            //else
            //{
            //    targetYPos = float.MaxValue;
            //}           

            // Debug.Log($"new Position: {newPos}");            
        }
        // Others

    } // Scope by class OnFieldCrewControllerBT

} // namespace Root