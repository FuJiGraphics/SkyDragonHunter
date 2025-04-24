using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UIDungeonEnteredMsgPanel : MonoBehaviour
    {
        [SerializeField] private DungeonUIMgr m_DungeonUIMgr;

        // Public Methods
        public void SetUIMgr(DungeonUIMgr uiMgr)
        {
            m_DungeonUIMgr = uiMgr;
        }
    } // Scope by class UIDungeonEnteredMsgPanel

} // namespace Root