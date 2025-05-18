using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI
{
    public class UILockScreen : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private TextMeshProUGUI m_Battery;
        [SerializeField] private TextMeshProUGUI m_Wifi;
        [SerializeField] private GameObject m_Lock;
        [SerializeField] private GameObject m_Unlock;

        private float m_UpdateInterval = 1f;
        private float m_NextUpdateTime = 0f;
        private bool m_IsLock = false;

        // 속성 (Properties)
        public bool IsLock
        {
            get => m_IsLock;
            set
            {
                m_IsLock = value;
                UpdateLockedStatus();
            }
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            UpdateTimeDisplay();
            UpdateBatteryDisplay();
            UpdateWifiDisplay();
            UpdateLockedStatus();
        }

        private void Update()
        {
            if (Time.time >= m_NextUpdateTime)
            {
                UpdateTimeDisplay();
                UpdateBatteryDisplay();
                UpdateWifiDisplay();
                m_NextUpdateTime = Time.time + m_UpdateInterval;
            }
        }
        // Public 메서드
        public void OnSetActiveFalse()
        {
            if (m_IsLock)
                return;

            gameObject.SetActive(false);
        }

        // Private 메서드
        private void UpdateTimeDisplay()
        {
            DateTime now = DateTime.Now;
            m_Time.text = now.ToString("HH mm");
        }

        private void UpdateBatteryDisplay()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaObject activity = GetUnityActivity())
            using (AndroidJavaObject intentFilter = new AndroidJavaObject("android.content.IntentFilter", "android.intent.action.BATTERY_CHANGED"))
            using (AndroidJavaObject batteryStatusIntent = activity.Call<AndroidJavaObject>("registerReceiver", null, intentFilter))
            {
                int level = batteryStatusIntent.Call<int>("getIntExtra", "level", -1);
                int scale = batteryStatusIntent.Call<int>("getIntExtra", "scale", -1);

                if (level == -1 || scale == -1 || scale == 0)
                {
                    m_Battery.text = "Battery: N/A";
                }
                else
                {
                    int percentage = (int)((level / (float)scale) * 100f);
                    m_Battery.text = percentage + "%";
                }
            }
#else
            m_Battery.text = "100%";
#endif
        }

        private AndroidJavaObject GetUnityActivity()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

        private void UpdateWifiDisplay()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    try
    {
        using (AndroidJavaObject activity = GetUnityActivity())
        using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
        using (AndroidJavaObject connectivityManager = context.Call<AndroidJavaObject>("getSystemService", "connectivity"))
        using (AndroidJavaObject networkInfo = connectivityManager.Call<AndroidJavaObject>("getActiveNetworkInfo"))
        {
            if (networkInfo != null && networkInfo.Call<bool>("isConnected"))
            {
                int type = networkInfo.Call<int>("getType");

                if (type == 1) // TYPE_WIFI
                {
                    m_Wifi.text = "Wi-Fi";
                }
                else if (type == 0) // TYPE_MOBILE
                {
                    m_Wifi.text = "LTE";
                }
                else
                {
                    m_Wifi.text = "Other";
                }
            }
            else
            {
                m_Wifi.text = "Disconnected";
            }
        }
    }
    catch (Exception ex)
    {
        m_Wifi.text = "Unknown";
        Debug.LogWarning("Wi-Fi check failed: " + ex.Message);
    }
#else
            m_Wifi.text = "Disconnected";
#endif
        }

        private void UpdateLockedStatus()
        {
            if (m_IsLock)
            {
                m_Lock.SetActive(true);
                m_Unlock.SetActive(false);
            }
            else
            {
                m_Lock.SetActive(false);
                m_Unlock.SetActive(true);
            }
        }

        // Others

    } // Scope by class UILockScreen
} // namespace Root
