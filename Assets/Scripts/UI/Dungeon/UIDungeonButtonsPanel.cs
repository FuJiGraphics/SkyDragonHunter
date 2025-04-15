using UnityEngine.UI;
using UnityEngine;

namespace SkyDragonHunter {

    public class UIDungeonButtonsPanel : MonoBehaviour
    {
        // Fields
        [SerializeField] private DungeonUIMgr   m_DungeonUIMgr;

        private float                           m_TimeScaleMultipler;

        [SerializeField] private Toggle         m_AcceleratorToggle;
        [SerializeField] private Button[]       m_SkillButtons;

        // Properties
        public float TimeScaleMultipler => m_TimeScaleMultipler;

        // UnityMethods
        private void Start()
        {
            Init();
            AddListeners();
        }

        // Public Methods
        public void SetUIMgr(DungeonUIMgr uiMgr)
        {
            m_DungeonUIMgr = uiMgr;
        }

        // Private Methods
        private void Init()
        {
            m_TimeScaleMultipler = 1f;
        }

        private void AddListeners()
        {
            m_AcceleratorToggle.onValueChanged.AddListener(OnToggledAccelerator);
            m_SkillButtons[0].onClick.AddListener(OnClickedSkill1);
            m_SkillButtons[1].onClick.AddListener(OnClickedSkill2);
            m_SkillButtons[2].onClick.AddListener(OnClickedSkill3);
            m_SkillButtons[3].onClick.AddListener(OnClickedSkill4);
        }

        private void OnToggledAccelerator(bool acceleratorOn)
        {
            if(acceleratorOn)
            {
                m_TimeScaleMultipler = 1.5f;
            }
            else
            {
                m_TimeScaleMultipler = 1f;
            }
        }

        private void OnClickedSkill1()
        {

        }

        private void OnClickedSkill2()
        {

        }

        private void OnClickedSkill3()
        {

        }

        private void OnClickedSkill4()
        {

        }


    } // Scope by class DungeonButtonPanel

} // namespace Root