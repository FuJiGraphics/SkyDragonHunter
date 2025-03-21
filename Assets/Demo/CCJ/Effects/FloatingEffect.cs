using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    // 필드 (Fields)
    public float speed = 1f;
    public float yOffset = 1f;

    private float m_lastAngle;

    // 속성 (Properties)
    // 외부 종속성 필드 (External dependencies field)
    // 이벤트 (Events)
    // 유니티 (MonoBehaviour 기본 메서드)
    private void Update()
    {
        m_lastAngle += Time.deltaTime * speed;
        m_lastAngle %= Mathf.PI * 2f;

        float angle = Mathf.Cos(m_lastAngle);
        var pos = transform.position;
        pos.y += angle * yOffset ;
        transform.position = pos;
    }

    // Public 메서드
    // Private 메서드
    // Others

} // Scope by class FloatingEffect
