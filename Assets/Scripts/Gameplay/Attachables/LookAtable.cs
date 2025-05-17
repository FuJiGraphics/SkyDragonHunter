using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{
    public class LookAtable : MonoBehaviour
        , ILookAtProvider
    {
        // 필드 (Fields)
        [SerializeField] private Transform spinner;
        [SerializeField] private Transform start;
        [SerializeField] private float rotateSpeed = 15f;

        private GameObject m_CurrentTarget = null;
        private bool isResetting = false;

        // 속성 (Properties)
        public bool IsTargetLook { get; private set; } = false;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        private void Update()
        {
            if (m_CurrentTarget == null && !isResetting)
            {
                isResetting = true;
                Quaternion targetRotation = Math2DHelper.GetLookAtRotation(start.position, Vector3.right);
                StartCoroutine(SmoothRotateReroll(spinner, targetRotation, () =>
                {
                    IsTargetLook = true;
                    m_CurrentTarget = null;
                    isResetting = false;
                }));
            }
        }

        // Public 메서드
        // Private 메서드
        // Others
        public void LookAt(GameObject target)
        {
            IsTargetLook = false;
            m_CurrentTarget = target;
            // 목표 회전값 계산
            Quaternion targetRotation = Math2DHelper.GetLookAtRotation(start.position, target.transform.position);

            // 부드럽게 회전
            StartCoroutine(SmoothRotate(spinner, targetRotation, () =>
            {
                IsTargetLook = true;
                m_CurrentTarget = null;
            }));
        }

        public void ResetDirection()
        {
            if (m_CurrentTarget == null)
            {
                Quaternion targetRotation = Math2DHelper.GetLookAtRotation(start.position, start.position + Vector3.right);
                StartCoroutine(SmoothRotateReroll(spinner, targetRotation, () =>
                {
                    IsTargetLook = true;
                    m_CurrentTarget = null;
                }));
            }
        }

        private IEnumerator SmoothRotate(Transform spinner, Quaternion targetRotation, System.Action onComplete)
        {
            const float angleThreshold = 1f;

            while (Quaternion.Angle(spinner.rotation, targetRotation) > angleThreshold)
            {
                spinner.rotation = Quaternion.RotateTowards(spinner.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }

            spinner.rotation = targetRotation;
            onComplete?.Invoke();
        }

        private IEnumerator SmoothRotateReroll(Transform spinner, Quaternion targetRotation, System.Action onComplete)
        {
            const float angleThreshold = 1f;

            while (Quaternion.Angle(spinner.rotation, targetRotation) > angleThreshold)
            {
                spinner.rotation = Quaternion.RotateTowards(spinner.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;

                if (m_CurrentTarget != null)
                {
                    yield break;
                }
            }

            spinner.rotation = targetRotation;
            onComplete?.Invoke();
        }

    } // Scope by class LookAtable
} // namespace Root
