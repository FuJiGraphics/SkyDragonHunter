
using SkyDragonHunter.Test;

namespace SkyDragonHunter.Interfaces {

    public interface ISaveLoadHandler
    {
        void OnSave(TempUserData database);
        void OnLoad(TempUserData database);

    } // Scope by interface ILoadedSaveData
} // namespace SkyDragonHunter.Interfaces