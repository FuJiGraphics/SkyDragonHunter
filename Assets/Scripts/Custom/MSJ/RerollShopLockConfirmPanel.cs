using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class RerollShopLockConfirmPanel : MonoBehaviour
    {
        // 필드 (Fields)
        // 확인 메시지를 표시할 텍스트
        [SerializeField] private TextMeshProUGUI confirmText;

        // 확인/취소 버튼
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        // 확인 후 실행할 콜백 (잠금 또는 해제 처리)
        private Action onConfirmCallback;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // 초기화: 버튼 리스너 등록
        private void Awake()
        {
            // 확인 버튼에 메서드 연결
            confirmButton.onClick.AddListener(OnClickConfirm);

            // 취소 버튼에 메서드 연결
            cancelButton.onClick.AddListener(OnClickCancel);

            // 시작 시 비활성화
            gameObject.SetActive(false);
        }

        // Public 메서드
        /// <summary>
        /// 패널 열기 함수. 슬롯에서 호출.
        /// </summary>
        /// <param name="message">출력할 문구 ("정말 잠그시겠습니까?" 등)</param>
        /// <param name="confirmCallback">확인 시 실행할 로직</param>
        /// <param name="position">패널 위치를 맞출 기준 위치</param>
        public void Open(Action confirmCallback, Vector3 position, string message, bool islocked)
        {
            // 콜백 저장
            onConfirmCallback = confirmCallback;

            // 메시지 세팅
            confirmText.text = message;

            // 임시 활성화 → 위치 설정 → 다시 비활성화 → 최종 활성화
            gameObject.SetActive(true); // (1) 잠깐 켬
            transform.position = position; // (2) 위치 설정
            gameObject.SetActive(false); // (3) 다시 끔
            gameObject.SetActive(true); // (4) 정상적으로 다시 켬
        }
        // Private 메서드
        // 확인 버튼 눌렀을 때 실행되는 로직
        private void OnClickConfirm()
        {
            // 콜백 실행
            onConfirmCallback?.Invoke();

            // 패널 닫기
            gameObject.SetActive(false);
        }

        // 취소 버튼 눌렀을 때 실행되는 로직
        private void OnClickCancel()
        {
            // 패널 닫기만
            gameObject.SetActive(false);
        }
        // Others

    } // Scope by class RerollShopLockConfirmPanel

} // namespace Root