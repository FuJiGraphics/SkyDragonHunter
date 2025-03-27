using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IAilmentAffectable
    {
        public void OnBurn(BurnStatusAilment status);
        public void OnFreeze(FreezeStatusAilment status);
        public void OnSlow(SlowStatusAilment status);

    } // Scope by interface IAilmentAffectable
} // namespace SkyDragonHunter.Interfaces