using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces {

    public enum AilmentType
    {
        Stun,
        Exposed,
        Taunt,
        Burn, 
        Slow, 
        Freeze, 
        Max
    }

    public interface IAilmentEffect
    {
        public float Duration { get; }
        public float Tick { get; }
        public AilmentType Type { get; }
        public int CurrentStackCount { get; }
        public int MaxStackCount { get; }
        public bool IsRefreshable { get; }
        public bool IsStackable { get; }
        public GameObject Caster { get; }

    } // public interface IAilmentEffect
} // namespace SkyDragonHunter.Interfaces