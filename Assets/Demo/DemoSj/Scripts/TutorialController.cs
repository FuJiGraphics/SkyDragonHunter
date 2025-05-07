using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class TutorialController : MonoBehaviour
    {
        // 필드 (Fields)
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
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnFirstActive()     
        { 
            firstActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnSecondActive()    
        { 
            secondActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnThirdActive()     
        { 
            thirdActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnFourthActive()    
        { 
            fourthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnFifthActive()     
        { 
            fifthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnSixthActive()     
        { 
            sixthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnSeventhActive()   
        { 
            seventhActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnEighthActive()    
        { 
            eighthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnNinthActive()     
        { 
            ninthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnTenthActive()     
        { 
            tenthActive = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }
        public void OnEndTutorial()     
        { 
            endTutorial = true;
            tutorialMgr.StepUp();
            tutorialMgr.ApplyStep();
            tutorialPanel.SetActive(true);
        }

        // Private 메서드
        // Others

    } // Scope by class TutorialController

} // namespace Root