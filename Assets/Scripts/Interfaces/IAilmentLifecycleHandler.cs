using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces {

    public interface IAilmentLifecycleHandler
    {
        public void OnEnter(GameObject receiver);
        public void OnStay();
        public void OnExit();

    } // public interface IAilmentLifecycleHandler
} // namespace SkyDragonHunter.Interfaces