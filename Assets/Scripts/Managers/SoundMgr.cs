using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter.Managers {

    public enum SoundType
    {
        BGM,
        SFX,
    }

    public enum BGM
    {
        Main,
        Dungeon,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10,
        None,
    }

    public enum SFX
    {
        Click,
        None,
    }

    public class SoundMgr : MonoBehaviour
    {
        private static SoundMgr instance = null;
        public static SoundMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    var soundMgrGo = new GameObject(nameof(SoundMgr));
                    instance = soundMgrGo.AddComponent<SoundMgr>();
                    DontDestroyOnLoad(soundMgrGo);
                }
                return instance;
            }
        }

        private BGM playingBGM = BGM.None;

        public float BgmVol { get; private set; }
        public float SfxVol { get; private set; }

        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                Debug.LogWarning($"destroyed duplicate SoundMgr {gameObject}");
                return;
            }

            InitSources();
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var btn in FindObjectsOfType<Button>())
            {
                btn.onClick.RemoveListener(CallPlayClickSFX);
                btn.onClick.AddListener(CallPlayClickSFX);
            }
        }

        private void CallPlayClickSFX()
        {
            PlaySFX(SFX.Click);
        }

        private void InitSources()
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;

            SetVolume(SoundType.BGM, 1f);
            SetVolume(SoundType.SFX, 1f);
        }

        public void SetVolume(SoundType soundType, float newVol)
        {
            newVol = Mathf.Clamp01(newVol);
            switch (soundType)
            {
                case SoundType.BGM:
                    BgmVol = newVol;
                    bgmSource.volume = BgmVol;
                    break;
                case SoundType.SFX:
                    SfxVol = newVol;
                    sfxSource.volume = SfxVol;
                    break;
            }
        } // Scope by class SoundMgr

        public void PlayBGM(BGM bgm, float volume = -1f)
        {
            if(volume != -1)
            {
                SetVolume(SoundType.BGM, volume);
            }

            if(playingBGM == bgm)
            {
                Debug.Log($"trying to play currently playing bgm {bgm}, bgm not changed");
                return;
            }

            var audioSourceData = DataTableMgr.AudioSourceTable.Get(bgm);
            if (audioSourceData == null)
            {
                Debug.LogError($"AudioSourceData not found with BGM {bgm}");
                return;
            }

            var audioClip = ResourcesMgr.Load<AudioClip>(audioSourceData.AddressableKey);
            if(audioClip == null)
            {
                Debug.LogError($"AuduiClip not found with addressable key '{audioSourceData.AddressableKey}'");
                return;
            }
            bgmSource.clip = audioClip;
            bgmSource.Play();
            playingBGM = bgm;
            Debug.Log($"BGM changed to {bgm}");
        }

        public void PlaySFX(SFX sfx, float volume = -1f)
        {
            if (volume >= 0)
            {
                SetVolume(SoundType.SFX, volume);
            }            

            var audioSourceData = DataTableMgr.AudioSourceTable.Get(sfx);
            if (audioSourceData == null)
            {
                Debug.LogError($"AudioSourceData not found with SFX {sfx}");
                return;
            }

            var audioClip = ResourcesMgr.Load<AudioClip>(audioSourceData.AddressableKey);
            if (audioClip == null)
            {
                Debug.LogError($"AuduiClip not found with addressable key '{audioSourceData.AddressableKey}'");
                return;
            }
            sfxSource.PlayOneShot(audioClip);
        }
    } // namespace Root
}