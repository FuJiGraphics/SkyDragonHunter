using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    /// <summary>
    /// 튜토리얼의 전체 흐름을 제어하는 매니저 클래스
    /// </summary>
    public class TutorialMgr : MonoBehaviour
    {
        // 필드 (Fields)
        // 텍스트 위치 조정용 UI 오브젝트 (좌/중앙/우 앵커)
        [Header("텍스트 위치 앵커 오브젝트")]
        [SerializeField] private GameObject leftObj;     // 왼쪽 정렬용 오브젝트
        [SerializeField] private GameObject midObj;      // 중앙 정렬용 오브젝트
        [SerializeField] private GameObject rightObj;    // 오른쪽 정렬용 오브젝트

        // 캐릭터 이미지 관련 필드
        [Header("캐릭터 스프라이트")]
        [SerializeField] private Image character;        // 실제 출력될 캐릭터 이미지
        [SerializeField] private Sprite[] characters;    // 스프라이트 목록 (인덱스로 접근)

        // 메시지 출력용 텍스트
        [Header("튜토리얼 메시지")]
        [SerializeField] private TextMeshProUGUI tutorialText;

        // 마스크 구멍을 뚫을 대상 버튼들
        [Header("마스크 대상 버튼 배열")]
        [SerializeField] private Button[] buttons;

        // 구멍을 뚫는 마스크 제어용 컨트롤러
        [Header("구멍 마스크 컨트롤러")]
        [SerializeField] private SingleHoleMaskController holeMaskCtrl;

        // 현재 튜토리얼 단계
        private int step = 0;

        // ----------------------- 테스트용 코드 시작 -----------------------
        private int[] stepToButtonIndex; // step별로 사용할 랜덤 버튼 인덱스

        void Start()
        {
            // 테스트용: step마다 랜덤 버튼 지정
            int count = 10;
            stepToButtonIndex = new int[count];

            for (int i = 0; i < count; i++)
            {
                stepToButtonIndex[i] = Random.Range(0, buttons.Length);
            }

            ApplyTestStep(); // 처음 한 번 적용
        }

        void Update()
        {
            // Space 키로 step 증가
            if (Input.GetKeyDown(KeyCode.Space))
            {
                step++;

                if (DataTableMgr.TutorialTable.Get(step) == null)
                {
                    Debug.Log("튜토리얼 테스트 종료");
                    return;
                }

                ApplyTestStep();
            }
        }

        // 현재 step에 맞는 버튼에 마스크 적용
        private void ApplyTestStep()
        {
            int index = stepToButtonIndex[Random.Range(0, 9)];

            // 마스크 적용: 랜덤으로 정해둔 버튼 인덱스에 맞춰 구멍 뚫기
            if (index >= 0 && index < buttons.Length)
            {
                RectTransform target = buttons[index].GetComponent<RectTransform>();
                if (target != null)
                {
                    holeMaskCtrl.SetHole(target);
                    Debug.Log($"[테스트] step: {step}, 버튼 인덱스: {index}");
                }
            }

            // 튜토리얼 데이터 로드
            var data = DataTableMgr.TutorialTable.Get(step);

            // 캐릭터 스프라이트 설정
            if (data.Character >= 0 && data.Character < characters.Length)
            {
                character.sprite = characters[data.Character];
                character.gameObject.SetActive(true);
            }

            // 텍스트 위치 설정 (왼쪽/가운데/오른쪽 중 하나만 켬)
            leftObj.SetActive(data.LeftPanel);
            midObj.SetActive(data.MidPanel);
            rightObj.SetActive(data.RightPanel);

            // 대사 출력
            tutorialText.text = data.Dialogue;
        }

        // ----------------------- 테스트용 코드 끝 -----------------------

        //// 속성 (Properties)
        //// 외부 종속성 필드 (External dependencies field)
        //// 이벤트 (Events)
        //// 유니티 (MonoBehaviour 기본 메서드)
        //// Public 메서드
        ///// <summary>
        ///// 튜토리얼 시작. 첫 번째 단계부터 시작한다.
        ///// </summary>
        //public void StartTutorial()
        //{
        //    step = 0;
        //    ApplyStep();
        //}

        ///// <summary>
        ///// 다음 단계로 이동한다.
        ///// </summary>
        //public void NextStep()
        //{
        //    step++;

        //    // 테이블에 해당 step이 존재하지 않으면 튜토리얼 종료
        //    if (DataTableMgr.TutorialTable.Get(step) == null)
        //    {
        //        EndTutorial();
        //        return;
        //    }

        //    ApplyStep();
        //}

        //// Private 메서드
        ///// <summary>
        ///// 현재 step에 해당하는 튜토리얼 데이터를 UI에 반영한다.
        ///// </summary>
        //private void ApplyStep()
        //{
        //    var data = DataTableMgr.TutorialTable.Get(step); // 테이블에서 현재 step에 해당하는 데이터 조회

        //    // 캐릭터 스프라이트 설정
        //    if (data.Character >= 0 && data.Character < characters.Length)
        //    {
        //        character.sprite = characters[data.Character];
        //        character.gameObject.SetActive(true);
        //    }

        //    // 텍스트 위치 오브젝트 활성화 처리
        //    leftObj.SetActive(data.LeftPanel);
        //    midObj.SetActive(data.MidPanel);
        //    rightObj.SetActive(data.RightPanel);

        //    // 메시지 텍스트 적용
        //    tutorialText.text = data.Dialogue;

        //    // 해당 step에 맞는 버튼 기준으로 마스크 위치 설정
        //    if (step >= 0 && step < buttons.Length)
        //    {
        //        RectTransform target = buttons[step].GetComponent<RectTransform>();
        //        if (target != null)
        //            holeMaskCtrl.SetHole(target);
        //    }
        //}

        ///// <summary>
        ///// 튜토리얼 종료 시 처리.
        ///// 마스크와 메시지 등을 비활성화한다.
        ///// </summary>
        //private void EndTutorial()
        //{
        //    holeMaskCtrl.gameObject.SetActive(false); // 마스크 비활성화
        //    tutorialText.text = "";                   // 텍스트 초기화
        //    character.gameObject.SetActive(false);    // 캐릭터 이미지 숨김
        //}
        //// Others

    } // Scope by class TutorialMgr

} // namespace Root