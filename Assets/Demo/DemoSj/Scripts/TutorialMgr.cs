using Org.BouncyCastle.Asn1.X509;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    /// <summary>
    /// 튜토리얼의 전체 흐름을 제어하는 매니저 클래스
    /// </summary>
    public class TutorialMgr : MonoBehaviour
    {
        [Header("튜토리얼 패널 오브젝트")]
        [SerializeField] private GameObject tutorialPanel;     // 왼쪽 정렬용
        [Header("텍스트 위치 앵커 오브젝트")]
        [SerializeField] private GameObject leftObj;     // 왼쪽 정렬용
        [SerializeField] private GameObject midObj;      // 중앙 정렬용
        [SerializeField] private GameObject rightObj;    // 오른쪽 정렬용

        [Header("캐릭터 스프라이트")]
        [SerializeField] private Image character;        // 출력용 캐릭터 이미지
        [SerializeField] private Sprite[] characters;    // 캐릭터 스프라이트 목록

        [Header("튜토리얼 메시지")]
        [SerializeField] private TextMeshProUGUI tutorialText; // 대사 텍스트

        [Header("구멍 대상 RectTransform 배열")]
        [SerializeField] private RectTransform[] targetRects; // 수정됨: 버튼이 아닌 UI 대응 위해 RectTransform 배열 사용

        [Header("구멍 마스크 컨트롤러")]
        [SerializeField] private SingleHoleMaskController holeMaskCtrl; // 마스크 처리

        public bool tutorialEnd { get; private set; } = false; // 튜토리얼 종료 여부
        public int step { get; private set; } = 0;             // 현재 스텝
        private int currentTargetIndex = -1; // 현재 스텝의 버튼 인덱스

        private void Start()
        {
            AttachClickListeners(); // 수정됨: 클릭 리스너 연결 메서드 호출
            ApplyStep(); // 첫 스텝 적용
        }

        private void Update()
        {
            // 테스트용: Space 키로 다음 스텝 이동
            if (Input.GetKeyDown(KeyCode.Space))
            {
                step++;

                if (DataTableMgr.TutorialTable.Get(step) == null)
                {
                    Debug.Log("튜토리얼 종료");
                    tutorialEnd = true;
                    return;
                }

                ApplyStep();
            }
        }

        /// <summary>
        /// 마스크 또는 리스너에서 호출되는 외부 클릭 처리 메서드
        /// </summary>
        public void NotifyClicked(GameObject clicked)
        {
            var data = DataTableMgr.TutorialTable.Get(step);
            if (data == null || tutorialEnd)
                return;

            if (data.ButtonIndex >= 0 && data.ButtonIndex < targetRects.Length)
            {
                if (targetRects[data.ButtonIndex].gameObject == clicked)
                {
                    StepUp();
                    ApplyStep();
                }
            }
        }

        public void OnTutorialMaskClicked()
        {
            Debug.Log($"[튜토리얼] 마스크 클릭 감지 → 현재 스텝: {step}");

            // 버튼 없는 상태에서만 처리
            var data = DataTableMgr.TutorialTable.Get(step);
            if (data == null) return;

            if (data.ButtonIndex < 0)
            {
                StepUp();
                ApplyStep();
            }
        }


        /// <summary>
        /// 버튼이 없는 경우 마스크를 눌러 다음 스텝으로 진행
        /// </summary>
        public void NextStepFromMask()
        {
            var data = DataTableMgr.TutorialTable.Get(step);
            if (data != null && data.ButtonIndex >= 0)
                return;

            step++;
            if (DataTableMgr.TutorialTable.Get(step) == null)
            {
                tutorialEnd = true;
                return;
            }
            ApplyStep();
        }

        // 현재 스텝에 버튼이 있는지 여부 반환 (수정됨)
        public bool HasButtonInCurrentStep()
        {
            return currentTargetIndex >= 0 && currentTargetIndex < targetRects.Length;
        }

        public void StepUp()
        {
            step++;
        }
        /// <summary>
        /// 현재 스텝에 해당하는 튜토리얼 데이터를 적용
        /// </summary>
        public void ApplyStep()
        {
            var data = DataTableMgr.TutorialTable.Get(step);
                        
            if (data == null)
            {
                tutorialEnd = true;
                return;
            }

            tutorialPanel.gameObject.SetActive(data.IsActive);
            if (!data.IsActive)
            {
                holeMaskCtrl.DisableHole(); // 구멍도 꺼줘야 함
                return;
            }

            AttachClickListeners(); // 추가: 비활성 → 재활성된 UI에도 다시 리스너 적용

            if (data.Character >= 0 && data.Character < characters.Length)
            {
                character.sprite = characters[data.Character];
                character.gameObject.SetActive(true);
            }

            leftObj.SetActive(data.LeftPanel);
            midObj.SetActive(data.MidPanel);
            rightObj.SetActive(data.RightPanel);

            tutorialText.text = data.Dialogue;

            currentTargetIndex = data.ButtonIndex;

            RectTransform target = null;
            if (data.ButtonIndex >= 0 && data.ButtonIndex < targetRects.Length)
            {
                target = targetRects[data.ButtonIndex];
            }

            if (target != null)
            {
                holeMaskCtrl.SetHole(target);
                return;
            }

            holeMaskCtrl.DisableHole();
        }

        /// <summary>
        /// 수정됨: targetRects 배열을 순회하며 TutorialClickListener를 부착하고 참조 연결
        /// </summary>
        private void AttachClickListeners()
        {
            foreach (var rt in targetRects)
            {
                if (rt == null) continue;

                var listener = rt.GetComponent<TutorialClickListener>();
                if (listener == null)
                    listener = rt.gameObject.AddComponent<TutorialClickListener>();

                listener.SetTutorialMgr(this);
            }
        }
    }
}
