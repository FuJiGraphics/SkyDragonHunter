using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class DungeonUIMgr : MonoBehaviour
    {
        // Fields
        [SerializeField] private UIDungeonButtonsPanel m_ButtonsPanel;
        [SerializeField] private UIDungeonInfosPanel m_InfoPanel;
        [SerializeField] private UIDungeonClearedPanel m_ClearedPanel;
        [SerializeField] private UIDungeonFailedPanel m_FailedPanel;
        [SerializeField] private UIDungeonPausedPanel m_PausedPanel;
        [SerializeField] private UIDungeonEnteredMsgPanel m_EnteredMsgPanel;

        // Properties
        private float TimeScale => m_ButtonsPanel.TimeScaleMultipler;
        public UIDungeonInfosPanel InfoPanel => m_InfoPanel;

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

        public void OnDungeonClearFailed()
        {
            StartCoroutine(FailedTimeScaling());
        }

        public void EnableFailedPanel(bool enabled)
        {
            m_FailedPanel.gameObject.SetActive(enabled);
            m_InfoPanel.gameObject.SetActive(!enabled);
            m_ButtonsPanel.gameObject.SetActive(!enabled);
            Time.timeScale = enabled ? 0f : TimeScale;
        }

        // Private Methods
        private IEnumerator FailedTimeScaling()
        {
            float duration = 2f;
            float remaining = duration;

            while (remaining > 0f)
            {
                yield return null;
                remaining = Mathf.Max(0, remaining -= Time.unscaledDeltaTime);
                Time.timeScale = Mathf.Lerp(0f, 1f, remaining / duration);
            }
            EnableFailedPanel(true);
        }

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
            m_EnteredMsgPanel.gameObject.SetActive(false);
        }
    } // Scope by class InDungeonUI

} // namespace Root