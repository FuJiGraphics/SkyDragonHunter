using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.test
{

    public class TestRandomPick : MonoBehaviour
    {
        // 필드 (Fields)
        public GameObject pickUpInfoPrefab;
        public Transform crewAndMoonStone;
        private int[] ints;
        private int count = 17;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if (crewAndMoonStone == null)
            {
                // "CrewAndMoonStone" 오브젝트를 계층 구조에서 찾아 할당
                crewAndMoonStone = GameObject
                    .Find("InGameUI/Canvas/SafeAreaPanel/SummonPanel/RandomPickCrewInfo/CrewAndMoonStone")
                    ?.transform;

                // 못 찾은 경우 경고 로그 출력
                if (crewAndMoonStone == null)
                {
                    Debug.LogWarning("CrewAndMoonStone을 찾을 수 없습니다. 경로를 확인하세요.");
                }
            }

            ints = new int[count];
            for (int i = 0; i < count - 1; i++)
            {
                ints[i] = i + 1;
            }
        }

        // Public 메서드
        public void RandomPick()
        {
            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            int selected = ints[Random.Range(0, ints.Length - 1)];

            // 숫자 드롭 생성
            CreatePickInfo(selected.ToString(), 1);

            // 문스톤 드롭 생성
            CreatePickInfo("문스톤", 1);
        }

        public void RandomTenPick()
        {
            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 각 숫자의 등장 횟수를 기록할 Dictionary
            Dictionary<int, int> pickUpCounts = new();

            // 10번 랜덤 추출 (중복 허용)
            for (int i = 0; i < 10; i++)
            {
                int selected = ints[Random.Range(0, ints.Length - 1)];

                // 이미 등장한 숫자면 +1, 아니면 1로 시작
                if (pickUpCounts.ContainsKey(selected))
                    pickUpCounts[selected]++;
                else
                    pickUpCounts[selected] = 1;
            }

            // 각 숫자마다 프리팹 생성
            foreach (var kvp in pickUpCounts)
            {
                // kvp.Key: 숫자, kvp.Value: 횟수
                CreatePickInfo(kvp.Key.ToString(), kvp.Value);
            }

            // 문스톤은 무조건 10개 드롭
            CreatePickInfo("문스톤", 10);
        }

        // Private 메서드
        private void CreatePickInfo(string crewName, int count)
        {
            // 프리팹 생성 및 부모 오브젝트 지정
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);

            // 프리팹에 붙어 있는 PickUpInfoUI 스크립트 가져오기
            var ui = instance.GetComponent<PickUpInfoUI>();

            // 스크립트가 존재하면 이름/수량 적용
            if (ui != null)
            {
                ui.SetData(crewName, count);
            }
        }
        // Others

    } // Scope by class TestRandomPick

} // namespace Root