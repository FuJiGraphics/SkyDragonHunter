using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class AudioSourceData : DataTableData
    {
        public SoundType SoundType { get; set; }
        public string AddressableKey { get; set; }
        public BGM BgmType { get; set; }
        public SFX SfxType { get; set; }
    }

    public class AudioSourceTable : DataTable<AudioSourceData>
    {
        public AudioSourceData Get(BGM bgmType)
        {
            int defaultID = 10001;
            int result = defaultID + (int)bgmType;
            return Get(result);
        }

        public AudioSourceData Get(SFX sfxType)
        {
            int defaultID = 11001;
            int result = defaultID + (int)sfxType;
            return Get(result);
        }
    } // Scope by class AudioSourceTable

} // namespace Root