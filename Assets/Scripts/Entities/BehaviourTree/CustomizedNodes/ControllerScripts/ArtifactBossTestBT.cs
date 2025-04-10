using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkyDragonHunter.Gameplay;

namespace SkyDragonHunter.Entities 
{
    public enum ArtifactBossAttackType
    {
        Melee,
        Ranged,
    }

    public class ArtifactBossTestBT : BaseControllerBT<ArtifactBossTestBT>
    {
        // Static Fields
        private static int s_InstanceNo = 0;

        // Fields
        [SerializeField] private BossAttackType m_AttackType;
        public int skillId;
        private float m_skillCooltime;
        public float lastSkillCasted;
        new public bool IsSkillAvailable;

        private CrewControllerBT m_CrewTarget;

        public Artifact[] artifacts;

        // Properties
        //public override bool IsSkillAvailable
        //{
        //    get
        //    {
        //        return Time.time > lastSkillCasted + m_skillCooltime;
        //    }
        //}

        // Unity Methods
        protected override void Start()
        {
            base.Start();
            SetArtifactStatus();
        }

        protected void Update()
        {
            UpdatePosition();
            m_BehaviourTree.Update();

            IsSkillAvailable = Time.time > lastSkillCasted + m_skillCooltime;
        }

        // Public Methods        
        public void UseSkill()
        {
            // TODO
            StartCoroutine(SkillShot());
            lastSkillCasted = Time.time;
            Debug.Log($"Skill id [{skillId}] used at '{Time.time}', CD : {m_skillCooltime}");
        }

        public IEnumerator SkillShot()
        {
            SetAnimTrigger(0);
            yield return new WaitForSeconds(0.1f);
            SetAnimTrigger(0);
            yield return new WaitForSeconds(0.1f);
            SetAnimTrigger(0);
            yield return new WaitForSeconds(0.1f);
            SetAnimTrigger(0);
            yield return new WaitForSeconds(0.1f);
            SetAnimTrigger(0);            
        }

        public override void ResetHealth()
        {
            throw new System.NotImplementedException();
        }

        public override void SetDataFromTable(int id)
        {
            ID = id;
            var data = DataTableMgr.BossTable.Get(id);
            if (data == null)
            {
                Debug.LogError($"Set ArtifactBoss Data Failed : ID '{id}' not found in boss table.");
                return;
            }

            name = data.Name;
            m_AttackType = data.Type;
            status.MaxHealth = data.HP;
            status.MaxDamage = data.ATK;
            status.MaxArmor = data.DEF;
            status.MaxResilient = data.REG;
            projectileId = data.ProjectileID;
            m_AttackInterval = data.AttackInterval;
            m_AttackRange = data.AttackRange;
            m_AggroRange = data.AggroRange;
            m_Speed = data.Speed;
            m_ChaseSpeed = data.ChaseSpeed;
            skillId = data.SkillID;
            m_skillCooltime = data.SkillCooltime;
        } 
        
        private void SetArtifactStatus()
        {
            
        }

        public override void ResetTarget()
        {
            if (m_CrewTarget != null && !m_CrewTarget.isMounted)
            {
                m_Target = m_CrewTarget.transform;
                return;
            }
            else
            {
                m_CrewTarget = null;
                if (m_Target == null)
                {
                    m_Target = GameObject.FindWithTag($"Player").transform;
                }
                else if (!m_Target.CompareTag(s_PlayerTag))
                {
                    m_Target = GameObject.FindWithTag($"Player").transform;
                }
            }

            var colliders = Physics2D.OverlapCircleAll(transform.position, m_AggroRange);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_CrewTag))
                    {
                        var crewBT = collider.GetComponent<CrewControllerBT>();
                        if (crewBT != null)
                        {
                            var onBoard = crewBT.isMounted;
                            if (onBoard)
                            {
                                continue;
                            }
                            else
                            {
                                m_CrewTarget = crewBT;
                                return;
                            }
                        }
                    }
                    m_CrewTarget = null;
                    return;
                }
            }
        }

        protected override void InitBehaviourTree()
        {
            m_BehaviourTree = new BehaviourTree<ArtifactBossTestBT>(this);

            var rootSelector = new SelectorNode<ArtifactBossTestBT>(this);

            var attackSequence = new SequenceNode<ArtifactBossTestBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<ArtifactBossTestBT>(this));
            attackSequence.AddChild(new EntityAttackAction<ArtifactBossTestBT>(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<ArtifactBossTestBT>(this);
            chaseSequence.AddChild(new EntityChasableCondition<ArtifactBossTestBT>(this));
            chaseSequence.AddChild(new EntityChaseAction<ArtifactBossTestBT>(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<ArtifactBossTestBT>(this);
            moveSequence.AddChild(new EntityMoveCondition<ArtifactBossTestBT>(this));
            moveSequence.AddChild(new EntityMoveAction<ArtifactBossTestBT>(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void UpdatePosition()
        {
            var newPos = transform.position;

            if (IsChasing || isMoving)
            {
                int toRight = 1;
                if (IsChasing)
                    toRight *= 3;
                if (!isDirectionToRight)
                    toRight *= -1;
                newPos.x += Time.deltaTime * m_Speed * toRight;
            }

            transform.position = newPos;
        }
    } // Scope by class ArtifactBossControllerBT

} // namespace Root