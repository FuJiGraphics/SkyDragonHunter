using SkyDragonHunter.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle damageDisplayToggle;
    [SerializeField] private Toggle[] autoPowerSavingToggles;

    [SerializeField] private Button dataInitButton;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button outerPanelArea;

    public bool IsDrawDamage => damageDisplayToggle.isOn;

    private void Awake()
    {
        AddListeners();
    }

    public void InitSettingDisplay()
    {
        bgmSlider.value = SoundMgr.Instance.BgmVol;
        sfxSlider.value = SoundMgr.Instance.SfxVol;
        damageDisplayToggle.isOn = true;
        autoPowerSavingToggles[0].isOn = true;
        OnToggledDamageDisplay(SaveLoadMgr.GameData.savedAccountData.isDisplayDmg);
    }

    private void AddListeners()
    {
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChange);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChange);
        damageDisplayToggle.onValueChanged.AddListener(OnToggledDamageDisplay);
        autoPowerSavingToggles[0].onValueChanged.AddListener(OnToggledAutoPowerSaving0);
        autoPowerSavingToggles[1].onValueChanged.AddListener(OnToggledAutoPowerSaving1);
        autoPowerSavingToggles[2].onValueChanged.AddListener(OnToggledAutoPowerSaving2);
        autoPowerSavingToggles[3].onValueChanged.AddListener(OnToggledAutoPowerSaving3);
        dataInitButton.onClick.AddListener(OnClickedDataInit);
        closeButton.onClick.AddListener(OnClickedClose);
        outerPanelArea.onClick.AddListener(OnClickedClose);
    }
    
    private void OnBgmSliderChange(float val)
    {
        SoundMgr.Instance.SetVolume(SoundType.BGM, val);
    }
    private void OnSfxSliderChange(float val)
    {
        SoundMgr.Instance.SetVolume(SoundType.SFX, val);
    }
    private void OnToggledDamageDisplay(bool display)
    {
        if(display)
        {
            // Logics when displaying Damage            
        }
        else
        {
            // Logics when not displaying Damage;
        }
        SaveLoadMgr.GameData.savedAccountData.isDisplayDmg = display;
    }
    private void OnToggledAutoPowerSaving0(bool on)
    {
        SaveLoadMgr.GameData.savedAccountData.autoPowerSavingMins = 0;
    }
    private void OnToggledAutoPowerSaving1(bool on)
    {
        SaveLoadMgr.GameData.savedAccountData.autoPowerSavingMins = 2;
    }
    private void OnToggledAutoPowerSaving2(bool on)
    {
        SaveLoadMgr.GameData.savedAccountData.autoPowerSavingMins = 5;
    }
    private void OnToggledAutoPowerSaving3(bool on)
    {
        SaveLoadMgr.GameData.savedAccountData.autoPowerSavingMins = 10;
    }
    private void OnClickedDataInit()
    {

    }
    private void OnClickedClose()
    {
        gameObject.SetActive(false);
    }
}
