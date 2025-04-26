using SkyDragonHunter;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AFKRewardTest : MonoBehaviour
{
    // Fields
    [SerializeField] private Button m_AFKButton;
    [SerializeField] private TextMeshProUGUI m_AFKButtonText;
    [SerializeField] private TextMeshProUGUI m_AFKTimeText;
    [SerializeField] private int m_AFKTimeScaler = 1;
    [SerializeField] private Button m_ReduceScaler;
    [SerializeField] private Button m_IncreaseScaler;
    [SerializeField] private TextMeshProUGUI m_ScalerText;

    private bool m_IsAFKMode;

    private DateTime m_LastOnlineTime;

    // External Dependencies
    [SerializeField] private TestWaveController m_WaveController;


    private TimeSpan ElapsedTime => DateTime.UtcNow - m_LastOnlineTime;   
    private TimeSpan ModifiedElapsedTime
    {
        get
        {
            var result = ElapsedTime * m_AFKTimeScaler;
            return result > TimeSpan.FromHours(24) ? TimeSpan.FromHours(24) : result;
        }
    }

    private int TotalSeconds => Mathf.FloorToInt((float)ModifiedElapsedTime.TotalSeconds);
    private int TotalMinutes => TotalSeconds / 60;
    private int Seconds => TotalSeconds % 60;
    private int Minutes => (TotalSeconds % 3600) / 60;
    private int Hours => TotalSeconds / 3600;

    // Unity Methods
    private void Start()
    {
        Init();
        AddListeners();
    }

    private void Update()
    {
        UpdateAFKMode();
    }

    // Public Methods


    // Private Methods
    private void Init()
    {
        m_AFKTimeScaler = 50;
        m_ScalerText.text = m_AFKTimeScaler.ToString() + 'x';
        var waveControllerGo = GameMgr.FindObject("WaveController");
        m_WaveController = waveControllerGo.GetComponent<TestWaveController>();
    }
    private void AddListeners()
    {
        m_AFKButton.onClick.AddListener(OnClickAFKButton);
        m_ReduceScaler.onClick.AddListener(OnClickReduceScaler);
        m_IncreaseScaler.onClick.AddListener(OnClickIncreaseScaler);
    }

    private void UpdateAFKMode()
    {
        if (!m_IsAFKMode)
            return;

        StringBuilder sb = new StringBuilder();
        sb.Append(Hours.ToString("D2"));
        sb.Append(" : ");
        sb.Append(Minutes.ToString("D2"));
        sb.Append(" : ");
        sb.Append(Seconds.ToString("D2"));

        m_AFKTimeText.text = sb.ToString();
    }

    private void OnClickAFKButton()
    {
        if(m_IsAFKMode)
        {
            EndAFKMode();
        }
        else
        {
            StartAFKMode();
        }
    }

    private void OnClickReduceScaler()
    {
        if (m_AFKTimeScaler <= 1)
        {
            return;
        }
        if (m_AFKTimeScaler <= 10)
        {
            AddScalerValue(-1);
            return;
        }
        if (m_AFKTimeScaler <= 100)
        {
            AddScalerValue(-10);
            return;
        }
        if (m_AFKTimeScaler <= 1000)
        {
            AddScalerValue(-100);
            return;
        }
        if (m_AFKTimeScaler <= 10000)
        {
            AddScalerValue(-1000);
            return;
        }
    }
    private void OnClickIncreaseScaler()
    {
        if (m_AFKTimeScaler >= 10000)
        {
            return;
        }
        if (m_AFKTimeScaler >= 1000)
        {
            AddScalerValue(1000);
            return;
        }       
        if (m_AFKTimeScaler >= 100)
        {
            AddScalerValue(100);
            return;
        }
        if (m_AFKTimeScaler >= 10)
        {
            AddScalerValue(10);
            return;
        }
        else
        {
            AddScalerValue(1);
            return;
        }
    }

    private void AddScalerValue(int value)
    {
        m_AFKTimeScaler += value;
        m_ScalerText.text = m_AFKTimeScaler.ToString() + 'x';
    }

    private void StartAFKMode()
    {
        m_IsAFKMode = true;
        m_AFKButtonText.text = "휴식모드 종료";
        m_AFKButton.image.color = new Color(150, 255, 0);
        m_LastOnlineTime = DateTime.UtcNow;
        

    }

    private void EndAFKMode()
    {
        m_IsAFKMode = false;
        m_AFKButtonText.text = "휴식모드 시작";
        m_AFKButton.image.color = Color.white;
        m_AFKTimeText.text = "00 : 00 : 00";

        var stageTable = DataTableMgr.StageTable.Get(m_WaveController.LastTriedMissionLevel, m_WaveController.LastTriedZoneLevel);
        var afkRewardTable = DataTableMgr.AFKRewardTable.Get(stageTable.AFKRewardTableID);
        BigNum afkGoldPerMin = new BigNum(afkRewardTable.AFKGold);
        BigNum afkGold =  afkGoldPerMin * TotalMinutes;
       
        AccountMgr.Coin += afkGold;
        DrawableMgr.Dialog("휴식보상", $"{afkGold}({afkGoldPerMin} * {TotalMinutes})");
        Debug.Log($"AFK coin acquired {afkGold}({afkGoldPerMin} * {TotalMinutes})");
    }
}
