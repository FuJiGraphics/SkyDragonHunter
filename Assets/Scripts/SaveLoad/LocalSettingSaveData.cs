using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public abstract class LocalSettingSaveData
    {
        public int MajorVersion { get; protected set; }
        public abstract LocalSettingSaveData VersionUp();
    } // Scope by class LocalSettingSaveData

    public class LocalSettingSaveDataV0 : LocalSettingSaveData
    {
        // Fields
        public float sfxVolume;
        public float bgmVolume;
        public int fpsSetting;

        public bool isVibrationEnabled;

        public LocalSettingSaveDataV0()
        {
            MajorVersion = 0;
            sfxVolume = 1f;
            bgmVolume = 1f;
        }
        public override LocalSettingSaveData VersionUp()
        {
            throw new System.NotImplementedException();
        }
    }

} // namespace Root