using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables {

    public class TutorialTableData : DataTableData
    {
        //public int ID;                                    // 현재 단계 (step 값과 비교)
        public int          Character { get; set; }         // 스프라이트 인덱스
        public bool         LeftPanel { get; set; }
        public bool         MidPanel { get; set; }
        public bool         RightPanel { get; set; }
        public string       Dialogue { get; set; }          // 대사
        public int          ButtonIndex { get; set; }          // 버튼 인덱스
        public bool         IsActive { get; set; }          // 게임오브젝트 활성
    }

    public class TutorialTable : DataTable<TutorialTableData>
    {
       
    } // Scope by class TutorialTable

} // namespace Root