using Org.BouncyCastle.Asn1.Cmp;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class TutorialController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UiMgr uiMgr;
        [SerializeField] private GameObject tutorialRewardPanel;
        public GameObject tutorialPanel;
        public TutorialMgr tutorialMgr;
        public bool firstActive     { get; private set; } = false;
        public bool secondActive    { get; private set; } = false;
        public bool thirdActive     { get; private set; } = false;
        public bool fourthActive    { get; private set; } = false;
        public bool fifthActive     { get; private set; } = false;
        public bool sixthActive     { get; private set; } = false;
        public bool seventhActive   { get; private set; } = false;
        public bool eighthActive    { get; private set; } = false;
        public bool ninthActive     { get; private set; } = false;
        public bool tenthActive     { get; private set; } = false;
        public bool endTutorial     { get; private set; } = false;

        private float thirdTutorialStartTime;
        private float eighthTutorialStartTime;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            tutorialRewardPanel.SetActive(false);
        }
        private void Update()
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            if (tutorialMgr.tutorialEnd)
            {
                Destroy(tutorialPanel);
                Destroy(tutorialRewardPanel);
                Destroy(tutorialMgr);
                Destroy(this.gameObject);
            }

            if (!tutorialPanel.activeSelf && secondActive && !thirdActive && !endTutorial)
            {
                thirdTutorialStartTime += Time.deltaTime;
                if (thirdTutorialStartTime > 3f)
                {
                    OnThirdActive();
                    thirdTutorialStartTime = 0;
                }
            }

            if (!tutorialPanel.activeSelf && seventhActive && !eighthActive && !endTutorial)
            {
                eighthTutorialStartTime += Time.deltaTime;
                if (eighthTutorialStartTime > 1f)
                {
                    OnEighthActive();
                    tutorialRewardPanel.SetActive(true);
                    AccountMgr.Diamond += 3000;
                    eighthTutorialStartTime = 0;
                }
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                if (!tutorialPanel.activeSelf && eighthActive && !ninthActive && !endTutorial)
                {
                    OnNinthActive();
                }
            }
        }
        // Public 메서드
        public void OnFirstActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            firstActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnSecondActive()    
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            secondActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnThirdActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            thirdActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnFourthActive()    
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            fourthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnFifthActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            fifthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnSixthActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            sixthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnSeventhActive()   
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            seventhActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnEighthActive()    
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            eighthActive = true;
        }
        public void OnNinthActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            ninthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnTenthActive()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            tenthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialMgr.OnTutorialPanel();
        }
        public void OnEndTutorial()     
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            endTutorial = true;
        }

        public void OffTutorialRewardPanel()
        {
            if (!tutorialMgr.IsStartTutorial)
                return;

            tutorialRewardPanel.SetActive(false);
        }
        // Private 메서드
        // Others

    } // Scope by class TutorialController

} // namespace Root