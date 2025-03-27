using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SkyDragonHunter.Entities {

    public enum CrewType
    {
        OnBoard,
        OnField,
    }

    public class CrewControllerBT :  BaseControllerBT<CrewControllerBT>
    {
        // 필드 (Fields)
        [SerializeField] private CrewType m_Type;

        private static readonly string s_EnemyTag = "Monster";

        public bool isIdle = false;
        public float lastIdleTime;

        [SerializeField] private float targetYPos = float.MaxValue;
        public MountableSlot m_MountSlot;
        public bool isMounted;
        [SerializeField] public Vector3 onFieldOriginPosition;

        // Test용도 임시 필드
        private float initialDelay_TEMP = 1f;

        // 속성 (Properties)
        public float Speed => m_Speed;

        public float DistanceToOrigin
        {
            get
            {
                var currentPos = Vector3.zero;
                currentPos.x = transform.position.x;
                currentPos.y = floatingEffect.StartY;

                var distance = Vector3.Distance(onFieldOriginPosition, currentPos);

                return distance;
            }
        }

        public override bool IsTargetInAttackRange
        {
            get
            {
                return TargetDistance < m_CharacterInventory.weapons[0].range;
            }
        }

        public override bool IsTargetInAggroRange
        {
            get
            {
                //Debug.LogError($"Crew Controller D/A: {TargetDistance} / {m_AggroRange} // y : {targetYPos}");
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
        private void Update()
        {            
            UpdatePosition();

            // TEMP FROM ~
            if (initialDelay_TEMP <= 0f)
            {
                if (initialDelay_TEMP < 0f)
                {
                    floatingEffect.enabled = true;
                    initialDelay_TEMP = 0f;
                }
                m_BehaviourTree.Update();
            }
            else
            {
                initialDelay_TEMP -= Time.deltaTime;
            }
            // ~ TEMP TO

            //m_BehaviourTree.Update();
        }

        protected override void Start()
        {
            InitMountSlot();        
            base.Start();
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
            if (m_Target == null)
            {
                targetYPos = float.MaxValue;                
            }
            else
            {
                return;
            }           

            var colliders = Physics2D.OverlapCircleAll(transform.position, m_AggroRange);
            if (colliders.Length > 0)
            {                 
                foreach(var collider in colliders)
                {
                    if(collider.CompareTag(s_EnemyTag))
                    {
                        if (m_Target == null || !m_Target.Equals(collider.transform))
                        {
                            m_Target = collider.transform;
                            var floatingEffectComp = m_Target.gameObject.GetComponent<FloatingEffect>();
                            if (floatingEffectComp != null)
                            {
                                targetYPos = floatingEffectComp.StartY;
                                //Debug.LogError($"target Y pos reassigned {targetYPos}");
                            }
                            else
                            {
                                Debug.LogError($"no floatingEffect Comp");
                            }
                            //ResetBehaviourTree();
                        }                                                
                    }
                }                
            }
            else
            {
                Debug.Log($"crew target None");
            }
        }

        public void MountAction(bool mounted)
        {            
            if(isMounted != mounted)
            {
                if(mounted)
                {
                    floatingEffect.enabled = false;
                    m_MountSlot.Mounting(gameObject);
                }
                else
                {
                    floatingEffect.enabled = true;
                    m_MountSlot.Dismounting();
                }
            }
            isMounted = mounted;
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

        public override void SetAnimTrigger(string triggerName)
        {
            base.SetAnimTrigger(triggerName);
        }

        // Private 메서드
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

            //var idleSequence = new SequenceNode<CrewControllerBT>(this);
            //idleSequence.AddChild(new OnFieldCrewIdleCondition(this));
            //idleSequence.AddChild(new OnFieldCrewIdleAction(this));
            //rootSelector.AddChild(idleSequence);

            var returnSequence = new SequenceNode<CrewControllerBT>(this);
            returnSequence.AddChild(new CrewReturnCondition(this));
            returnSequence.AddChild(new CrewReturnAction(this));
            rootSelector.AddChild(returnSequence);

            var mountSequence = new SequenceNode<CrewControllerBT>(this);
            mountSequence.AddChild(new CrewMountCondition(this));
            mountSequence.AddChild(new CrewMountAction(this));
            rootSelector.AddChild(mountSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void UpdatePosition()
        {
            var newPos = transform.position;
            var direction = Vector3.zero;            

            if (m_Target != null && isChasing)
            {
                float multiplier = 3f;                

                direction.y = targetYPos - floatingEffect.StartY;

                if (targetYPos == float.MaxValue || Mathf.Abs(direction.y) < 0.1)
                    direction.y = 0;

                if (isDirectionToRight)
                {
                    direction.x = base.TargetDistance;
                }
                else
                {
                    direction.x = base.TargetDistance * -1;
                }

                var normal = direction.normalized;
                newPos.x += Time.deltaTime * m_Speed * normal.x * multiplier;
                floatingEffect.StartY += Time.deltaTime * m_Speed * normal.y * multiplier;

                transform.position = newPos;
            }
                                   
        }
        
        private void InitMountSlot()
        {           
            if (m_MountSlot == null)
            {
                throw new System.NullReferenceException("MountSlot cannot be found, please assign Mountslot on inspector");
            }
            isMounted = false;
            //onFieldOriginPosition = m_MountSlot.transform.position;
            onFieldOriginPosition = Vector3.zero;
            m_MountSlot.Mounting(gameObject);
            floatingEffect.StartY = transform.position.y;
            floatingEffect.enabled = false;
        }
        // Others

    } // Scope by class OnFieldCrewControllerBT

} // namespace Root