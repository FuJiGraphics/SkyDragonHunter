using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class DungeonUIMgr : MonoBehaviour
    {
        // Fields
        [SerializeField] private UIDungeonButtonsPanel      m_ButtonsPanel;
        [SerializeField] private UIDungeonInfosPanel        m_InfoPanel;
        [SerializeField] private UIDungeonClearedPanel      m_ClearedPanel;
        [SerializeField] private UIDungeonFailedPanel       m_FailedPanel;
        [SerializeField] private UIDungeonPausedPanel       m_PausedPanel;
        [SerializeField] private UIDungeonEnteredMsgPanel   m_EnteredMsgPanel;

        // Properties
        private float TimeScale => m_ButtonsPanel.TimeScaleMultipler;

        // Unity Methods
        public void Start()
        {
            InitDungeonUIMgr();
        }

        // Public Methods
        public void EnablePausedPanel(bool enabled)
        {
            m_PausedPanel.enabled = enabled;
            Time.timeScale = enabled ? 0f : TimeScale;
        }

        // Private Methods
        private void InitDungeonUIMgr()
        {
            m_ButtonsPanel.SetUIMgr(this);
            m_InfoPanel.SetUIMgr(this);
            m_ClearedPanel.SetUIMgr(this);
            m_FailedPanel.SetUIMgr(this);
            m_PausedPanel.SetUIMgr(this);
            m_EnteredMsgPanel.SetUIMgr(this);

            m_ButtonsPanel.enabled = true;
            m_InfoPanel.enabled = true;
            m_ClearedPanel.enabled = false;
            m_FailedPanel.enabled = false;
            m_PausedPanel.enabled = false;
            m_EnteredMsgPanel.enabled = true;
        }
    } // Scope by class InDungeonUI

} // namespace Root