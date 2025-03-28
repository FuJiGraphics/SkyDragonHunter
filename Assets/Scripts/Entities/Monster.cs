using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Entities 
{
    public class Monster : MonoBehaviour
    {
        // 필드 (Fields)
        private AlphaUnit m_Damage;
        private AlphaUnit m_HealthPoint;

        private float m_AttackDuration;
        private float m_AttackTimer;

        private float m_MoveSpeed;

        private float m_AttackRange;
        private float m_AggroRange;

        private bool m_IsDead;
        private float m_DeathTimer;

        private float m_DamagedTimer;

        [SerializeField] private Transform m_AttackTarget;

        private SpriteRenderer m_SpriteRenderer;
        // 테스트용 나중에 삭제
        private bool OnTestDestroy;
        float testDestroyStartTime = 0f;
        // 테스트용 나중에 삭제

        // 속성 (Properties)
        public AlphaUnit Damage => m_Damage;
        public AlphaUnit HealthPoint => m_HealthPoint;
        public bool m_IsAttackable
        {
            get
            {
                bool success = false;
                if (m_AttackTarget == null)
                    return false;

                var targetPos = m_AttackTarget.position;
                if (Vector3.Distance(targetPos, transform.position) < m_AttackRange)
                {
                    success = true;
                }
                
                return success;
            }
        }

        public Transform AttackTarget => m_AttackTarget;        

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_Damage = 100;
            m_HealthPoint = 50;
            m_AttackDuration = 1.5f;
            m_AttackTimer = 0f;
            m_IsDead = false;
            m_DeathTimer = 1f;
            m_DamagedTimer = 0f;
            m_MoveSpeed = 4f;
            m_AttackRange = 3f;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_AttackTarget = GameObject.FindWithTag("Finish").transform;
            OnTestDestroy = false;
        }
    
        private void Update()
        {
            BlinkOnDamage();

            if (m_IsDead)
                return;

            m_AttackTimer -= Time.deltaTime;
            if(m_IsAttackable && m_AttackTimer <= 0)
            {
                Attack();
                OnTestDestroy = true;
            }

            if (OnTestDestroy)
            {
                testDestroyStartTime += Time.deltaTime;
                if (testDestroyStartTime > m_DeathTimer)
                {
                    Destroy(gameObject);
                }
            }

            BlinkOnAttack();
            UpdatePosition();

            ForTestOnly();
        }

        // Public 메서드
        public void SetMonsterStats(AlphaUnit damage, AlphaUnit hp, float attackDuration, float moveSpeed, float attackRange)
        {
            m_Damage = damage;
            m_HealthPoint = hp;
            m_AttackDuration = attackDuration;
            m_AttackTimer = m_AttackDuration;
            m_MoveSpeed = moveSpeed;
            m_AttackRange = attackRange;
        }

        public void TakeDamage(AlphaUnit damage)
        {
            m_DamagedTimer = 1f;
            m_HealthPoint -= damage;
            if (m_HealthPoint < 0)
            {
                m_HealthPoint = 0;
                Die();
            }            
        }
        private void Attack()
        {
            m_AttackTimer = m_AttackDuration;
            if(m_AttackTimer > 0f)
            {
                return;
            }

            // What to do On Attack

        }

        // Private 메서드
        private void ForTestOnly()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                Attack();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                TakeDamage(10);
                Debug.Log("s누름");
            }
        }

        private void BlinkOnDamage()
        {
            if (m_DamagedTimer <= 0)
                return;

            if (m_DamagedTimer > 0.7f)
            {
                m_SpriteRenderer.color = Color.red;
            }
            else
            {
                m_SpriteRenderer.color = Color.white;
            }

            m_DamagedTimer -= Time.deltaTime;
            m_DamagedTimer = Mathf.Max(0f, m_DamagedTimer);
        }

        private void BlinkOnAttack()
        {
            if (m_AttackTimer <= m_AttackDuration - 0.2f)
            {
                if(m_SpriteRenderer.color == Color.blue)
                {
                    m_SpriteRenderer.color = Color.white;
                }
                return;
            }

            if (m_AttackTimer > m_AttackDuration - 0.2f)
            {
                m_SpriteRenderer.color = Color.blue;
            }
        }

        private void UpdatePosition()
        {
            if(m_IsAttackable)
            {
                return;
            }

            Vector3 movement = new Vector3(-m_MoveSpeed, 0f, 0f);
            transform.position += movement * Time.deltaTime;
        }

        private void Die()
        {
            Destroy(gameObject, m_DeathTimer);
        }


        // Others

    } // Scope by class Monster

} // namespace Root