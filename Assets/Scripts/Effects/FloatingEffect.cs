using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    // 필드 (Fields)
    [SerializeField] private float m_FloatSpeed = 1f;
    [SerializeField] private float m_FloatAmount = 0.5f;

    private float m_lastAngle;

    // 속성 (Properties)
    public float FloatSpeedOffset { get; set; } = 1f;

    // 외부 종속성 필드 (External dependencies field)
    // 이벤트 (Events)
    // 유니티 (MonoBehaviour 기본 메서드)
    private void Update()
    {
        m_lastAngle += Time.deltaTime * (m_FloatSpeed * 0.1f) * FloatSpeedOffset;

        float offset = Mathf.Cos(m_lastAngle) * (m_FloatAmount * 0.01f);
        var pos = transform.position;
        pos.y += offset;
        transform.position = pos;
    }

    // Public 메서드
    // Private 메서드
    // Others

} // Scope by class FloatingEffect
