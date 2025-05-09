using Org.BouncyCastle.Asn1.X509;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
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
        [SerializeField] private bool m_IsStartTutorial = false;

        [SerializeField] private UiMgr uiMgr;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject[] tutorialBlocks;
        [SerializeField] private TutorialController tutorialController;
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




        [SerializeField] private TutorialClickListener listener; // 마스크 처리
        private bool oneGo = false;

        public GameObject allButtonsBlockPanel;
        public bool IsStartTutorial { get => m_IsStartTutorial; set => IsStartTutorial = value; }
        public bool TutorialEnd { get; private set; } = false; // 튜토리얼 종료 여부
        public int step { get; private set; } = 0;             // 현재 스텝
        private int currentTargetIndex = -1; // 현재 스텝의 버튼 인덱스

        private void Start()
        {
            TutorialEnd = TutorialEndValue.GetTutorialEnd();
            //m_IsStartTutorial = TutorialEndValue.GetIsStartTutorial();

           

            if (uiMgr == null)
            {
                uiMgr = GameMgr.FindObject<UiMgr>("InGameUI");
            }


            if (m_IsStartTutorial)
            {
                AttachClickListeners(); // 수정됨: 클릭 리스너 연결 메서드 호출
                ApplyStep(); // 첫 스텝 적용
            }
            else
            {
                var tutorialPanel = GameMgr.FindObject("TutorialPanel");
                tutorialPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (TutorialEnd)
            {
                tutorialController.TutorialObjDestroy();
            }

            if (!m_IsStartTutorial && !oneGo)
            {
                OffBlocks();
            }
            // 테스트용: Space 키로 다음 스텝 이동
            if (Input.GetKeyDown(KeyCode.Space))
            {
                step++;

                if (DataTableMgr.TutorialTable.Get(step) == null)
                {
                    Debug.Log("튜토리얼 종료");
                    TutorialEnd = true;
                    m_IsStartTutorial = false;
                    TutorialEndValue.SetValueIsStartTutorial(m_IsStartTutorial);
                    TutorialEndValue.SetValueTutorialEnd(TutorialEnd);
                    return;
                }

                ApplyStep();
            }
        }

        public void OnTutorialPanel()
        {
            tutorialPanel.SetActive(true);
            allButtonsBlockPanel.SetActive(false);
        }

        public void OffTutorialPanel()
        {
            // 패널 비활성화
            if (step <= 138)
            {
                allButtonsBlockPanel.SetActive(true);
            }
            
            tutorialPanel.SetActive(false);
            uiMgr.OnInGamePanels();

        }
        /// <summary>
        /// 마스크 또는 리스너에서 호출되는 외부 클릭 처리 메서드
        /// </summary>
        public void NotifyClicked(GameObject clicked)
        {
            if (!m_IsStartTutorial)
                return;

            var data = DataTableMgr.TutorialTable.Get(step);
            if (data == null || TutorialEnd) return;

            Debug.Log($"[튜토리얼] NotifyClicked 호출됨: {clicked.name}");

            if (data.ButtonIndex >= 0 && data.ButtonIndex < targetRects.Length)
            {
                var expected = targetRects[data.ButtonIndex];
                Debug.Log($"[튜토리얼] 클릭: {clicked.name}, 기대값: {expected.name}");

                if (clicked == expected.gameObject || clicked.transform.IsChildOf(expected))
                {
                    Debug.Log("[튜토리얼] 클릭 일치 → 스텝 증가");
                    StepUp();
                    ApplyStep();
                }
                else
                {
                    Debug.LogWarning("[튜토리얼] 클릭 대상 불일치");
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
            if (!m_IsStartTutorial)
                return;

            var data = DataTableMgr.TutorialTable.Get(step);
            if (data != null && data.ButtonIndex >= 0)
                return;

            step++;
            if (DataTableMgr.TutorialTable.Get(step) == null)
            {
                TutorialEnd = true;
                return;
            }
            ApplyStep();
        }

        // 현재 스텝에 버튼이 있는지 여부 반환 (수정됨)
        public bool HasButtonInCurrentStep()
        {
            if (!m_IsStartTutorial)
                return false;

            return currentTargetIndex >= 0 && currentTargetIndex < targetRects.Length;
        }

        public void StepUp()
        {
            if (!m_IsStartTutorial)
                return;

            step++;
        }
        /// <summary>
        /// 현재 스텝에 해당하는 튜토리얼 데이터를 적용
        /// </summary>
        public void ApplyStep()
        {
            if (!m_IsStartTutorial)
                return;

            var data = DataTableMgr.TutorialTable.Get(step);

            if (data == null)
            {
                TutorialEnd = true;
                return;
            }

            if (!data.IsActive)
            {
                OffTutorialPanel();

                return;
            }

            AttachClickListeners(); // 추가: 비활성 → 재활성된 UI에도 다시 리스너 적용

            if (data.ButtonIndex == 5 || data.ButtonIndex == 12 || data.ButtonIndex == 43)
            {
                StartCoroutine(DelayedAttachChildListeners(data.ButtonIndex));
            }


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

        public void AdvanceStepIfValid(GameObject clicked)
        {
            if (!m_IsStartTutorial)
                return;

            var data = DataTableMgr.TutorialTable.Get(step);
            if (data == null || TutorialEnd) return;

            if (data.ButtonIndex >= 0)
            {
                var target = targetRects[data.ButtonIndex];

                Transform clickedTr = clicked.transform;
                Transform targetTr = target.transform;

                if (clickedTr == targetTr || clickedTr.IsChildOf(targetTr) || targetTr.IsChildOf(clickedTr))
                {
                    StepUp();
                    ApplyStep();
                }
            }
            else
            {
                // 버튼 없는 스텝이면 그냥 진행
                StepUp();
                ApplyStep();
            }
        }

        public void SetTutorialEnd()
        {
            if (tutorialBlocks != null)
            {
                foreach (GameObject block in tutorialBlocks)
                {
                    Destroy(block);
                }
            }
            TutorialEnd = true;
            m_IsStartTutorial = false;
            TutorialEndValue.SetValueIsStartTutorial(m_IsStartTutorial);
            TutorialEndValue.SetValueTutorialEnd(TutorialEnd);
        }

        /// <summary>
        /// 수정됨: targetRects 배열을 순회하며 TutorialClickListener를 부착하고 참조 연결
        /// </summary>
        private void AttachClickListeners()
        {
            if (!m_IsStartTutorial)
                return;

            foreach (var rt in targetRects)
            {
                if (rt == null) continue;

                // 수정됨: 비활성 상태에서도 AddComponent 가능하게 하기 위해서 임시 활성화
                GameObject targetGO = rt.gameObject;

                bool wasInactive = !targetGO.activeSelf;
                if (wasInactive)
                    targetGO.SetActive(true);

                var listener = targetGO.GetComponent<TutorialClickListener>();
                if (listener == null)
                {
                    listener = targetGO.AddComponent<TutorialClickListener>();
                }

                listener.SetTutorialMgr(this);

                if (wasInactive)
                    targetGO.SetActive(false);
            }
        }

        private IEnumerator DelayedAttachChildListeners(int index)
        {
            yield return null; // 한 프레임 대기

            AttachChildListenersOfIndex(index);
        }

        private void AttachChildListenersOfIndex(int index)
        {
            if (!m_IsStartTutorial)
                return;

            if (index < 0 || index >= targetRects.Length) return;

            var parent = targetRects[index];
            foreach (Transform child in parent)
            {
                var rt = child.GetComponent<RectTransform>();
                if (rt == null) continue;

                var listener = rt.GetComponent<TutorialClickListener>();
                if (listener == null)
                    listener = rt.gameObject.AddComponent<TutorialClickListener>();

                listener.SetTutorialMgr(this);
            }
        }

        private void OffBlocks()
        {
            foreach (GameObject block in tutorialBlocks)
            {
                block.SetActive(false);
            }
            oneGo = true;
        }

    }


}
