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
        public void Awake()
        {
            InitDungeonUIMgr();
        }

        // Public Methods
        public void EnablePausedPanel(bool enabled)
        {
            m_PausedPanel.gameObject.SetActive(enabled);
            Time.timeScale = enabled ? 0f : TimeScale;
        }

        public void EnableClearedPanel(bool enabled)
        {
            m_ClearedPanel.gameObject.SetActive(enabled);
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

            m_ButtonsPanel.gameObject.SetActive(true);
            m_InfoPanel.gameObject.SetActive(true);
            m_ClearedPanel.gameObject.SetActive(false);
            m_FailedPanel.gameObject.SetActive(false);
            m_PausedPanel.gameObject.SetActive(false);
            m_EnteredMsgPanel.gameObject.SetActive(true);
        }
    } // Scope by class InDungeonUI

} // namespace Root