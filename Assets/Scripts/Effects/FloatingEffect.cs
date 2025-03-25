using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    // 필드 (Fields)
    public float floatSpeed = 1f;
    public float floatSpeedOffset = 1f; 
    public float floatAmount = 0.5f;

    [SerializeField] private float m_StartY;
    private float m_lastAngle;

    // 속성 (Properties)
    public float StartY
    {
        get
        {
            return m_StartY;
        }
        set
        {
            m_StartY = value;
        }
    }
    public float SetStartY(float y) => m_StartY = y;

    // 외부 종속성 필드 (External dependencies field)
    // 이벤트 (Events)
    // 유니티 (MonoBehaviour 기본 메서드)
    private void Awake()
    {
        m_StartY = transform.position.y;
    }

    private void Update()
    {
        m_lastAngle += Time.deltaTime * floatSpeed * floatSpeedOffset;

        float offset = Mathf.Cos(m_lastAngle) * floatAmount;
        var pos = transform.position;
        pos.y = m_StartY + offset;
        transform.position = pos;
    }

    // Public 메서드
    // Private 메서드
    // Others

} // Scope by class FloatingEffect
